﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Utilities.Encoders;
using Xrpl.AddressCodec;
using Xrpl.BinaryCodec.Types;
using Xrpl.Client;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Common;
using Xrpl.Models.Subscriptions;
using static System.Net.Mime.MediaTypeNames;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/createMockRippled.ts

namespace XrplTests.Xrpl
{
    public class CreateMockRippled
    {
        public int _port;
        private TcpListener _listener;
        private Dictionary<string, dynamic> _responses = new Dictionary<string, dynamic>();
        private bool suppressOutput = false;
        private Thread tcpListenerThread;

        public CreateMockRippled(int port)
        {
            this._port = port;
        }

        string CreateResponse(Dictionary<string, dynamic> request, Dictionary<string, dynamic> response)
        {
            var cloneResp = response;
            if (response["type"] == null && response["error"] == null)
            {
                throw new AddressCodecException($"Bad response format. Must contain `type` or `error`. {response}");
            }
            //cloneResp["id"] = request["id"];
            cloneResp["id"] = 1;
            return JsonConvert.SerializeObject(cloneResp);
        }

        void AddResponse(string command, Dictionary<string, dynamic> response)
        {
            if (response["type"] == null && response["error"] == null)
            {
                throw new AddressCodecException($"Bad response format. Must contain `type` or `error`. {response}");
            }
            _responses[command] = response;
        }

        Dictionary<string, dynamic> GetResponse(Dictionary<string, dynamic> request)
        {
            string command = request["command"];
            if (command == null)
            {
                throw new AddressCodecException($"No handler for {command}");
            }
            Dictionary<string, dynamic> functionOrObject = this._responses[command];
            //if (functionOrObject is Func)
            //{
            //    return functionOrObject(request) as Dictionary<string, dynamic>;
            //}
            return functionOrObject;
        }

        void TestCommand(TcpClient client, Dictionary<string, dynamic> request)
        {
            Dictionary<string, dynamic> data = request["data"];
            if (data["disconnectIn"] != null) {
                Dictionary<string, dynamic> response = new Dictionary<string, dynamic>
                {
                    { "result", null },
                    { "status", "Success" },
                    { "type", "response" },
                };
                string responseString = CreateResponse(request, response);
                //client.send();
            }
        }

        void Send(NetworkStream netstr, string message)
        {
            try
            {
                byte[] send = Encoding.UTF8.GetBytes(message);
                netstr.Write(send, 0, send.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void Ping(NetworkStream netstr, Dictionary<string, dynamic> request)
        {
            Dictionary<string, dynamic> response = new Dictionary<string, dynamic>
            {
                { "result", null },
                { "status", "Success" },
                { "type", "response" },
            };
            Send(netstr, JsonConvert.SerializeObject(response));
        }

        public async void Start()
        {
            this._listener = new TcpListener(IPAddress.Parse("127.0.0.1"), this._port);
            this._listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            this._listener.Start();

            tcpListenerThread = new Thread(() =>
            {
                TcpClient client = this._listener.AcceptTcpClient();
                Console.WriteLine("A client connected.");

                NetworkStream stream = client.GetStream();

                while (true)
                {
                    while (!stream.DataAvailable);
                    while (client.Available < 3); // match against "get"

                    byte[] bytes = new byte[client.Available];
                    stream.Read(bytes, 0, client.Available);
                    string s = Encoding.UTF8.GetString(bytes);

                    if (Regex.IsMatch(s, "^GET", RegexOptions.IgnoreCase))
                    {
                        Console.WriteLine("=====Handshaking from client=====\n{0}", s);
                        // 1. Obtain the value of the "Sec-WebSocket-Key" request header without any leading or trailing whitespace
                        // 2. Concatenate it with "258EAFA5-E914-47DA-95CA-C5AB0DC85B11" (a special GUID specified by RFC 6455)
                        // 3. Compute SHA-1 and Base64 hash of the new value
                        // 4. Write the hash back as the value of "Sec-WebSocket-Accept" response header in an HTTP response
                        string swk = Regex.Match(s, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
                        string swka = swk + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                        byte[] swkaSha1 = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(swka));
                        string swkaSha1Base64 = Convert.ToBase64String(swkaSha1);

                        // HTTP/1.1 defines the sequence CR LF as the end-of-line marker
                        byte[] response = Encoding.UTF8.GetBytes(
                            "HTTP/1.1 101 Switching Protocols\r\n" +
                            "Connection: Upgrade\r\n" +
                            "Upgrade: websocket\r\n" +
                            "Sec-WebSocket-Accept: " + swkaSha1Base64 + "\r\n\r\n");

                        stream.Write(response, 0, response.Length);
                    }
                    else
                    {
                        bool fin = (bytes[0] & 0b10000000) != 0,
                        mask = (bytes[1] & 0b10000000) != 0; // must be true, "All messages from the client to the server have this bit set"
                        int opcode = bytes[0] & 0b00001111, // expecting 1 - text message
                            offset = 2;
                        ulong msglen = (ulong)(bytes[1] & 0b01111111);

                        if (msglen == 126)
                        {
                            // bytes are reversed because websocket will print them in Big-Endian, whereas
                            // BitConverter will want them arranged in little-endian on windows
                            msglen = BitConverter.ToUInt16(new byte[] { bytes[3], bytes[2] }, 0);
                            offset = 4;
                        }
                        else if (msglen == 127)
                        {
                            // To test the below code, we need to manually buffer larger messages — since the NIC's autobuffering
                            // may be too latency-friendly for this code to run (that is, we may have only some of the bytes in this
                            // websocket frame available through client.Available).
                            msglen = BitConverter.ToUInt64(new byte[] { bytes[9], bytes[8], bytes[7], bytes[6], bytes[5], bytes[4], bytes[3], bytes[2] }, 0);
                            offset = 10;
                        }

                        if (msglen == 0)
                        {
                            Console.WriteLine("msglen == 0");
                        }
                        else if (mask)
                        {
                            byte[] decoded = new byte[msglen];
                            byte[] masks = new byte[4] { bytes[offset], bytes[offset + 1], bytes[offset + 2], bytes[offset + 3] };
                            offset += 4;

                            for (ulong i = 0; i < msglen; ++i)
                                decoded[i] = (byte)(bytes[(ulong)offset + i] ^ masks[i % 4]);

                            string jsonStr = Encoding.ASCII.GetString(decoded);
                            dynamic request = null;
                            try
                            {
                                request = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonStr);
                                //throw new Exception("HERE1");
                                if (request["id"] == null)
                                {
                                    throw new XrplError($"Request has no id: {JsonConvert.SerializeObject(request)}");
                                }
                                Console.WriteLine(request["command"]);
                                if (request["command"] == null)
                                {
                                    throw new XrplError($"Request has no command: {JsonConvert.SerializeObject(request)}");
                                }
                                if (request["command"] == "ping")
                                {
                                    Ping(stream, request);
                                }
                                else if (request["command"] == "test_command")
                                {
                                    Console.WriteLine("TEST COMMAND");
                                }
                                else if (this._responses.ContainsKey(request["command"]))
                                {
                                    throw new Exception("THIS NOT");
                                    Console.WriteLine("SUPPOSED TO BE HERE");
                                }
                                else
                                {
                                    throw new XrplError($"No event handler registered in mock rippled for ${request["command"]}");
                                }
                            }
                            catch (XrplError err)
                            {
                                if (!this.suppressOutput)
                                {
                                    Console.WriteLine($"{err}");
                                }
                                if (request != null)
                                {
                                    Dictionary<string, dynamic> errorResponse = new Dictionary<string, dynamic>
                                    {
                                        { "type", "response" },
                                        { "status", "error" },
                                        { "error", err.Message },
                                    };
                                    Send(stream, CreateResponse(request, errorResponse));
                                }
                            }
                            catch (Exception err)
                            {
                                throw err;
                            }
                        }
                        else
                            Console.WriteLine("mask bit not set");
                    }
                    break;
                }
            });
            tcpListenerThread.Start();
        }
    }
}
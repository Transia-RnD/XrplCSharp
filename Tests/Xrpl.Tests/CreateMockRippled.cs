using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities.Net;
using Xrpl.AddressCodec;
using Xrpl.Client.Exceptions;
using XrplTests.Xrpl.MockRippled;
using IPAddress = System.Net.IPAddress;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/createMockRippled.ts

namespace XrplTests.Xrpl
{
    public static class Logger
    {
        static readonly TextWriter tw;

        static Logger()
        {
            string _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            tw = TextWriter.Synchronized(File.AppendText(_filePath + "/Log.txt"));
        }

        public static void Write(string logMessage)
        {
            try
            {
                Log(logMessage, tw);
            }
            catch (IOException e)
            {
                tw.Close();
            }
        }

        private static readonly object _syncObject = new object();

        public static void Log(string logMessage, TextWriter w)
        {
            // only one thread can own this lock, so other threads
            // entering this method will wait here until lock is
            // available.
            lock (_syncObject)
            {
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                w.WriteLine("  :");
                w.WriteLine("  :{0}", logMessage);
                w.WriteLine("-------------------------------");
                // Update the underlying file.
                w.Flush();
            }
        }
    }

    public class CreateMockRippled
    {
        public int _port;
        private TcpListener _listener;
        private Dictionary<string, dynamic> _responses = new Dictionary<string, dynamic>();
        public bool suppressOutput = false;
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
                throw new AddressCodecException($"Bad response format. Must contain `type` or `error`. {response["type"]}");
            }
            cloneResp["id"] = request["id"];
            return JsonConvert.SerializeObject(cloneResp);
        }

        public void AddResponse(string command, Dictionary<string, dynamic> response)
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

        void TestCommand(Client client, Dictionary<string, dynamic> request)
        {
            JToken jdata = request["data"];
            Dictionary<string, dynamic> data = jdata.ToObject<Dictionary<string, dynamic>>();

            data.TryGetValue("disconnectIn", out var disconnectIn);
            data.TryGetValue("openOnOtherPort", out var openOnOtherPort);
            data.TryGetValue("closeServerAndReopen", out var closeServerAndReopen);
            data.TryGetValue("unrecognizedResponse", out var unrecognizedResponse);
            data.TryGetValue("closeServer", out var closeServer);
            data.TryGetValue("delayedResponseIn", out var delayedResponseIn);

            if (disconnectIn != null) {
                Dictionary<string, dynamic> response = new Dictionary<string, dynamic>
                {
                    { "result", new Dictionary<string, dynamic>() {} },
                    { "status", "Success" },
                    { "type", "response" },
                };
                string responseString = CreateResponse(request, response);
                this.Send(client, responseString);
            }
            if (openOnOtherPort != null)
            {
                Dictionary<string, dynamic> response = new Dictionary<string, dynamic>
                {
                    { "result", new Dictionary<string, dynamic>() {
                        { "port", 9999 }
                    } },
                    { "status", "Success" },
                    { "type", "response" },
                };
                string responseString = CreateResponse(request, response);
                this.Send(client, responseString);
            }
            if (closeServerAndReopen != null)
            {
                Dictionary<string, dynamic> response = new Dictionary<string, dynamic>
                {
                    { "result", new Dictionary<string, dynamic>() {} },
                    { "status", "Success" },
                    { "type", "response" },
                };
                string responseString = CreateResponse(request, response);
                this.Send(client, responseString);
            }
            if (unrecognizedResponse != null)
            {
                Dictionary<string, dynamic> response = new Dictionary<string, dynamic>
                {
                    { "result", new Dictionary<string, dynamic>() {} },
                    { "status", "Success" },
                    { "type", "response" },
                };
                string responseString = CreateResponse(request, response);
                this.Send(client, responseString);
            }
            if (closeServer != null)
            {
                client.GetSocket().Close();
                //this._listener.Stop();
                //client.Close();
                //netstr.Dispose();
            }
            if (delayedResponseIn != null)
            {
                Dictionary<string, dynamic> response = new Dictionary<string, dynamic>
                {
                    { "result", new Dictionary<string, dynamic>() {} },
                    { "status", "Success" },
                    { "type", "response" },
                };
                string responseString = CreateResponse(request, response);
                this.Send(client, responseString);
            }
        }

        void Send(Client client, string message)
        {
            try
            {
                Console.WriteLine($"SERVER SEND: {message}");
                client.GetServer().SendMessage(client, message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void Ping(Client client, Dictionary<string, dynamic> request)
        {
            Dictionary<string, dynamic> response = new Dictionary<string, dynamic>
            {
                { "result", null },
                { "status", "Success" },
                { "type", "response" },
            };
            Send(client, JsonConvert.SerializeObject(response));
        }
        public void Start()
        {

            Server server = new Server(new IPEndPoint(IPAddress.Parse("127.0.0.1"), this._port));

            // Bind the event for when a client connected
            server.OnClientConnected += (object sender, OnClientConnectedHandler e) =>
            {
                string clientGuid = e.GetClient().GetGuid();
                //Console.WriteLine($"Client with guid {clientGuid} connected!");
            };

            // Bind the event for when a message is received
            server.OnMessageReceived += (object sender, OnMessageReceivedHandler e) =>
            {
                string jsonStr = e.GetMessage();
                Console.WriteLine($"SERVER RECV: {jsonStr}");
                Dictionary<string, dynamic> request = null;
                try
                {
                    request = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonStr);
                    var _command = request.TryGetValue("command", out var command);
                    if (request["id"] == null)
                    {
                        throw new XrplError($"Request has no id: {JsonConvert.SerializeObject(request)}");
                    }
                    if (!_command)
                    {
                        throw new XrplError($"Request has no command: {JsonConvert.SerializeObject(request)}");
                    }
                    if (command == "ping")
                    {
                        Ping(e.GetClient(), request);
                    }
                    else if (command == "test_command")
                    {
                        this.TestCommand(e.GetClient(), request);
                    }
                    else if (this._responses.ContainsKey(command))
                    {
                        this.Send(e.GetClient(), this.CreateResponse(request, this.GetResponse(request)));
                    }
                    else
                    {
                        throw new XrplError($"No event handler registered in mock rippled for {request["command"]}");
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
                            { "error", err.Message.ToString() },
                        };
                        this.Send(e.GetClient(), CreateResponse(request, errorResponse));
                        return;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            };

            // Bind the event for when a client connected
            server.OnSendMessage += (object sender, OnSendMessageHandler e) =>
            {
                string clientGuid = e.GetClient().GetGuid();
                //Console.WriteLine($"Server sent message to client {clientGuid}!");
            };

            // Bind the event for when a client disconnected
            server.OnClientDisconnected += (object sender, OnClientDisconnectedHandler e) =>
            {
                //e.GetClient().GetSocket().Close();
                //e.GetClient().GetSocket().Dispose();
                //e.GetClient().GetServer().ClientDisconnect(e.GetClient());
                string clientGuid = e.GetClient().GetGuid();
                //Console.WriteLine($"Client with guid {clientGuid} disconnected!");
            };
        }
    }
}
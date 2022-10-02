using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Xrpl.AddressCodecLib;
using Xrpl.ClientLib;
using Xrpl.ClientLib.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/createMockRippled.ts

namespace XrplTests.Xrpl
{
    public class CreateMockRippled
    {
        public int _port;
        private TcpListener _listener;

        private Queue<Action<byte[]>> _requestHandles;
        private Queue<byte[]> _response;

        public CreateMockRippled(int port)
        {
            this._port = port;
            this._requestHandles = new Queue<Action<byte[]>>();
            this._response = new Queue<byte[]>();
        }

        public string CreateResponse(Dictionary<string, dynamic> request, Dictionary<string, dynamic> response)
        {
            var cloneResp = response;
            if (response["type"] == null && response["error"] == null)
            {
                throw new AddressCodecException($"Bad response format. Must contain `type` or `error`. {response}");
            }
            cloneResp["id"] = request["id"];
            return JsonConvert.SerializeObject(cloneResp);
        }

        public async void Start()
        {
            Debug.WriteLine(IPAddress.Parse("127.0.0.1"));
            this._listener = new TcpListener(IPAddress.Any, this._port);
            this._listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            this._listener.Start();

            this._listener.BeginAcceptTcpClient(o =>
            {
                Console.WriteLine("ACCEPTING");

                TcpClient client = null;
                try
                {
                    client = this._listener.EndAcceptTcpClient(o);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                if (client == null) return;

                using (var stream = client.GetStream())
                {
                    while (true)
                    {
                        
                        //var requestHandle = this._requestHandles.Count > 0
                        //    ? this._requestHandles.Dequeue()
                        //    : null;

                        //if (requestHandle == null)
                        //    break;

                        byte[] buffer = new byte[100000];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        string jsonStr = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        throw new XrplError(jsonStr);
                        Dictionary<string, dynamic> request = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonStr);

                        if (request["id"] == null)
                        {
                            throw new XrplError($"Request has no id: {request.ToString()}");
                        }

                        if (request["command"] == null)
                        {
                            throw new XrplError($"Request has no command: {request.ToString()}");
                        }

                        //parse(request);
                        Dictionary<string, dynamic> request1 = new Dictionary<string, dynamic>
                        {
                            { "command", "ping" }
                        };

                        //throw new XrplError($"No event handler registered in mock rippled for {request1["command"]}");


                        //throw new XrplError(data);
                        //Debug.WriteLine("RE");
                        //throw new Exception("STREAM");

                        //TODO: DUMP
                        Console.WriteLine("[MockServer] Packet DUMP:" + bytesRead);

                        //if (requestHandle != null)
                        //    requestHandle(request);

                        //var response = this._response.Count > 0
                        //    ? this._response.Dequeue()
                        //    : null;
                        //if (response != null)
                        //    stream.Write(response, 0, response.Length);
                    }
                }
            }, null);
        }

        //public void Receive(TcpClient conn)
        //{
        //    conn.
        //}

        public void Stop()
        {
            this._listener.Stop();
        }
    }
}


//using (var stream = client.GetStream())
//{
//    while (true)
//    {

//        //var requestHandle = this._requestHandles.Count > 0
//        //    ? this._requestHandles.Dequeue()
//        //    : null;

//        //if (requestHandle == null)
//        //    break;

//        byte[] buffer = new byte[100000];
//        int bytesRead = stream.Read(buffer, 0, buffer.Length);
//        string jsonStr = Encoding.ASCII.GetString(buffer, 0, bytesRead);
//        throw new XrplError(jsonStr);
//        Dictionary<string, dynamic> request = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonStr);

//        if (request["id"] == null)
//        {
//            throw new XrplError($"Request has no id: {request.ToString()}");
//        }

//        if (request["command"] == null)
//        {
//            throw new XrplError($"Request has no command: {request.ToString()}");
//        }

//        //parse(request);
//        Dictionary<string, dynamic> request1 = new Dictionary<string, dynamic>
//                        {
//                            { "command", "ping" }
//                        };

//        //throw new XrplError($"No event handler registered in mock rippled for {request1["command"]}");


//        //throw new XrplError(data);
//        //Debug.WriteLine("RE");
//        //throw new Exception("STREAM");

//        //TODO: DUMP
//        Console.WriteLine("[MockServer] Packet DUMP:" + bytesRead);

//        //if (requestHandle != null)
//        //    requestHandle(request);

//        //var response = this._response.Count > 0
//        //    ? this._response.Dequeue()
//        //    : null;
//        //if (response != null)
//        //    stream.Write(response, 0, response.Length);
//    }
//}
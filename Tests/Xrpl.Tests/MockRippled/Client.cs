// Copyright © 2017 - MazyModz. Created by Dennis Andersson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Org.BouncyCastle.Bcpg;
using System.Drawing;

namespace Xrpl.Tests.MockRippled
{
    ///<summary>
    /// Object for all connectecd clients
    /// </summary>
    public partial class MockClient
    {

        #region Fields

        ///<summary>The socket of the connected client</summary>
        private Socket _socket;

        ///<summary>The guid of the connected client</summary>
        private string _guid;

        /// <summary>The server that the client is connected to</summary>
        private Server _server;

        /// <summary>If the server has sent a ping to the client and is waiting for a pong</summary>
        private bool _bIsWaitingForPong;

        #endregion

        #region Class Events

        /// <summary>Create a new object for a connected client</summary>
        /// <param name="Server">The server object instance that the client is connected to</param>
        /// <param name="Socket">The socket of the connected client</param>
        public MockClient(Server Server, Socket Socket)
        {
            this._server = Server;
            this._socket = Socket;
            this._guid = Helpers.CreateGuid("client");

            // Start to detect incomming messages
            GetSocket().BeginReceive(new byte[] { 0 }, 0, 0, SocketFlags.None, messageCallback, null);
        }

        #endregion

        #region Field Getters

        /// <summary>Gets the guid of the connected client</summary>
        /// <returns>The GUID of the client</returns>
        public string GetGuid()
        {
            return _guid;
        }

        ///<summary>Gets the socket of the connected client</summary>
        ///<returns>The socket of the client</return>
        public Socket GetSocket()
        {
            return _socket;
        }

        /// <summary>The socket that this client is connected to</summary>
        /// <returns>Listen socket</returns>
        public Server GetServer()
        {
            return _server;
        }

        /// <summary>Gets if the server is waiting for a pong response</summary>
        /// <returns>If the server is waiting for a pong response</returns>
        public bool GetIsWaitingForPong()
        {
            return _bIsWaitingForPong;
        }

        #endregion

        #region Field Setters

        /// <summary>Sets if the server is waiting for a pong response</summary>
        /// <param name="bIsWaitingForPong">If the server is waiting for a pong response</param>
        public void SetIsWaitingForPong(bool bIsWaitingForPong)
        {
            _bIsWaitingForPong = bIsWaitingForPong;
        }

        #endregion

        #region Methods

        /// <summary>Called when a message was received from the client</summary>
        private void messageCallback(IAsyncResult AsyncResult)
        {
            try
            {
                GetSocket().EndReceive(AsyncResult);

                // Read the incomming message
                byte[] messageBuffer = new byte[1024 * 10];
                if (messageBuffer.Length == 0)
                {
                    return;
                }
                int bytesReceived = GetSocket().Receive(messageBuffer, messageBuffer.Length, SocketFlags.None);

                if (bytesReceived == 0)
                {
                    return;
                }

                // Resize the byte array to remove whitespaces 
                if (bytesReceived < messageBuffer.Length) Array.Resize<byte>(ref messageBuffer, bytesReceived);

                // Get the opcode of the frame
                EOpcodeType opcode = Helpers.GetFrameOpcode(messageBuffer);

                // If the connection was closed
                if (opcode == EOpcodeType.Pong)
                {
                    return;
                }

                if (opcode == EOpcodeType.ClosedConnection)
                {
                    GetServer().ClientDisconnect(this);
                    return;
                }

                // Pass the message to the server event to handle the logic
                var readableBytes = messageBuffer.Length;
                while (readableBytes > 0)
                {
                    // Get the frame data
                    //Debug.WriteLine($"AVAIL LEN: {readableBytes}");
                    SFrameMaskData frameData = Helpers.GetFrameData(messageBuffer);

                    // Get the decode frame key from the frame data
                    byte[] decodeKey = new byte[4];
                    for (int i = 0; i < 4; i++) decodeKey[i] = messageBuffer[frameData.KeyIndex + i];

                    int dataIndex = frameData.KeyIndex + 4;
                    int count = 0;

                    //Debug.WriteLine($"DATA INDEX: {dataIndex}");

                    // Decode the data using the key
                    for (int i = dataIndex; i < frameData.TotalLenght; i++)
                    {
                        //Debug.WriteLine($"DECODE KEY: {i}");
                        messageBuffer[i] = (byte)(messageBuffer[i] ^ decodeKey[count % 4]);
                        count++;
                    }

                    // Return the decoded message
                    //Debug.WriteLine($"READING: {frameData.TotalLenght}");

                    string message = Encoding.Default.GetString(messageBuffer, dataIndex, frameData.DataLength);
                    GetServer().ReceiveMessage(this, message);
                    readableBytes -= frameData.TotalLenght;

                    byte[] newArray = new byte[messageBuffer.Length - frameData.TotalLenght];
                    Buffer.BlockCopy(messageBuffer, frameData.TotalLenght, newArray, 0, newArray.Length);
                    messageBuffer = newArray;
                }

                // Start to receive messages again
                GetSocket().BeginReceive(new byte[] { 0 }, 0, 0, SocketFlags.None, messageCallback, null);

            }
            catch (Exception exception)
            {
                Debug.WriteLine($"MOCK CLIENT ERROR: {exception.Message}");
                GetSocket().Close();
                GetSocket().Dispose();
                GetServer().ClientDisconnect(this);
            }
        }

        #endregion

    }
}
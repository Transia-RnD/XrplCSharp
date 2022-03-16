using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RippleDotNet
{
    //credit: https://github.com/Badiboy/WebSocketWrapper/blob/master/WebSocketWrapper.cs
    internal class WebSocketClient : IDisposable
    {

        private const int ReceiveChunkSize = 1048576;
        private const int SendChunkSize = 1024;

        private ClientWebSocket _ws;
        private readonly Uri _uri;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;

        private Action<WebSocketClient> _onConnected;
        private Action<Exception, WebSocketClient> _onConnectionError;
        private Action<byte[], WebSocketClient> _onMessageBinary;
        private Action<string, WebSocketClient> _onMessageString;
        private Action<WebSocketClient> _onDisconnected;

        protected WebSocketClient(string uri)
        {
            _uri = new Uri(uri);
            _cancellationToken = _cancellationTokenSource.Token;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="uri">The URI of the WebSocket server.</param>
        /// <returns>Instance of the created WebSocketWrapper</returns>
        internal static WebSocketClient Create(string uri)
        {
            return new WebSocketClient(uri);
        }

        /// <summary>
        /// Connects to the WebSocket server.
        /// </summary>
        /// <returns>Self</returns>
        internal WebSocketClient Connect()
        {
            if (_ws == null)
            {
                _ws = new ClientWebSocket();
                _ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
            }

            ConnectAsync();
            return this;
        }

        /// <summary>
        /// Disconnects from the WebSocket server.
        /// </summary>
        /// <returns>Self</returns>
        internal WebSocketClient Disconnect()
        {
            DisconnectAsync();
            return this;
        }

        /// <summary>
        /// Get the current state of the WebSocket client.
        /// </summary>
        internal WebSocketState State
        {
            get
            {
                if (_ws == null)
                    return WebSocketState.None;

                return _ws.State;
            }
        }

        /// <summary>
        /// Set the Action to call when the connection has been established.
        /// </summary>
        /// <param name="onConnect">The Action to call</param>
        /// <returns>Self</returns>
        internal WebSocketClient OnConnect(Action<WebSocketClient> onConnect)
        {
            _onConnected = onConnect;
            return this;
        }

        /// <summary>
        /// Set the Action to call when the connection fails.
        /// </summary>
        /// <param name="onConnectionError">The Action to call</param>
        /// <returns>Self</returns>
        internal WebSocketClient OnConnectionError(Action<Exception, WebSocketClient> onConnectionError)
        {
            _onConnectionError = onConnectionError;
            return this;
        }

        /// <summary>
        /// Set the Action to call when the connection has been terminated.
        /// </summary>
        /// <param name="onDisconnect">The Action to call</param>
        /// <returns>Self</returns>
        internal WebSocketClient OnDisconnect(Action<WebSocketClient> onDisconnect)
        {
            _onDisconnected = onDisconnect;
            return this;
        }

        /// <summary>
        /// Set the Action to call when a messages has been received.
        /// </summary>
        /// <param name="onMessage">The Action to call.</param>
        /// <returns>Self</returns>
        internal WebSocketClient OnBinaryMessage(Action<byte[], WebSocketClient> onMessage)
        {
            _onMessageBinary = onMessage;
            return this;
        }

        /// <summary>
        /// Set the Action to call when a messages has been received.
        /// </summary>
        /// <param name="onMessage">The Action to call.</param>
        /// <returns>Self</returns>
        internal WebSocketClient OnMessageReceived(Action<string, WebSocketClient> onMessage)
        {
            _onMessageString = onMessage;
            return this;
        }

        /// <summary>
        /// Send a byte array to the WebSocket server.
        /// </summary>
        /// <param name="data">The data to send</param>
        internal void SendMessage(byte[] data)
        {
            SendMessageAsync(data);
        }

        /// <summary>
        /// Send a UTF8 string to the WebSocket server.
        /// </summary>
        /// <param name="message">The message to send</param>
        internal void SendMessage(string message)
        {
            SendMessage(Encoding.UTF8.GetBytes(message));
        }

        private async void SendMessageAsync(byte[] message)
        {
            if (_ws.State != WebSocketState.Open)
            {
                throw new Exception("Connection is not open.");
            }

            var messagesCount = (int)Math.Ceiling((double)message.Length / SendChunkSize);

            for (var i = 0; i < messagesCount; i++)
            {
                var offset = (SendChunkSize * i);
                var count = SendChunkSize;
                var lastMessage = ((i + 1) == messagesCount);

                if ((count * (i + 1)) > message.Length)
                {
                    count = message.Length - offset;
                }

                await _ws.SendAsync(new ArraySegment<byte>(message, offset, count), WebSocketMessageType.Text, lastMessage, _cancellationToken);
            }
        }

        private async void ConnectAsync()
        {
            try
            {
                await _ws.ConnectAsync(_uri, _cancellationToken);
                CallOnConnected();
                StartListen();
            }
            catch (Exception e)
            {
                _ws.Dispose();
                _ws = null;
                CallOnConnectionError(e);
            }
        }

        private async void DisconnectAsync()
        {
            if (_ws != null)
            {
                if (_ws.State != WebSocketState.Open)
                    await _ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                _ws.Dispose();
                _ws = null;
                CallOnDisconnected();
            }
        }

        private async void StartListen()
        {
            var buffer = new byte[ReceiveChunkSize];

            try
            {
                while (_ws.State == WebSocketState.Open)
                {
                    byte[] byteResult = new byte[0];

                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            Disconnect();
                        }
                        else
                        {
                            byteResult = byteResult.Concat(buffer.Take(result.Count)).ToArray();
                        }

                    } while (!result.EndOfMessage);

                    CallOnMessage(byteResult);
                }
            }
            catch (Exception)
            {
                //CallOnDisconnected();
                Disconnect();
            }
            /*
                        finally
                        {
                            _ws.Dispose();
                        }
            */
        }

        private void CallOnMessage(byte[] result)
        {
            if (_onMessageBinary != null)
                RunInTask(() => _onMessageBinary(result, this));

            if (_onMessageString != null)
                RunInTask(() => _onMessageString(Encoding.UTF8.GetString(result), this));
        }


        private void CallOnDisconnected()
        {
            if (_onDisconnected != null)
                RunInTask(() => _onDisconnected(this));
        }

        private void CallOnConnected()
        {
            if (_onConnected != null)
                RunInTask(() => _onConnected(this));
        }

        private void CallOnConnectionError(Exception e)
        {
            if (_onConnectionError != null)
                RunInTask(() => _onConnectionError(e, this));
            else
                throw e;
        }

        private static void RunInTask(Action action)
        {
            Task.Factory.StartNew(action);
        }

        public void Dispose()
        {
            _ws?.Dispose();
            _cancellationTokenSource?.Dispose();
        }
    }
}

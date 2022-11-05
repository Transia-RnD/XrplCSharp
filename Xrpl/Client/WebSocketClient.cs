using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xrpl.Client
{
    //credit: https://github.com/Badiboy/WebSocketWrapper/blob/master/WebSocketWrapper.cs

    public class WebSocketClient : IDisposable
    {

        //private Action<WebSocketClient> _onConnected;
        //private Action<Exception, WebSocketClient> _onConnectionError;
        //private Action<byte[], WebSocketClient> _onMessageBinary;
        //private Action<string, WebSocketClient> _onMessageString;
        //private Action<WebSocketClient> _onDisconnected;

        /// <summary>
        /// The encapsulated websocket client.
        /// </summary>
        private readonly ClientWebSocket ws;

        /// <summary>
        /// The size of the message send to the server.
        /// </summary>
        private readonly int sendChunkSize;

        /// <summary>
        /// The default size of the accepted messages.
        /// </summary>
        private readonly int receiveChunkSize;

        /// <summary>
        /// The background worker used to manage the reception of messages.
        /// </summary>
        private readonly BackgroundWorker worker;

        private readonly string uri;

        private readonly CancellationToken cancellationToken;


        public WebSocketClient(string uri)
        {
            this.uri = uri;
            this.sendChunkSize = 1024;
            this.receiveChunkSize = 1048576;
            int keepAlive = 5;

            cancellationToken = new CancellationTokenSource().Token;

            this.ws = new ClientWebSocket();
            this.ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(keepAlive);

            this.worker = new BackgroundWorker();
            this.worker.DoWork += (s, e) => this.CatchMessagesAsync().Wait();
        }

        /// <summary>
        /// Raised when the websocket is connected.
        /// </summary>
        public event EventHandler OnConnected;

        /// <summary>
        /// Raised when the websocket is closed.
        /// </summary>
        public event EventHandler OnDisconnect;

        /// <summary>
        /// Raised when an error occurs during the reception of a message.
        /// </summary>
        public event EventHandler<Exception> OnConnectionError;

        /// <summary>
        /// Raised when a new message is received.
        /// </summary>
        public event EventHandler<string> OnMessageReceived;

        /// <summary>
        /// Connects to a WebSocket server as an asynchronous operation.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task ConnectAsync()
        {
            Uri server = new Uri(this.uri);
            await this.ws.ConnectAsync(server, cancellationToken);
            this.worker.RunWorkerAsync();
            this.RaiseConnected();
        }

        /// <summary>
        /// Sends a message back to the websocket server.
        /// </summary>
        /// <param name="message">The message to be send.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task SendMessageAsync(string message)
        {
            //Debug.WriteLine($"WS: SENT: {message}");
            if (this.ws.State != WebSocketState.Open)
            {
                throw new Exception("Connection is not open.");
            }

            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
            int messagesCount = (int)Math.Ceiling((double)messageBuffer.Length / this.sendChunkSize);

            for (int i = 0; i < messagesCount; i++)
            {
                int offset = (this.sendChunkSize * i);
                int count = this.sendChunkSize;
                bool lastMessage = ((i + 1) == messagesCount);

                if ((count * (i + 1)) > messageBuffer.Length)
                {
                    count = messageBuffer.Length - offset;
                }
                //Debug.WriteLine($"CLIENT WS BUFFER: {messageBuffer.Length}");
                await this.ws.SendAsync(new ArraySegment<byte>(messageBuffer, offset, count), WebSocketMessageType.Binary, lastMessage, CancellationToken.None);
            }
        }


        /// <summary>
        /// Catches all the messages send by the server and raises <see cref="MessageReceived"/> events.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private async Task CatchMessagesAsync()
        {
            //Debug.WriteLine("WS: CONNECTED");
            byte[] buffer = new byte[this.receiveChunkSize];

            while (this.ws.State == WebSocketState.Open)
            {
                try
                {
                    //Debug.WriteLine("WS: RECEIVED");
                    var stringResult = new StringBuilder();

                    WebSocketReceiveResult result;
                    do
                    {
                        result = await this.ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        //Debug.WriteLine("WS: RESULT");
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            this.RaiseClosed();
                            return;
                        }
                        else
                        {
                            string str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            stringResult.Append(str);
                        }
                    }
                    while (!result.EndOfMessage);
                    {
                        //Debug.WriteLine("WS: EndOfMessage");
                        this.RaiseMessageReceived(stringResult.ToString());
                    }
                }
                catch (Exception exception)
                {
                    //Debug.WriteLine($"WS: EXCEPTION: {exception.Message}");
                    this.RaiseError(exception);
                }
            }

            //Debug.WriteLine("WS: CLOSING");
            this.RaiseClosed();
        }

        /// <summary>
        /// Raises the <see cref="Connected"/> event.
        /// </summary>
        private void RaiseConnected()
        {
            this.OnConnected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="Closed"/> event.
        /// </summary>
        private void RaiseClosed()
        {
            this.OnDisconnect?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="Error"/> event.
        /// </summary>
        /// <param name="exception">The catched exception.</param>
        private void RaiseError(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            this.OnConnectionError?.Invoke(this, exception);
        }

        /// <summary>
        /// Raises the <see cref="MessageReceived"/> event.
        /// </summary>
        /// <param name="message">The message received.</param>
        private void RaiseMessageReceived(string message)
        {
            Debug.WriteLine($"WS RECEIVED: {DateTime.Now}");
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.OnMessageReceived?.Invoke(this, message);
        }

        public void Close(WebSocketCloseStatus code, string? message = null)
        {
            //Debug.WriteLine("WS: CLOSE");
            this.ws.CloseAsync(code, message, cancellationToken);
            this.RaiseClosed();
        }

        /// <summary>
        /// Disposes the encapsulated websocket client.
        /// </summary>
        public void Dispose()
        {
            this.ws.Dispose();
        }
    }
}

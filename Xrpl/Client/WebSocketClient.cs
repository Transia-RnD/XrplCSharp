using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xrpl.Client
{

    public class WebSocketClient : IDisposable
    {

        private ClientWebSocket _client;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private Uri _uri;
        private string _name;
        public bool _isConnected;
        private bool _isDisposed;

        public event Action<CancellationTokenSource> OnConnected;
        public event Action<string> OnMessageReceived;
        public event Action<Exception> OnError;
        public event Action<Exception> OnConnectionException;
        public event Action<int> OnDisconnect;

        public WebSocketClient(string uri)
        {
            _uri = new Uri(uri);
            _name = "xrpl";
            _client = new ClientWebSocket();
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }

        public async Task ConnectAsync()
        {
            try
            {
                await _client.ConnectAsync(_uri, _cancellationToken);
                _isConnected = true;
                OnConnected?.Invoke(_cancellationTokenSource);
                await ReceiveAsync();
            }
            catch (Exception ex)
            {
                OnConnectionException?.Invoke(ex);
            }
        }

        public async Task Send(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            if (_isConnected)
            {
                try
                {
                    await _client.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, _cancellationToken);
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(ex);
                }
            }
        }

        public async Task ReceiveAsync()
        {
            if (_isConnected)
            {
                try
                {
                    var buffer = new byte[1024 * 4];
                    var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken);
                    // Debug.WriteLine("WS: RESULT");
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, _cancellationToken);
                        OnDisconnect?.Invoke(((int)WebSocketCloseStatus.NormalClosure));
                    }
                    else
                    {
                        // Debug.WriteLine("WS: OnMessageReceived");
                        OnMessageReceived?.Invoke(Encoding.UTF8.GetString(buffer));
                        await ReceiveAsync();
                    }
                }
                catch (Exception ex)
                {
                    if (_isConnected)
                        OnError?.Invoke(ex);
                }
            }
        }

        public void Close(int? code = (int)WebSocketCloseStatus.NormalClosure)
        {
            if (_isConnected)
            {
                _cancellationTokenSource.Cancel();
                _client.Abort();
                _client.Dispose();
                _isConnected = false;
                OnDisconnect?.Invoke((int)code);
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Close();
                _cancellationTokenSource.Dispose();
                _isDisposed = true;
            }
        }
    }
}
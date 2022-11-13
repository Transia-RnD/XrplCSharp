using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xrpl.Client.Exceptions;
using System.Timers;
using static Xrpl.Client.RequestManager;
using Xrpl.AddressCodec;
using Xrpl.Models.Subscriptions;
using Xrpl.Models.Methods;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/client/connection.ts

namespace Xrpl.Client
{
    public class Connection
    {

        public event OnError OnError;
        public event OnConnected OnConnected;
        public event OnDisconnect OnDisconnect;
        public event OnLedgerClosed OnLedgerClosed;
        public event OnTransaction OnTransaction;
        public event OnManifestReceived OnManifestReceived;
        public event OnPeerStatusChange OnPeerStatusChange;
        public event OnConsensusPhase OnConsensusPhase;
        public event OnPathFind OnPathFind;

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public class Trace
        {
            public string id { get; set; }
            public string message { get; set; }
        }

        public class ConnectionOptions
        {
            public Trace trace { get; set; }
            public string proxy { get; set; }
            public string proxyAuthorization { get; set; }
            public string authorization { get; set; }
            public string trustedCertificates { get; set; }
            public string key { get; set; }
            public string passphrase { get; set; }
            public string certificate { get; set; }
            public int timeout { get; set; }
            public int connectionTimeout { get; set; }
            public Dictionary<string, dynamic> headers { get; set; }
        }

        internal WebSocketClient CreateWebSocket(string url, ConnectionOptions config)
        {
            // Client or Creation...
            //ClientWebSocketOptions options = new ClientWebSocketOptions()
            //{
            //    Proxy = config.proxy,
            //    Credentials = config.authorization,
            //    ClientCertificates = config.trustedCertificates
            //};
            //options.agent = getAgent(url, config)
            //WebSocketCreationOptions create = new WebSocketCreationOptions()
            //{

            //};
            //  if (config.authorization != null)
            //  {
            //      string base64 = Base64Encode(config.authorization);
            //      options.headers = {
            //          ...options.headers,
            //          Authorization: $"Basic {base64}",
            //      }
            //      const optionsOverrides = _.omitBy(
            //      {
            //          ca: config.trustedCertificates,
            //          key: config.key,
            //          passphrase: config.passphrase,
            //          cert: config.certificate,
            //  },
            //  (value) => value == null,
            //)
            //const websocketOptions = { ...options, ...optionsOverrides };
            return new WebSocketClient(url); // todo add options
        }

        int TIMEOUT = 20;
        int CONNECTION_TIMEOUT = 5;
        int INTENTIONAL_DISCONNECT_CODE = 4000;

        public readonly string url;
        public WebSocketClient ws;

        private int? reconnectTimeoutID = null;
        private int? heartbeatIntervalID = null;

        public readonly ConnectionOptions config;
        public RequestManager requestManager = new RequestManager();
        public ConnectionManager connectionManager = new ConnectionManager();

        public Connection(string server, ConnectionOptions? options = null)
        {
            url = server;
            config = options ?? new ConnectionOptions();
            config.timeout = TIMEOUT * 1000;
            config.connectionTimeout = CONNECTION_TIMEOUT * 1000;

        }

        public bool IsConnected()
        {
            return this.State() == WebSocketState.Open;
        }

        public Timer timer;

        public Task Connect()
        {
            if (this.IsConnected())
            {
                var p1 = new TaskCompletionSource();
                p1.TrySetResult();
                return p1.Task;
            }
            if (this.State() == WebSocketState.Connecting)
            {
                this.connectionManager.AwaitConnection();
            }
            if (this.url == null)
            {
                throw new ConnectionError("Cannot connect because no server was specified");
            }
            if (this.ws != null)
            {
                throw new XrplError("Websocket connection never cleaned up.");
            }
            //Create the connection timeout, in case the connection hangs longer than expected.

            timer = new Timer(this.config.connectionTimeout);
            timer.Elapsed += (sender, e) => OnConnectionFailed(new ConnectionError($"Error: connect() timed out after {this.config.connectionTimeout} ms.If your internet connection is working, the rippled server may be blocked or inaccessible.You can also try setting the 'connectionTimeout' option in the Client constructor."), null);
            timer.Start();

            //// Connection listeners: these stay attached only until a connection is done/open.
            this.ws = CreateWebSocket(this.url, this.config);
            if (this.ws == null)
            {
                throw new XrplError("Connect: created null websocket");
            }
            int connectionTimeoutID = 1;
            //this.ws.on('error', (error) => this.onConnectionFailed(error))
            //this.ws.on('error', () => clearTimeout(connectionTimeoutID))
            //this.ws.on('close', (reason) => this.onConnectionFailed(reason))
            //this.ws.on('close', () => clearTimeout(connectionTimeoutID))
            //this.ws.once('open', () => {
            //    void this.onceOpen(connectionTimeoutID)
            //})

            this.ws.OnConnected += (c, e) => OnceOpen((WebSocketClient)c);
            this.ws.OnMessageReceived += (c, m) => OnMessage(m, (WebSocketClient)c);
            this.ws.OnConnectionError += (c, e) => OnConnectionFailed(e, (WebSocketClient)c);
            this.ws.OnConnectionError += (c, e) => timer.Stop();
            this.ws.OnDisconnect += (c, e) => OnceClose((WebSocketClient)c, e);
            this.ws.OnDisconnect += (c, e) => timer.Stop();
            _ = this.ws.ConnectAsync();
            return this.connectionManager.AwaitConnection();
        }

        public Task<int> Disconnect()
        {
            //Debug.WriteLine("DISCONNECTING...");
            ////this.ClearHeartbeatInterval();
            //if (this.reconnectTimeoutID != null)
            //{
            //    //clearTimeout(this.reconnectTimeoutID);
            //    this.reconnectTimeoutID = null;
            //}
            if (this.State() == WebSocketState.Closed)
            {
                //Debug.WriteLine("WS CLOSED");
                var p1 = new TaskCompletionSource<int>();
                p1.TrySetResult(0);
                return p1.Task;
            }

            if (this.ws == null)
            {
                //Debug.WriteLine("WS NULL");
                var p1 = new TaskCompletionSource<int>();
                p1.TrySetResult(0);
                return p1.Task;
            }

            var promise = new TaskCompletionSource<int>();

            if (this.ws != null)
            {
                //Debug.WriteLine("WS NO NULL");
                this.ws.OnDisconnect += (c, e) =>
                {
                    //Debug.WriteLine("INSIDE DISCONNECT");
                    promise.TrySetResult(((int)WebSocketCloseStatus.NormalClosure));
                };
            }

            /// <summary>
            /// Connection already has a disconnect handler for the disconnect logic.
            /// Just close the websocket manually (with our "intentional" code) to
            /// trigger that.
            /// </summary>
            if (this.ws != null && this.State() != WebSocketState.CloseReceived)
            {
                //Debug.WriteLine("CLOSING...");
                this.ws.Close(WebSocketCloseStatus.NormalClosure);
            }
            return promise.Task;
        }

        private void OnConnectionFailed(Exception error, WebSocketClient client)
        {
            Debug.WriteLine($"OnConnectionFailed: {error.Message}");
            if (this.ws != null)
            {
                //this.ws.RemoveAllListeners();
                //this.ws.on('error', () => {
                /*
                * Correctly listen for -- but ignore -- any future errors: If you
                * don't have a listener on "error" node would log a warning on error.
                */
                //});
                this.ws.Close(WebSocketCloseStatus.ProtocolError);
                this.ws = null;
            }
            this.connectionManager.RejectAllAwaiting(new NotConnectedError(error.Message));
        }

        private void OnConnectionFailed(WebSocketClient client)
        {
            //Debug.WriteLine($"OnConnectionFailed: NO error.Message");
            //clearTimeout(connectionTimeoutID))
            timer.Stop();
            this.connectionManager.RejectAllAwaiting(new NotConnectedError());
        }

        public Task WebsocketSendAsync(string message)
        {
            //Debug.WriteLine($"CLIENT: SEND: {message}");
            return this.ws.SendMessageAsync(message);
        }

        public async Task<Dictionary<string, dynamic>> Request(Dictionary<string, dynamic> request, int? timeout = null)
        {
            if (!this.ShouldBeConnected() || this.ws == null)
            {
                throw new NotConnectedError();
            }
            XrplRequest _request = this.requestManager.CreateRequest(request, timeout ?? this.config.timeout);
            //Debug.WriteLine(_request.Message);
            try
            {
                // Debug.WriteLine($"CLIENT: SEND: {_request.Id}");
                await this.WebsocketSendAsync(_request.Message);
            }
            catch (EncodingFormatException error)
            {
                this.requestManager.Reject(_request.Id, error);
            }
            return await _request.Promise;
        }

        public async Task<dynamic> GRequest<T, R>(R request, int? timeout = null)
        {
            if (!this.ShouldBeConnected() || this.ws == null)
            {
                throw new NotConnectedError();
            }
            XrplGRequest _request = this.requestManager.CreateGRequest<T, R>(request, timeout ?? this.config.timeout);
            //Debug.WriteLine(_request.Message);
            try
            {
                // Debug.WriteLine($"CLIENT: SEND: {_request.Id}");
                await this.WebsocketSendAsync(_request.Message);
            }
            catch (EncodingFormatException error)
            {
                this.requestManager.Reject(_request.Id, error);
            }
            return await _request.Promise;
        }

        public string GetUrl()
        {
            return this.url;
        }

        public WebSocketState State()
        {
            return this.ws != null ? WebSocketState.Open : WebSocketState.Closed;
        }

        private bool ShouldBeConnected()
        {
            return this.ws != null;
        }

        //private void OnceOpen(int connectionTimeoutID)
        private async void OnceOpen(WebSocketClient client)
        {
            //Debug.WriteLine("ONCE OPEN");
            if (this.ws == null)
            {
                throw new XrplError("onceOpen: ws is null");
            }

            //this.ws.RemoveAllListeners()
            //clearTimeout(connectionTimeoutID)
            timer.Stop();
            // Finalize the connection and resolve all awaiting connect() requests
            try
            {
                //this.retryConnectionBackoff.reset();
                //this.startHeartbeatInterval();
                this.connectionManager.ResolveAllAwaiting();
                //this.OnConnected();
            }
            catch (Exception error)
            {
                Debug.WriteLine($"Once Open Error: {error.Message}");
                this.connectionManager.RejectAllAwaiting(error);
                // Ignore this error, propagate the root cause.
                await this.Disconnect();
            }
        }

        //private void OnceClose(int? code = null, string? reason = null)
        private void OnceClose(WebSocketClient client, EventArgs error)
        {
            //Debug.WriteLine("ONCE CLOSE");
            if (this.ws == null)
            {
                throw new XrplError("OnceClose: ws is null");
            }
            //this.clearHeartbeatInterval();
            this.requestManager.RejectAll(new DisconnectedError($"websocket was closed, {"SOME"}"));
            //this.ws.removeAllListeners();
            this.ws = null;
            int? code = null;
            string reason = null;
            if (code == null)
            {
                //Debug.WriteLine("CODE == NULL");
                //string reasonText = reason ? reason.ToString() : null;
                string reasonText = reason;
                // eslint-disable-next-line no-Debug -- The error is helpful for debugging.
                //Debug.error(
                //  `Disconnected but the disconnect code was undefined(The given reason was ${ reasonText}).` +
                //    `This could be caused by an exception being thrown during a 'connect' callback. ` +
                //    `Disconnecting with code 1011 to indicate an internal error has occurred.`,
                //)

                /*
                 * Error code 1011 represents an Internal Error according to
                 * https://developer.mozilla.org/en-US/docs/Web/API/CloseEvent/code
                 */
                //Debug.WriteLine("DISCONNECT1");
                this.OnDisconnect(1011);
            }
            else
            {
                //Debug.WriteLine("DISCONNECT2");
                this.OnDisconnect(code);
            }

            /// <summary>
            /// If this wasn't a manual disconnect, then lets reconnect ASAP.
            /// Code can be undefined if there's an exception while connecting.
            /// </summary>
            if (code != INTENTIONAL_DISCONNECT_CODE && code != null)
            {
                //this.intentionalDisconnect();
            }
        }

        private void OnMessage(string message, WebSocketClient client)
        {
            BaseResponse data;
            try
            {
                data = JsonConvert.DeserializeObject<BaseResponse>(message);
            }
            catch (Exception error)
            {
                this.OnError("error", "badMessage", error.Message, message);
                return;
            }
            if (data.Type == null && data.Error != null)
            {
                // e.g. slowDown
                this.OnError("error", data.Error, "data.ErrorMessage", data);
                return;
            }
            if (data.Type != null)
            {
                Enum.TryParse(data.Type.ToString(), out ResponseStreamType type);
                switch (type)
                {
                    case ResponseStreamType.ledgerClosed:
                        {
                            object response = JsonConvert.DeserializeObject<object>(message.ToString());
                            OnLedgerClosed?.Invoke(response);
                            break;
                        }
                    case ResponseStreamType.validationReceived:
                        {
                            var response = JsonConvert.DeserializeObject<ValidationStream>(message);
                            OnManifestReceived?.Invoke(response);
                            break;
                        }
                    case ResponseStreamType.transaction:
                        {
                            var response = JsonConvert.DeserializeObject<TransactionStream>(message);
                            OnTransaction?.Invoke(response);
                            break;
                        }
                    case ResponseStreamType.peerStatusChange:
                        {
                            var response = JsonConvert.DeserializeObject<PeerStatusStream>(message);
                            OnPeerStatusChange?.Invoke(response);
                            break;
                        }
                    case ResponseStreamType.consensusPhase:
                        {
                            var response = JsonConvert.DeserializeObject<ConsensusStream>(message);
                            OnConsensusPhase?.Invoke(response);
                            break;
                        }
                    case ResponseStreamType.path_find:
                        {
                            var response = JsonConvert.DeserializeObject<PathFindStream>(message);
                            OnPathFind?.Invoke(response);
                            break;
                        }
                    default:
                        break;
                }
            }
            if (data.Type == "response")
            {
                try
                {
                    this.requestManager.HandleResponse(data);
                }
                catch (XrplError error)
                {
                    this.OnError("error", "badMessage", error.Message, error);
                }
                catch (Exception error)
                {
                    this.OnError("error", "badMessage", error.Message, error);
                }
            }
        }

        public void OnMessage(string message)
        {
            OnMessage(message, ws);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using Xrpl.AddressCodec;
using Xrpl.Client.Exceptions;
using Xrpl.Utils.Hashes.ShaMap;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static Xrpl.Client.RequestManager;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/client/connection.ts

namespace Xrpl.Client
{
    public class Connection
    {

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

        /**
         * Create a new websocket given your URL and optional proxy/certificate
         * configuration.
         *
         * @param url - The URL to connect to.
         * @param config - THe configuration options for the WebSocket.
         * @returns A Websocket that fits the given configuration parameters.
         */
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
            return WebSocketClient.Create(url); // todo add options
        }


        int TIMEOUT = 20;
        int CONNECTION_TIMEOUT = 5;
        int INTENTIONAL_DISCONNECT_CODE = 4000;

        public readonly string url;
        internal WebSocketClient ws;

        private int? reconnectTimeoutID = null;
        private int? heartbeatIntervalID = null;

        public readonly ConnectionOptions config;
        public RequestManager requestManager = new RequestManager();
        public ConnectionManager connectionManager = new ConnectionManager();

        public Connection(string server, ConnectionOptions? options = null)
        {
            Debug.WriteLine(server);
            url = server;
            config = options ?? new ConnectionOptions();
            //config.timeout = TIMEOUT * 1000;
            //config.connectionTimeout = CONNECTION_TIMEOUT * 1000;
            config.timeout = 5;
            config.connectionTimeout = 5;
        }

        public bool IsConnected()
        {
            return this.ws.State != WebSocketState.Closed;
        }

        public Task Connect()
        {
            //if (this.IsConnected())
            //{
            //    return Task;
            //}
            //if (this.ws.State == WebSocketState.Connecting)
            //{
            //    this.connectionManager.AwaitConnection();
            //}
            //if (!this.url)
            //{
            //    throw new ConnectionError("Cannot connect because no server was specified");
            //}
            //if (this.ws != null)
            //{
            //    throw new XrplError("Websocket connection never cleaned up.");
            //}
            // Create the connection timeout, in case the connection hangs longer than expected.

            var task = Task.Run(() =>
            {
                //// Connection listeners: these stay attached only until a connection is done/open.
                this.ws = CreateWebSocket(this.url, this.config);
                this.ws.Connect();

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
                this.ws.OnConnectionError(OnConnectionFailed);
                this.ws.OnDisconnect(OnConnectionFailed);
                this.ws.OnConnect(OnceOpen);
                //OnceOpen(connectionTimeoutID);
                this.connectionManager.AwaitConnection();
                //OnceOpen(connectionTimeoutID);
            });
            if (task.Wait(TimeSpan.FromSeconds(this.config.connectionTimeout)))
            {
                return task;
            }
            else
            {
                OnConnectionFailed(new ConnectionError($"Error: connect() timed out after {this.config.connectionTimeout} ms.If your internet connection is working, the rippled server may be blocked or inaccessible.You can also try setting the 'connectionTimeout' option in the Client constructor."), null);
                return task;
            }
        }

        public Task<int> Disconnect()
        {
            throw new XrplError("Disconnect");
            //this.ClearHeartbeatInterval();
            if (this.reconnectTimeoutID != null)
            {
                //clearTimeout(this.reconnectTimeoutID);
                this.reconnectTimeoutID = null;
            }
            if (this.State() == WebSocketState.Closed)
            {
                var p1 = new TaskCompletionSource<int>();
                p1.SetResult(0);
                return p1.Task;
            }

            if (this.ws == null)
            {
                var p1 = new TaskCompletionSource<int>();
                p1.SetResult(0);
                return p1.Task;
            }

            var promise = new TaskCompletionSource<int>();

            if (this.ws == null)
            {
                promise.SetResult(0);
            }
            if (this.ws != null)
            {
                //this.ws.once('close', (code) => {
                //    resolve(code));
                //}
                int code = 4000;
                promise.SetResult(code);
            }
            /*
            * Connection already has a disconnect handler for the disconnect logic.
            * Just close the websocket manually (with our "intentional" code) to
            * trigger that.
            */
            if (this.ws != null && this.State() != WebSocketState.CloseReceived)
            {
                this.ws.Disconnect(); // should use INTENTIONAL_DISCONNECT_CODE
            }
            return promise.Task;
        }

        private void OnConnectionFailed(Exception error, WebSocketClient client)
        {
            throw new XrplError($"OnConnectionFailed: {error}");
            if (this.ws != null)
            {
                //this.ws.RemoveAllListeners();
                //this.ws.on('error', () => {
                /*
                * Correctly listen for -- but ignore -- any future errors: If you
                * don't have a listener on "error" node would log a warning on error.
                */
                //});
                //throw new NotConnectedError(error.Message);
                this.ws.Dispose();
                this.ws = null;
            }
            this.connectionManager.RejectAllAwaiting(new NotConnectedError(error.Message));
        }

        private void OnConnectionFailed(WebSocketClient client)
        {
            throw new XrplError("OnConnectionFailed");
            //clearTimeout(connectionTimeoutID))
        }

        private void WebsocketSendAsync(WebSocketClient ws, string message)
        {
            Debug.WriteLine(ws);
            Debug.WriteLine(ws.State.ToString());
            ws.SendMessage(message);
            //return new Promise<void>((resolve, reject) => {
            //    ws.SendMessage(message, (error) => {
            //        if (error)
            //        {
            //            reject(new DisconnectedError(error.message, error))
            //        }
            //        else
            //        {
            //            resolve()
            //        }
            //    })
            //})
        }

        public Task<dynamic> Request(Dictionary<string, dynamic> request, int? timeout = null)
        {
            Debug.WriteLine(this.ws.State.ToString());
            if (!this.ShouldBeConnected() || this.ws == null)
            {
                throw new NotConnectedError();
            }
            XrplRequest _request = this.requestManager.CreateRequest(request, timeout ?? this.config.timeout);
            //this.trace('send', message)
            try
            {
                this.WebsocketSendAsync(this.ws, _request.Message);
            }
            catch (EncodingFormatException error)
            {
                this.requestManager.Reject(_request.Id, error);
            }
            return _request.Promise.TaskCompletionResult;
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
        private void OnceOpen(WebSocketClient client)
        {
            if (this.ws == null)
            {
                throw new XrplError("onceOpen: ws is null");
            }

            //this.ws.RemoveAllListeners()
            //clearTimeout(connectionTimeoutID)
            //this.ws.on('message', (message: string) => this.onMessage(message));
            //this.ws.on('error', (error) =>
            //  this.emit('error', 'websocket', error.message, error),
            //)
            //this.ws.once('close', (code ?: number, reason ?: Buffer) => { });
            // Finalize the connection and resolve all awaiting connect() requests
            //throw new XrplError("ONCE OPEN");
            try
            {
                //this.retryConnectionBackoff.reset();
                //this.startHeartbeatInterval();
                this.connectionManager.ResolveAllAwaiting();
                //this.emit('connected');
            }
            catch (Exception error)
            {
                throw error;
                this.connectionManager.RejectAllAwaiting(error);
                // Ignore this error, propagate the root cause.
                //await this.disconnect().catch (() => { });
            }
        }

        private void HandleClose(int? code = null, string? reason = null)
        {
            if (this.ws == null)
            {
                throw new XrplError("onceClose: ws is null");
            }
            //this.clearHeartbeatInterval();
            //this.requestManager.rejectAll(new DisconnectedError("websocket was closed, ${ new TextDecoder('utf-8').decode(reason) }"));
            //this.ws.removeAllListeners();
            this.ws = null;
            if (code == null)
            {
                //string reasonText = reason ? reason.ToString() : null;
                string reasonText = reason;
                // eslint-disable-next-line no-console -- The error is helpful for debugging.
                //console.error(
                //  `Disconnected but the disconnect code was undefined(The given reason was ${ reasonText}).` +
                //    `This could be caused by an exception being thrown during a 'connect' callback. ` +
                //    `Disconnecting with code 1011 to indicate an internal error has occurred.`,
                //)

                /*
                 * Error code 1011 represents an Internal Error according to
                 * https://developer.mozilla.org/en-US/docs/Web/API/CloseEvent/code
                 */
                int internalErrorCode = 1011;
                //this.emit('disconnected', internalErrorCode);
            }
            else
            {
                //this.emit('disconnected', code);
            }

            /*
             * If this wasn't a manual disconnect, then lets reconnect ASAP.
             * Code can be undefined if there's an exception while connecting.
             */
            if (code != INTENTIONAL_DISCONNECT_CODE && code != null)
            {
                //this.intentionalDisconnect();
            }
        }
    }
}
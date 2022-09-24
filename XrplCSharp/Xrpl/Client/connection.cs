using System;
using System.Collections.Generic;
using System.Net.WebSockets;

// https://github.com/XRPLF/xrpl.js/blob/76b73e16a97e1a371261b462ee1a24f1c01dbb0c/packages/xrpl/src/client/connection.ts#L388

namespace Xrpl.ClientLib
{
    public class connection
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
        public void CreateWebSocket(string url, ConnectionOptions config)
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

        }

        //public void CreateWebSocket(string url, ConnectionOptions config)
        //{

        //}
    }
}
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xrpl.Client;
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/setup.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    public class SetupIntegration
    {
        public XrplWallet wallet;
        public XrplClient client;

        public async Task<SetupIntegration> SetupClient(string serverUrl)
        {
            wallet = XrplWallet.Generate();
            var promise = new TaskCompletionSource();
            client = new XrplClient(serverUrl);
            client.connection.OnConnected += () =>
            {
                Console.WriteLine($"SetupIntegration CONNECTED");
                return Task.CompletedTask;
            };
            client.connection.OnDisconnect += (code) =>
            {
                Console.WriteLine($"SetupIntegration DISCONNECTED: {code}");
                return Task.CompletedTask;
            };
            client.connection.OnError += (error, errorMessage, message, data) =>
            {
                Console.WriteLine($"SetupIntegration ERROR: {message}");
                return Task.CompletedTask;
            };
            await client.Connect();
            await Utils.FundAccount(client, wallet);
            return this;
        }
    }
}
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
            client = new XrplClient(serverUrl);
            client.OnConnected += () =>
            {
                Console.WriteLine($"SetupIntegration CONNECTED");
                return Task.CompletedTask;
            };
            client.OnDisconnect += (code) =>
            {
                Console.WriteLine($"SetupIntegration DISCONNECTED: {code}");
                return Task.CompletedTask;
            };
            client.OnError += (error, errorMessage, message, data) =>
            {
                Console.WriteLine($"SetupIntegration ERROR: {message}");
                return Task.CompletedTask;
            };
            await client.Connect();
            await Utils.FundNative(client, wallet);
            return this;
        }
    }
}
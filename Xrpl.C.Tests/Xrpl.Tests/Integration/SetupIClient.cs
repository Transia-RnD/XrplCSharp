using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client;
using Xrpl.Client.Models.Ledger;
using Xrpl.XrplWallet;
using Xrpl.Tests.Client.Tests.Integration;
using System.Diagnostics;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/setup.ts

namespace Xrpl.Tests.Client.Tests.Integration
{
    public class SetupIntegration
    {
        public Wallet wallet;
        public RippleClient client;

        public async Task<SetupIntegration> SetupClient(string serverUrl)
        {
            Debug.WriteLine(serverUrl);
            wallet = Wallet.Generate();
            var promise = new TaskCompletionSource();
            client = new RippleClient(serverUrl);
            client.Connect();
            await Utils.FundAccount(client, wallet);
            return this;
        }
    }
}
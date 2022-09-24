using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.ClientLib;
using Xrpl.Models.Ledger;
using Xrpl.WalletLib;
using static Xrpl.WalletLib.Wallet;

using XrplTests.Xrpl.ClientLib.Integration;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/setup.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    public class SetupIntegration
    {
        public Wallet wallet;
        public Client client;

        public async Task<SetupIntegration> SetupClient(string serverUrl)
        {
            wallet = Wallet.Generate();
            var promise = new TaskCompletionSource();
            client = new Client(serverUrl);
            client.Connect();
            await Utils.FundAccount(client, wallet);
            return this;
        }
    }
}
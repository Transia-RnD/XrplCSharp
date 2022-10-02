using System.Threading.Tasks;
using Xrpl.ClientLib;
using Xrpl.WalletLib;

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
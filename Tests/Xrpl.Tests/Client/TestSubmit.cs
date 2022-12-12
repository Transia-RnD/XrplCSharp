using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Client;
using Xrpl.Models.Transactions;
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/client/submit.ts

namespace XrplTests.Xrpl.ClientLib
{
    [TestClass]
    public class TestUSubmit
    {

        public static SetupUnitClient runner;

        [TestInitialize]
        public async Task MyTestInitializeAsync()
        {
            runner = await new SetupUnitClient().SetupClient();
        }

        [TestCleanup]
        public async Task MyTestCleanupAsync()
        {
            await runner.client.Disconnect();
        }

        static string publicKey = "030E58CDD076E798C84755590AAF6237CA8FAE821070A59F648B517A30DC6F589D";
        static string privateKey = "00141BA006D3363D2FB2785E8DF4E44D3A49908780CB4FB51F6D217C08C021429F";
        static string address = "rhvh5SrgBL5V8oeV9EpDuVszeJSSCEkbPc";
        static Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
        {
            { "TransactionType", "Payment" },
            { "Account", address },
            { "Destination", "rQ3PTWGLCbPz8ZCicV5tCX3xuymojTng5r" },
            { "Amount", "20000000" },
            { "Sequence", 1 },
            { "Fee", "12" },
            { "LastLedgerSequence", 12312 },
        };

        [TestMethod]
        public async Task TestSubmitUnsigned()
        {
            string accountInfoString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"account_data\":{\"Account\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"Balance\":\"922913243\",\"Domain\":\"6578616D706C652E636F6D\",\"EmailHash\":\"23463B99B62A72F26ED677CC556C44E8\",\"Flags\":655360,\"LedgerEntryType\":\"AccountRoot\",\"OwnerCount\":1,\"PreviousTxnID\":\"19899273706A9E040FDB5885EE991A1DC2BAD878A0D6E7DBCFB714E63BF737F7\",\"PreviousTxnLgrSeq\":6614625,\"Sequence\":23,\"TransferRate\":1002000000,\"TickSize\":5,\"WalletLocator\":\"00000000000000000000000000000000000000000000000000000000DEADBEEF\",\"index\":\"396400950EA27EB5710C0D5BE1D2B4689139F168AC5D07C13B8140EC3F82AE71\",\"urlgravatar\":\"http://www.gravatar.com/avatar/23463b99b62a72f26ed677cc556c44e8\",\"signer_lists\":[{\"Flags\":0,\"LedgerEntryType\":\"SignerList\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"D2707DE50E1244B2C2AAEBC78C82A19ABAE0599D29362C16F1B8458EB65CCFE4\",\"PreviousTxnLgrSeq\":3131157,\"SignerEntries\":[{\"SignerEntry\":{\"Account\":\"rpHit3GvUR1VSGh2PXcaaZKEEUnCVxWU2i\",\"SignerWeight\":1}},{\"SignerEntry\":{\"Account\":\"rN4oCm1c6BQz6nru83H52FBSpNbC9VQcRc\",\"SignerWeight\":1}},{\"SignerEntry\":{\"Account\":\"rJ8KhCi67VgbapiKCQN3r1ZA6BMUxUvvnD\",\"SignerWeight\":1}}],\"SignerListID\":0,\"SignerQuorum\":3,\"index\":\"5A9373E02D1DEF7EC9204DEB4819BA42D6AA6BCD878DC8C853062E9DD9708D11\"}]},\"ledger_index\":9592219}}";
            Dictionary<string, dynamic> accountInfoData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(accountInfoString);

            string ledgerString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"ledger\":{\"account_hash\":\"EC028EC32896D537ECCA18D18BEBE6AE99709FEFF9EF72DBD3A7819E918D8B96\",\"close_time\":464908910,\"parent_close_time\":464908900,\"close_time_human\":\"2014-Sep-2421:21:50\",\"close_time_resolution\":10,\"closed\":true,\"close_flags\":0,\"ledger_hash\":\"0F7ED9F40742D8A513AE86029462B7A6768325583DF8EE21B7EC663019DD6A0F\",\"ledger_index\":\"9038214\",\"parent_hash\":\"4BB9CBE44C39DC67A1BE849C7467FE1A6D1F73949EA163C38A0121A15E04FFDE\",\"total_coins\":\"99999973964317514\",\"transaction_hash\":\"ECB730839EB55B1B114D5D1AD2CD9A932C35BA9AB6D3A8C2F08935EAC2BAC239\",\"transactions\":[\"1FC4D12C30CE206A6E23F46FAC62BD393BE9A79A1C452C6F3A04A13BC7A5E5A3\",\"E25C38FDB8DD4A2429649588638EE05D055EE6D839CABAF8ABFB4BD17CFE1F3E\"]},\"ledger_hash\":\"1723099E269C77C4BDE86C83FA6415D71CF20AA5CB4A94E5C388ED97123FB55B\",\"ledger_index\":9038214,\"validated\":true}}";
            Dictionary<string, dynamic> ledgerData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ledgerString);

            string serverInfoString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"info\":{\"build_version\":\"0.24.0-rc1\",\"complete_ledgers\":\"32570-6595042\",\"hostid\":\"ARTS\",\"io_latency_ms\":1,\"last_close\":{\"converge_time_s\":2.007,\"proposers\":4},\"load_factor\":1,\"peers\":53,\"pubkey_node\":\"n94wWvFUmaKGYrKUGgpv1DyYgDeXRGdACkNQaSe7zJiy5Znio7UC\",\"server_state\":\"full\",\"validated_ledger\":{\"age\":5,\"base_fee_xrp\":0.00001,\"hash\":\"4482DEE5362332F54A4036ED57EE1767C9F33CF7CE5A6670355C16CECE381D46\",\"reserve_base_xrp\":20,\"reserve_inc_xrp\":5,\"seq\":6595042},\"validation_quorum\":3}}}";
            Dictionary<string, dynamic> serverInfoData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(serverInfoString);

            string submitString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"success\":true,\"engine_result\":\"tesSUCCESS\",\"engine_result_code\":0,\"engine_result_message\":\"Thetransactionwasapplied.Onlyfinalinavalidatedledger.\",\"tx_blob\":\"1200002280000000240000016861D4838D7EA4C6800000000000000000000000000055534400000000004B4E9C06F24296074F7BC48F92A97916C6DC5EA9684000000000002710732103AB40A0490F9B7ED8DF29D246BF2D6269820A0EE7742ACDD457BEA7C7D0931EDB7446304402200E5C2DD81FDF0BE9AB2A8D797885ED49E804DBF28E806604D878756410CA98B102203349581946B0DDA06B36B35DBC20EDA27552C1F167BCF5C6ECFF49C6A46F858081144B4E9C06F24296074F7BC48F92A97916C6DC5EA983143E9D4A2B8AA0780F682D136F7A56D6724EF53754\",\"tx_json\":{\"Account\":\"rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn\",\"Amount\":{\"currency\":\"USD\",\"issuer\":\"rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn\",\"value\":\"1\"},\"Destination\":\"ra5nK24KXen9AHvsdFTKHSANinZseWnPcX\",\"Fee\":\"10000\",\"Flags\":2147483648,\"Sequence\":360,\"SigningPubKey\":\"03AB40A0490F9B7ED8DF29D246BF2D6269820A0EE7742ACDD457BEA7C7D0931EDB\",\"TransactionType\":\"Payment\",\"TxnSignature\":\"304402200E5C2DD81FDF0BE9AB2A8D797885ED49E804DBF28E806604D878756410CA98B102203349581946B0DDA06B36B35DBC20EDA27552C1F167BCF5C6ECFF49C6A46F8580\",\"hash\":\"4D5D90890F8D49519E4151938601EF3D0B30B16CD6A519D9C99102C9FA77F7E0\"}}}";
            Dictionary<string, dynamic> submitData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(submitString);

            XrplWallet wallet = new XrplWallet(publicKey, privateKey);
            runner.mockedRippled.AddResponse("account_info", accountInfoData);
            runner.mockedRippled.AddResponse("ledger", ledgerData);
            runner.mockedRippled.AddResponse("server_info", serverInfoData);
            runner.mockedRippled.AddResponse("submit", submitData);

            try
            {

                Submit response = await runner.client.Submit(tx, wallet);
                Assert.AreEqual("tesSUCCESS", response.EngineResult);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error);
                Assert.Fail();
            }
        }
    }
}


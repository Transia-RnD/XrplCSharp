using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Client;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Methods;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/client/autofill.ts

namespace Xrpl.Tests.ClientLib
{
    [TestClass]
    public class TestUAutofill
    {
        static string Fee = "10";
        static int Sequence = 1432;
        static int LastLedgerSequence = 2908734;

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

        [TestMethod]
        public async Task TestNoAutofill()
        {
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "DepositPreauth" },
                { "Account", "rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf" },
                { "Authorize", "rpZc4mVfWUif9CRoHRKKcmhu1nx2xktxBo" },
                { "Fee", Fee },
                { "Sequence", Sequence },
                { "LastLedgerSequence", LastLedgerSequence },
            };
            Dictionary<string, dynamic> txResult = await runner.client.Autofill(tx);
            Assert.AreEqual(Fee, txResult["Fee"]);
            Assert.AreEqual(Sequence, txResult["Sequence"]);
            Assert.AreEqual(LastLedgerSequence, txResult["LastLedgerSequence"]);
        }

        [TestMethod]
        public async Task TestAutofillXAddress()
        {
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "Payment" },
                { "Account", "XVLhHMPHU98es4dbozjVtdWzVrDjtV18pX8yuPT7y4xaEHi" },
                { "Amount", "1234" },
                { "Destination", "X7AcgcsBL6XDcUb289X4mJ8djcdyKaB5hJDWMArnXr61cqZ" },
            };

            string accountInfoString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"account_data\":{\"Account\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"Balance\":\"922913243\",\"Domain\":\"6578616D706C652E636F6D\",\"EmailHash\":\"23463B99B62A72F26ED677CC556C44E8\",\"Flags\":655360,\"LedgerEntryType\":\"AccountRoot\",\"OwnerCount\":1,\"PreviousTxnID\":\"19899273706A9E040FDB5885EE991A1DC2BAD878A0D6E7DBCFB714E63BF737F7\",\"PreviousTxnLgrSeq\":6614625,\"Sequence\":23,\"TransferRate\":1002000000,\"TickSize\":5,\"WalletLocator\":\"00000000000000000000000000000000000000000000000000000000DEADBEEF\",\"index\":\"396400950EA27EB5710C0D5BE1D2B4689139F168AC5D07C13B8140EC3F82AE71\",\"urlgravatar\":\"http://www.gravatar.com/avatar/23463b99b62a72f26ed677cc556c44e8\",\"signer_lists\":[{\"Flags\":0,\"LedgerEntryType\":\"SignerList\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"D2707DE50E1244B2C2AAEBC78C82A19ABAE0599D29362C16F1B8458EB65CCFE4\",\"PreviousTxnLgrSeq\":3131157,\"SignerEntries\":[{\"SignerEntry\":{\"Account\":\"rpHit3GvUR1VSGh2PXcaaZKEEUnCVxWU2i\",\"SignerWeight\":1}},{\"SignerEntry\":{\"Account\":\"rN4oCm1c6BQz6nru83H52FBSpNbC9VQcRc\",\"SignerWeight\":1}},{\"SignerEntry\":{\"Account\":\"rJ8KhCi67VgbapiKCQN3r1ZA6BMUxUvvnD\",\"SignerWeight\":1}}],\"SignerListID\":0,\"SignerQuorum\":3,\"index\":\"5A9373E02D1DEF7EC9204DEB4819BA42D6AA6BCD878DC8C853062E9DD9708D11\"}]},\"ledger_index\":9592219}}";
            Dictionary<string, dynamic> accountInfoData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(accountInfoString);

            string ledgerString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"ledger\":{\"account_hash\":\"EC028EC32896D537ECCA18D18BEBE6AE99709FEFF9EF72DBD3A7819E918D8B96\",\"close_time\":464908910,\"parent_close_time\":464908900,\"close_time_human\":\"2014-Sep-2421:21:50\",\"close_time_resolution\":10,\"closed\":true,\"close_flags\":0,\"ledger_hash\":\"0F7ED9F40742D8A513AE86029462B7A6768325583DF8EE21B7EC663019DD6A0F\",\"ledger_index\":\"9038214\",\"parent_hash\":\"4BB9CBE44C39DC67A1BE849C7467FE1A6D1F73949EA163C38A0121A15E04FFDE\",\"total_coins\":\"99999973964317514\",\"transaction_hash\":\"ECB730839EB55B1B114D5D1AD2CD9A932C35BA9AB6D3A8C2F08935EAC2BAC239\",\"transactions\":[\"1FC4D12C30CE206A6E23F46FAC62BD393BE9A79A1C452C6F3A04A13BC7A5E5A3\",\"E25C38FDB8DD4A2429649588638EE05D055EE6D839CABAF8ABFB4BD17CFE1F3E\"]},\"ledger_hash\":\"1723099E269C77C4BDE86C83FA6415D71CF20AA5CB4A94E5C388ED97123FB55B\",\"ledger_index\":9038214,\"validated\":true}}";
            Dictionary<string, dynamic> ledgerData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ledgerString);

            string serverInfoString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"info\":{\"build_version\":\"0.24.0-rc1\",\"complete_ledgers\":\"32570-6595042\",\"hostid\":\"ARTS\",\"io_latency_ms\":1,\"last_close\":{\"converge_time_s\":2.007,\"proposers\":4},\"load_factor\":1,\"peers\":53,\"pubkey_node\":\"n94wWvFUmaKGYrKUGgpv1DyYgDeXRGdACkNQaSe7zJiy5Znio7UC\",\"server_state\":\"full\",\"validated_ledger\":{\"age\":5,\"base_fee_xrp\":0.00001,\"hash\":\"4482DEE5362332F54A4036ED57EE1767C9F33CF7CE5A6670355C16CECE381D46\",\"reserve_base_xrp\":20,\"reserve_inc_xrp\":5,\"seq\":6595042},\"validation_quorum\":3}}}";
            Dictionary<string, dynamic> serverInfoData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(serverInfoString);

            runner.mockedRippled.AddResponse("account_info", accountInfoData);
            runner.mockedRippled.AddResponse("ledger", ledgerData);
            runner.mockedRippled.AddResponse("server_info", serverInfoData);

            Dictionary<string, dynamic> txResult = await runner.client.Autofill(tx);
            Assert.AreEqual("rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf", txResult["Account"]);
            Assert.AreEqual("r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59", txResult["Destination"]);
            Assert.AreEqual(Fee, txResult["Fee"]);
            Assert.AreEqual(Sequence, txResult["Sequence"]);
            Assert.AreEqual(LastLedgerSequence, txResult["LastLedgerSequence"]);
        }

        [TestMethod]
        public async Task TestAutofillSequence()
        {
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "DepositPreauth" },
                { "Account", "rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf" },
                { "Authorize", "rpZc4mVfWUif9CRoHRKKcmhu1nx2xktxBo" },
                { "Fee", Fee },
                { "LastLedgerSequence", LastLedgerSequence },
            };

            string accountInfoString = "{\"status\":\"success\",\"type\":\"response\",\"result\":{\"account_data\":{\"Sequence\":23,},},}";
            Dictionary<string, dynamic> accountInfoData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(accountInfoString);

            runner.mockedRippled.AddResponse("account_info", accountInfoData);

            Dictionary<string, dynamic> txResult = await runner.client.Autofill(tx);
            Assert.IsTrue(23 == txResult["Sequence"]);
        }

        [TestMethod]
        //[ExpectedException(typeof(NotConnectedException))]
        public void TestAutofillDeteteBlockers()
        {
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "DeleteAccount" },
                { "Account", "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn" },
                { "Destination", "X7AcgcsBL6XDcUb289X4mJ8djcdyKaB5hJDWMArnXr61cqZ" },
                { "Fee", Fee },
                { "Sequence", Sequence },
                { "LastLedgerSequence", LastLedgerSequence },
            };

            string accountInfoString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"account_data\":{\"Account\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"Balance\":\"922913243\",\"Domain\":\"6578616D706C652E636F6D\",\"EmailHash\":\"23463B99B62A72F26ED677CC556C44E8\",\"Flags\":655360,\"LedgerEntryType\":\"AccountRoot\",\"OwnerCount\":1,\"PreviousTxnID\":\"19899273706A9E040FDB5885EE991A1DC2BAD878A0D6E7DBCFB714E63BF737F7\",\"PreviousTxnLgrSeq\":6614625,\"Sequence\":23,\"TransferRate\":1002000000,\"TickSize\":5,\"WalletLocator\":\"00000000000000000000000000000000000000000000000000000000DEADBEEF\",\"index\":\"396400950EA27EB5710C0D5BE1D2B4689139F168AC5D07C13B8140EC3F82AE71\",\"urlgravatar\":\"http://www.gravatar.com/avatar/23463b99b62a72f26ed677cc556c44e8\",\"signer_lists\":[{\"Flags\":0,\"LedgerEntryType\":\"SignerList\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"D2707DE50E1244B2C2AAEBC78C82A19ABAE0599D29362C16F1B8458EB65CCFE4\",\"PreviousTxnLgrSeq\":3131157,\"SignerEntries\":[{\"SignerEntry\":{\"Account\":\"rpHit3GvUR1VSGh2PXcaaZKEEUnCVxWU2i\",\"SignerWeight\":1}},{\"SignerEntry\":{\"Account\":\"rN4oCm1c6BQz6nru83H52FBSpNbC9VQcRc\",\"SignerWeight\":1}},{\"SignerEntry\":{\"Account\":\"rJ8KhCi67VgbapiKCQN3r1ZA6BMUxUvvnD\",\"SignerWeight\":1}}],\"SignerListID\":0,\"SignerQuorum\":3,\"index\":\"5A9373E02D1DEF7EC9204DEB4819BA42D6AA6BCD878DC8C853062E9DD9708D11\"}]},\"ledger_index\":9592219}}";
            Dictionary<string, dynamic> accountInfoData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(accountInfoString);

            string ledgerString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"ledger\":{\"account_hash\":\"EC028EC32896D537ECCA18D18BEBE6AE99709FEFF9EF72DBD3A7819E918D8B96\",\"close_time\":464908910,\"parent_close_time\":464908900,\"close_time_human\":\"2014-Sep-2421:21:50\",\"close_time_resolution\":10,\"closed\":true,\"close_flags\":0,\"ledger_hash\":\"0F7ED9F40742D8A513AE86029462B7A6768325583DF8EE21B7EC663019DD6A0F\",\"ledger_index\":\"9038214\",\"parent_hash\":\"4BB9CBE44C39DC67A1BE849C7467FE1A6D1F73949EA163C38A0121A15E04FFDE\",\"total_coins\":\"99999973964317514\",\"transaction_hash\":\"ECB730839EB55B1B114D5D1AD2CD9A932C35BA9AB6D3A8C2F08935EAC2BAC239\",\"transactions\":[\"1FC4D12C30CE206A6E23F46FAC62BD393BE9A79A1C452C6F3A04A13BC7A5E5A3\",\"E25C38FDB8DD4A2429649588638EE05D055EE6D839CABAF8ABFB4BD17CFE1F3E\"]},\"ledger_hash\":\"1723099E269C77C4BDE86C83FA6415D71CF20AA5CB4A94E5C388ED97123FB55B\",\"ledger_index\":9038214,\"validated\":true}}";
            Dictionary<string, dynamic> ledgerData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ledgerString);

            string serverInfoString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"info\":{\"build_version\":\"0.24.0-rc1\",\"complete_ledgers\":\"32570-6595042\",\"hostid\":\"ARTS\",\"io_latency_ms\":1,\"last_close\":{\"converge_time_s\":2.007,\"proposers\":4},\"load_factor\":1,\"peers\":53,\"pubkey_node\":\"n94wWvFUmaKGYrKUGgpv1DyYgDeXRGdACkNQaSe7zJiy5Znio7UC\",\"server_state\":\"full\",\"validated_ledger\":{\"age\":5,\"base_fee_xrp\":0.00001,\"hash\":\"4482DEE5362332F54A4036ED57EE1767C9F33CF7CE5A6670355C16CECE381D46\",\"reserve_base_xrp\":20,\"reserve_inc_xrp\":5,\"seq\":6595042},\"validation_quorum\":3}}}";
            Dictionary<string, dynamic> serverInfoData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(serverInfoString);

            string accountObjectsString = "{\"id\":1,\"status\":\"success\",\"type\":\"response\",\"result\":{\"account\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"account_objects\":[{\"Balance\":{\"currency\":\"ASP\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"0\"},\"Flags\":65536,\"HighLimit\":{\"currency\":\"ASP\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"ASP\",\"issuer\":\"r3vi7mWxru9rJCxETCyA1CHvzL96eZWx5z\",\"value\":\"10\"},\"LowNode\":\"0000000000000000\",\"PreviousTxnID\":\"BF7555B0F018E3C5E2A3FF9437A1A5092F32903BE246202F988181B9CED0D862\",\"PreviousTxnLgrSeq\":1438879,\"index\":\"2243B0B630EA6F7330B654EFA53E27A7609D9484E535AB11B7F946DF3D247CE9\"},{\"Balance\":{\"currency\":\"XAU\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"0\"},\"Flags\":3342336,\"HighLimit\":{\"currency\":\"XAU\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"XAU\",\"issuer\":\"r3vi7mWxru9rJCxETCyA1CHvzL96eZWx5z\",\"value\":\"0\"},\"LowNode\":\"0000000000000000\",\"PreviousTxnID\":\"79B26D7D34B950AC2C2F91A299A6888FABB376DD76CFF79D56E805BF439F6942\",\"PreviousTxnLgrSeq\":5982530,\"index\":\"9ED4406351B7A511A012A9B5E7FE4059FA2F7650621379C0013492C315E25B97\"},{\"Balance\":{\"currency\":\"USD\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"0\"},\"Flags\":1114112,\"HighLimit\":{\"currency\":\"USD\",\"issuer\":\"rMwjYedjc7qqtKYVLiAccJSmCwih4LnE2q\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"USD\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"5\"},\"LowNode\":\"0000000000000000\",\"PreviousTxnID\":\"6FE8C824364FB1195BCFEDCB368DFEE3980F7F78D3BF4DC4174BB4C86CF8C5CE\",\"PreviousTxnLgrSeq\":10555014,\"index\":\"2DECFAC23B77D5AEA6116C15F5C6D4669EBAEE9E7EE050A40FE2B1E47B6A9419\"},{\"Balance\":{\"currency\":\"MXN\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"481.992867407479\"},\"Flags\":65536,\"HighLimit\":{\"currency\":\"MXN\",\"issuer\":\"rHpXfibHgSb64n8kK9QWDpdbfqSpYbM9a4\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"MXN\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"1000\"},\"LowNode\":\"0000000000000000\",\"PreviousTxnID\":\"A467BACE5F183CDE1F075F72435FE86BAD8626ED1048EDEFF7562A4CC76FD1C5\",\"PreviousTxnLgrSeq\":3316170,\"index\":\"EC8B9B6B364AF6CB6393A423FDD2DDBA96375EC772E6B50A3581E53BFBDFDD9A\"},{\"Balance\":{\"currency\":\"EUR\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"0.793598266778297\"},\"Flags\":1114112,\"HighLimit\":{\"currency\":\"EUR\",\"issuer\":\"rLEsXccBGNR3UPuPu2hUXPjziKC3qKSBun\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"EUR\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"1\"},\"LowNode\":\"0000000000000000\",\"PreviousTxnID\":\"E9345D44433EA368CFE1E00D84809C8E695C87FED18859248E13662D46A0EC46\",\"PreviousTxnLgrSeq\":5447146,\"index\":\"4513749B30F4AF8DA11F077C448128D6486BF12854B760E4E5808714588AA915\"},{\"Balance\":{\"currency\":\"CNY\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"0\"},\"Flags\":2228224,\"HighLimit\":{\"currency\":\"CNY\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"3\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"CNY\",\"issuer\":\"rnuF96W4SZoCJmbHYBFoJZpR8eCaxNvekK\",\"value\":\"0\"},\"LowNode\":\"0000000000000008\",\"PreviousTxnID\":\"2FDDC81F4394695B01A47913BEC4281AC9A283CC8F903C14ADEA970F60E57FCF\",\"PreviousTxnLgrSeq\":5949673,\"index\":\"578C327DA8944BDE2E10C9BA36AFA2F43E06C8D1E8819FB225D266CBBCFDE5CE\"},{\"Balance\":{\"currency\":\"DYM\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"1.336889190631542\"},\"Flags\":65536,\"HighLimit\":{\"currency\":\"DYM\",\"issuer\":\"rGwUWgN5BEg3QGNY3RX2HfYowjUTZdid3E\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"DYM\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"3\"},\"LowNode\":\"0000000000000000\",\"PreviousTxnID\":\"6DA2BD02DFB83FA4DAFC2651860B60071156171E9C021D9E0372A61A477FFBB1\",\"PreviousTxnLgrSeq\":8818732,\"index\":\"5A2A5FF12E71AEE57564E624117BBA68DEF78CD564EF6259F92A011693E027C7\"},{\"Balance\":{\"currency\":\"CHF\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"-0.3488146605801446\"},\"Flags\":131072,\"HighLimit\":{\"currency\":\"CHF\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"CHF\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0\"},\"LowNode\":\"000000000000008C\",\"PreviousTxnID\":\"722394372525A13D1EAAB005642F50F05A93CF63F7F472E0F91CDD6D38EB5869\",\"PreviousTxnLgrSeq\":2687590,\"index\":\"F2DBAD20072527F6AD02CE7F5A450DBC72BE2ABB91741A8A3ADD30D5AD7A99FB\"},{\"Balance\":{\"currency\":\"BTC\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"0\"},\"Flags\":131072,\"HighLimit\":{\"currency\":\"BTC\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"3\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0\"},\"LowNode\":\"0000000000000043\",\"PreviousTxnID\":\"03EDF724397D2DEE70E49D512AECD619E9EA536BE6CFD48ED167AE2596055C9A\",\"PreviousTxnLgrSeq\":8317037,\"index\":\"767C12AF647CDF5FEB9019B37018748A79C50EDAF87E8D4C7F39F78AA7CA9765\"},{\"Balance\":{\"currency\":\"USD\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"-16.00534471983042\"},\"Flags\":131072,\"HighLimit\":{\"currency\":\"USD\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"5000\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0\"},\"LowNode\":\"000000000000004A\",\"PreviousTxnID\":\"CFFF5CFE623C9543308C6529782B6A6532207D819795AAFE85555DB8BF390FE7\",\"PreviousTxnLgrSeq\":14365854,\"index\":\"826CF5BFD28F3934B518D0BDF3231259CBD3FD0946E3C3CA0C97D2C75D2D1A09\"}],\"ledger_hash\":\"053DF17D2289D1C4971C22F235BC1FCA7D4B3AE966F842E5819D0749E0B8ECD3\",\"ledger_index\":14378733,\"validated\":true}}";
            Dictionary<string, dynamic> accountObjectsData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(accountObjectsString);

            runner.mockedRippled.AddResponse("account_info", accountInfoData);
            runner.mockedRippled.AddResponse("ledger", ledgerData);
            runner.mockedRippled.AddResponse("server_info", serverInfoData);
            runner.mockedRippled.AddResponse("account_objects", accountObjectsData);

            Dictionary<string, dynamic> txResult = runner.client.Autofill(tx).Result;
        }

        [TestMethod]
        public async Task TestAutofillFee()
        {
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "DepositPreauth" },
                { "Account", "rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf" },
                { "Authorize", "rpZc4mVfWUif9CRoHRKKcmhu1nx2xktxBo" },
                { "Sequence", Sequence },
                { "LastLedgerSequence", LastLedgerSequence },
            };

            string serverInfoString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"info\":{\"build_version\":\"0.24.0-rc1\",\"complete_ledgers\":\"32570-6595042\",\"hostid\":\"ARTS\",\"io_latency_ms\":1,\"last_close\":{\"converge_time_s\":2.007,\"proposers\":4},\"load_factor\":1,\"peers\":53,\"pubkey_node\":\"n94wWvFUmaKGYrKUGgpv1DyYgDeXRGdACkNQaSe7zJiy5Znio7UC\",\"server_state\":\"full\",\"validated_ledger\":{\"age\":5,\"base_fee_xrp\":0.00001,\"hash\":\"4482DEE5362332F54A4036ED57EE1767C9F33CF7CE5A6670355C16CECE381D46\",\"reserve_base_xrp\":20,\"reserve_inc_xrp\":5,\"seq\":6595042},\"validation_quorum\":3}}}";
            Dictionary<string, dynamic> serverInfoData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(serverInfoString);

            runner.mockedRippled.AddResponse("server_info", serverInfoData);

            Dictionary<string, dynamic> txResult = await runner.client.Autofill(tx);
            Assert.IsTrue("12" == txResult["Fee"]);
        }

        [TestMethod]
        public async Task TestAutofillEscrowFinish()
        {
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "Account", "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn" },
                { "TransactionType", "EscrowFinish" },
                { "Owner", "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn" },
                { "OfferSequence", 7 },
                { "Condition", "A0258020E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855810100" },
                { "Fulfillment", "A0028000" },
            };

            string accountInfoString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"account_data\":{\"Account\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"Balance\":\"922913243\",\"Domain\":\"6578616D706C652E636F6D\",\"EmailHash\":\"23463B99B62A72F26ED677CC556C44E8\",\"Flags\":655360,\"LedgerEntryType\":\"AccountRoot\",\"OwnerCount\":1,\"PreviousTxnID\":\"19899273706A9E040FDB5885EE991A1DC2BAD878A0D6E7DBCFB714E63BF737F7\",\"PreviousTxnLgrSeq\":6614625,\"Sequence\":23,\"TransferRate\":1002000000,\"TickSize\":5,\"WalletLocator\":\"00000000000000000000000000000000000000000000000000000000DEADBEEF\",\"index\":\"396400950EA27EB5710C0D5BE1D2B4689139F168AC5D07C13B8140EC3F82AE71\",\"urlgravatar\":\"http://www.gravatar.com/avatar/23463b99b62a72f26ed677cc556c44e8\",\"signer_lists\":[{\"Flags\":0,\"LedgerEntryType\":\"SignerList\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"D2707DE50E1244B2C2AAEBC78C82A19ABAE0599D29362C16F1B8458EB65CCFE4\",\"PreviousTxnLgrSeq\":3131157,\"SignerEntries\":[{\"SignerEntry\":{\"Account\":\"rpHit3GvUR1VSGh2PXcaaZKEEUnCVxWU2i\",\"SignerWeight\":1}},{\"SignerEntry\":{\"Account\":\"rN4oCm1c6BQz6nru83H52FBSpNbC9VQcRc\",\"SignerWeight\":1}},{\"SignerEntry\":{\"Account\":\"rJ8KhCi67VgbapiKCQN3r1ZA6BMUxUvvnD\",\"SignerWeight\":1}}],\"SignerListID\":0,\"SignerQuorum\":3,\"index\":\"5A9373E02D1DEF7EC9204DEB4819BA42D6AA6BCD878DC8C853062E9DD9708D11\"}]},\"ledger_index\":9592219}}";
            Dictionary<string, dynamic> accountInfoData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(accountInfoString);

            string ledgerString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"ledger\":{\"account_hash\":\"EC028EC32896D537ECCA18D18BEBE6AE99709FEFF9EF72DBD3A7819E918D8B96\",\"close_time\":464908910,\"parent_close_time\":464908900,\"close_time_human\":\"2014-Sep-2421:21:50\",\"close_time_resolution\":10,\"closed\":true,\"close_flags\":0,\"ledger_hash\":\"0F7ED9F40742D8A513AE86029462B7A6768325583DF8EE21B7EC663019DD6A0F\",\"ledger_index\":\"9038214\",\"parent_hash\":\"4BB9CBE44C39DC67A1BE849C7467FE1A6D1F73949EA163C38A0121A15E04FFDE\",\"total_coins\":\"99999973964317514\",\"transaction_hash\":\"ECB730839EB55B1B114D5D1AD2CD9A932C35BA9AB6D3A8C2F08935EAC2BAC239\",\"transactions\":[\"1FC4D12C30CE206A6E23F46FAC62BD393BE9A79A1C452C6F3A04A13BC7A5E5A3\",\"E25C38FDB8DD4A2429649588638EE05D055EE6D839CABAF8ABFB4BD17CFE1F3E\"]},\"ledger_hash\":\"1723099E269C77C4BDE86C83FA6415D71CF20AA5CB4A94E5C388ED97123FB55B\",\"ledger_index\":9038214,\"validated\":true}}";
            Dictionary<string, dynamic> ledgerData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ledgerString);

            string serverInfoString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"info\":{\"build_version\":\"0.24.0-rc1\",\"complete_ledgers\":\"32570-6595042\",\"hostid\":\"ARTS\",\"io_latency_ms\":1,\"last_close\":{\"converge_time_s\":2.007,\"proposers\":4},\"load_factor\":1,\"peers\":53,\"pubkey_node\":\"n94wWvFUmaKGYrKUGgpv1DyYgDeXRGdACkNQaSe7zJiy5Znio7UC\",\"server_state\":\"full\",\"validated_ledger\":{\"age\":5,\"base_fee_xrp\":0.00001,\"hash\":\"4482DEE5362332F54A4036ED57EE1767C9F33CF7CE5A6670355C16CECE381D46\",\"reserve_base_xrp\":20,\"reserve_inc_xrp\":5,\"seq\":6595042},\"validation_quorum\":3}}}";
            Dictionary<string, dynamic> serverInfoData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(serverInfoString);

            runner.mockedRippled.AddResponse("account_info", accountInfoData);
            runner.mockedRippled.AddResponse("ledger", ledgerData);
            runner.mockedRippled.AddResponse("server_info", serverInfoData);

            Dictionary<string, dynamic> txResult = await runner.client.Autofill(tx);
            Assert.IsTrue("399" == txResult["Fee"]);
        }

        [TestMethod]
        public async Task TestAutofillDelete()
        {
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "Account", "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn" },
                { "TransactionType", "AccountDelete" },
                { "Destination", "X7AcgcsBL6XDcUb289X4mJ8djcdyKaB5hJDWMArnXr61cqZ" },
            };

            string accountInfoString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"account_data\":{\"Account\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"Balance\":\"922913243\",\"Domain\":\"6578616D706C652E636F6D\",\"EmailHash\":\"23463B99B62A72F26ED677CC556C44E8\",\"Flags\":655360,\"LedgerEntryType\":\"AccountRoot\",\"OwnerCount\":1,\"PreviousTxnID\":\"19899273706A9E040FDB5885EE991A1DC2BAD878A0D6E7DBCFB714E63BF737F7\",\"PreviousTxnLgrSeq\":6614625,\"Sequence\":23,\"TransferRate\":1002000000,\"TickSize\":5,\"WalletLocator\":\"00000000000000000000000000000000000000000000000000000000DEADBEEF\",\"index\":\"396400950EA27EB5710C0D5BE1D2B4689139F168AC5D07C13B8140EC3F82AE71\",\"urlgravatar\":\"http://www.gravatar.com/avatar/23463b99b62a72f26ed677cc556c44e8\",\"signer_lists\":[{\"Flags\":0,\"LedgerEntryType\":\"SignerList\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"D2707DE50E1244B2C2AAEBC78C82A19ABAE0599D29362C16F1B8458EB65CCFE4\",\"PreviousTxnLgrSeq\":3131157,\"SignerEntries\":[{\"SignerEntry\":{\"Account\":\"rpHit3GvUR1VSGh2PXcaaZKEEUnCVxWU2i\",\"SignerWeight\":1}},{\"SignerEntry\":{\"Account\":\"rN4oCm1c6BQz6nru83H52FBSpNbC9VQcRc\",\"SignerWeight\":1}},{\"SignerEntry\":{\"Account\":\"rJ8KhCi67VgbapiKCQN3r1ZA6BMUxUvvnD\",\"SignerWeight\":1}}],\"SignerListID\":0,\"SignerQuorum\":3,\"index\":\"5A9373E02D1DEF7EC9204DEB4819BA42D6AA6BCD878DC8C853062E9DD9708D11\"}]},\"ledger_index\":9592219}}";
            Dictionary<string, dynamic> accountInfoData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(accountInfoString);

            string ledgerString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"ledger\":{\"account_hash\":\"EC028EC32896D537ECCA18D18BEBE6AE99709FEFF9EF72DBD3A7819E918D8B96\",\"close_time\":464908910,\"parent_close_time\":464908900,\"close_time_human\":\"2014-Sep-2421:21:50\",\"close_time_resolution\":10,\"closed\":true,\"close_flags\":0,\"ledger_hash\":\"0F7ED9F40742D8A513AE86029462B7A6768325583DF8EE21B7EC663019DD6A0F\",\"ledger_index\":\"9038214\",\"parent_hash\":\"4BB9CBE44C39DC67A1BE849C7467FE1A6D1F73949EA163C38A0121A15E04FFDE\",\"total_coins\":\"99999973964317514\",\"transaction_hash\":\"ECB730839EB55B1B114D5D1AD2CD9A932C35BA9AB6D3A8C2F08935EAC2BAC239\",\"transactions\":[\"1FC4D12C30CE206A6E23F46FAC62BD393BE9A79A1C452C6F3A04A13BC7A5E5A3\",\"E25C38FDB8DD4A2429649588638EE05D055EE6D839CABAF8ABFB4BD17CFE1F3E\"]},\"ledger_hash\":\"1723099E269C77C4BDE86C83FA6415D71CF20AA5CB4A94E5C388ED97123FB55B\",\"ledger_index\":9038214,\"validated\":true}}";
            Dictionary<string, dynamic> ledgerData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ledgerString);

            string serverInfoString = "{\"status\":\"success\",\"type\":\"response\",\"result\":{\"state\":{\"validated_ledger\":{\"reserve_inc\":2000000,},},},}";
            Dictionary<string, dynamic> serverInfoData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(serverInfoString);

            string serverInfo1String = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"info\":{\"build_version\":\"0.24.0-rc1\",\"complete_ledgers\":\"32570-6595042\",\"hostid\":\"ARTS\",\"io_latency_ms\":1,\"last_close\":{\"converge_time_s\":2.007,\"proposers\":4},\"load_factor\":1,\"peers\":53,\"pubkey_node\":\"n94wWvFUmaKGYrKUGgpv1DyYgDeXRGdACkNQaSe7zJiy5Znio7UC\",\"server_state\":\"full\",\"validated_ledger\":{\"age\":5,\"base_fee_xrp\":0.00001,\"hash\":\"4482DEE5362332F54A4036ED57EE1767C9F33CF7CE5A6670355C16CECE381D46\",\"reserve_base_xrp\":20,\"reserve_inc_xrp\":5,\"seq\":6595042},\"validation_quorum\":3}}}";
            Dictionary<string, dynamic> serverInfo1Data = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(serverInfo1String);

            string accountObjectsString = "{\"id\":1,\"status\":\"success\",\"type\":\"response\",\"result\":{\"account\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"account_objects\":[],\"ledger_hash\":\"053DF17D2289D1C4971C22F235BC1FCA7D4B3AE966F842E5819D0749E0B8ECD3\",\"ledger_index\":14378733,\"validated\":true}}";
            Dictionary<string, dynamic> accountObjectsData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(accountObjectsString);

            runner.mockedRippled.AddResponse("account_info", accountInfoData);
            runner.mockedRippled.AddResponse("ledger", ledgerData);
            runner.mockedRippled.AddResponse("server_info", serverInfoData);
            runner.mockedRippled.AddResponse("server_info", serverInfo1Data);
            runner.mockedRippled.AddResponse("server_info", accountObjectsData);

            Dictionary<string, dynamic> txResult = await runner.client.Autofill(tx);
            Assert.IsTrue("200000" == txResult["Fee"]);
        }

        [TestMethod]
        public async Task TestAutofillLastLedger()
        {
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "DepositPreauth" },
                { "Account", "rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf" },
                { "Authorize", "rpZc4mVfWUif9CRoHRKKcmhu1nx2xktxBo" },
                { "Fee", Fee },
                { "Sequence", Sequence },
            };

            string ledgerString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"ledger\":{\"ledger_index\":\"9038214\",}}}";
            Dictionary<string, dynamic> ledgerData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ledgerString);

            runner.mockedRippled.AddResponse("ledger", ledgerData);

            Dictionary<string, dynamic> txResult = await runner.client.Autofill(tx);
            Assert.IsTrue(9038234 == txResult["LastLedgerSequence"]);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Client;
using Xrpl.Models.Methods;
using Xrpl.Models.Subscriptions;
using Xrpl.Sugar;
using XrplTests.Xrpl.MockRippled;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/client/subscribe.ts

namespace XrplTests.Xrpl.ClientLib
{
    [TestClass]
    public class TestUSubscribe
    {

        public static SetupUnitClient runner;

        [ClassInitialize]
        public static async Task MyClassInitializeAsync(TestContext testContext)
        {
            runner = await new SetupUnitClient().SetupClient();
        }

        [ClassCleanup]
        public static async Task MyClassCleanupAsync()
        {
            await runner.client.Disconnect();
        }

        [TestMethod]
        public async Task TestSubscribe()
        {

            string jsonString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"fee_base\":10,\"fee_ref\":10,\"hostid\":\"NAP\",\"ledger_hash\":\"60EBABF55F6AB58864242CADA0B24FBEA027F2426917F39CA56576B335C0065A\",\"ledger_index\":8819951,\"ledger_time\":463782770,\"load_base\":256,\"load_factor\":256,\"pubkey_node\":\"n9Lt7DgQmxjHF5mYJsV2U9anALHmPem8PWQHWGpw4XMz79HA5aJY\",\"random\":\"EECFEE93BBB608914F190EC177B11DE52FC1D75D2C97DACBD26D2DFC6050E874\",\"reserve_base\":20000000,\"reserve_inc\":5000000,\"server_status\":\"full\",\"validated_ledgers\":\"32570-8819951\"}}";
            Dictionary<string, dynamic> jsonData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);
            runner.mockedRippled.AddResponse("subscribe", jsonData);
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "command", "subscribe" },
            };
            await runner.client.Request(tx);
        }

        [TestMethod]
        public async Task TestSubscribe1()
        {
            var server = "wss://xrplcluster.com/";

            var client = new XrplClient(server);

            client.Connect().Wait();

            client.OnConnected += () =>
            {
                //Console.WriteLine("CONNECTED");
            };

            var subscribe = await runner.client.Subscribe(
            new SubscribeRequest()
            {
                Streams = new List<string>(new[]
                {
                    "ledger",
                })
            });
            //Console.WriteLine(subscribe);

            while (client.connection.State() == WebSocketState.Open)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        [TestMethod]
        public async Task TestUnsubscribe()
        {

            string jsonString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{}}";
            Dictionary<string, dynamic> jsonData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);
            runner.mockedRippled.AddResponse("unsubscribe", jsonData);
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "command", "unsubscribe" },
            };
            await runner.client.Request(tx);
        }

        [TestMethod]
        public void TestEmitsTransaction()
        {
            bool isDone = false;
            runner.client.OnTransaction += r =>
            {
                Assert.IsTrue(r.Type == ResponseStreamType.transaction);
                isDone = true;
            };

            string jsonString = "{\"engine_result\":\"tesSUCCESS\",\"engine_result_code\":0,\"engine_result_message\":\"Thetransactionwasapplied.Onlyfinalinavalidatedledger.\",\"ledger_hash\":\"922099A5528EFDF820ABFAB0CAAB8647DF6E7103B3BA8CDD3A6D56EAF1B39B3B\",\"ledger_index\":66093882,\"meta\":{\"AffectedNodes\":[{\"DeletedNode\":{\"FinalFields\":{\"Account\":\"rnruxxLTbJUMNtFNBJ7X2xSiy1KE7ajUuH\",\"BookDirectory\":\"623C4C4AD65873DA787AC85A0A1385FE6233B6DE100799474F1E3E58B40BAC52\",\"BookNode\":\"0\",\"Flags\":0,\"OwnerNode\":\"0\",\"PreviousTxnID\":\"E3E2E94FE181C5F5E03E2FE5347C4E8E27E18290FF3B7FA6BA9B124AD54F147D\",\"PreviousTxnLgrSeq\":66093873,\"Sequence\":18466973,\"TakerGets\":\"9416365482\",\"TakerPays\":{\"currency\":\"CNY\",\"issuer\":\"rJ1adrpGS3xsnQMb9Cw54tWJVFPuSdZHK\",\"value\":\"80159.63607543072\"}},\"LedgerEntryType\":\"Offer\",\"LedgerIndex\":\"3A93F99B4CB2F4FB0F4F5182E85C37855611E6470262DF63896B6E0AA4231AE0\"}},{\"CreatedNode\":{\"LedgerEntryType\":\"DirectoryNode\",\"LedgerIndex\":\"623C4C4AD65873DA787AC85A0A1385FE6233B6DE100799474F1E2BD6998872D5\",\"NewFields\":{\"ExchangeRate\":\"4f1e2bd6998872d5\",\"RootIndex\":\"623C4C4AD65873DA787AC85A0A1385FE6233B6DE100799474F1E2BD6998872D5\",\"TakerPaysCurrency\":\"000000000000000000000000434E590000000000\",\"TakerPaysIssuer\":\"0360E3E0751BD9A566CD03FA6CAFC78118B82BA0\"}}},{\"DeletedNode\":{\"FinalFields\":{\"ExchangeRate\":\"4f1e3e58b40bac52\",\"Flags\":0,\"RootIndex\":\"623C4C4AD65873DA787AC85A0A1385FE6233B6DE100799474F1E3E58B40BAC52\",\"TakerGetsCurrency\":\"0000000000000000000000000000000000000000\",\"TakerGetsIssuer\":\"0000000000000000000000000000000000000000\",\"TakerPaysCurrency\":\"000000000000000000000000434E590000000000\",\"TakerPaysIssuer\":\"0360E3E0751BD9A566CD03FA6CAFC78118B82BA0\"},\"LedgerEntryType\":\"DirectoryNode\",\"LedgerIndex\":\"623C4C4AD65873DA787AC85A0A1385FE6233B6DE100799474F1E3E58B40BAC52\"}},{\"CreatedNode\":{\"LedgerEntryType\":\"Offer\",\"LedgerIndex\":\"8934A20864E420B7D0F6CDC61F5D8D2E609DEB8E25D3CB26A1B595032483A4C8\",\"NewFields\":{\"Account\":\"rnruxxLTbJUMNtFNBJ7X2xSiy1KE7ajUuH\",\"BookDirectory\":\"623C4C4AD65873DA787AC85A0A1385FE6233B6DE100799474F1E2BD6998872D5\",\"Sequence\":18466977,\"TakerGets\":\"8221180253\",\"TakerPays\":{\"currency\":\"CNY\",\"issuer\":\"rJ1adrpGS3xsnQMb9Cw54tWJVFPuSdZHK\",\"value\":\"69817.9622410017\"}}}},{\"ModifiedNode\":{\"FinalFields\":{\"Account\":\"rnruxxLTbJUMNtFNBJ7X2xSiy1KE7ajUuH\",\"Balance\":\"5116214416\",\"Flags\":0,\"OwnerCount\":5,\"Sequence\":18466978},\"LedgerEntryType\":\"AccountRoot\",\"LedgerIndex\":\"9AC13F682F58D555C134D098EEEE1A14BECB904C65ACBBB0046B35B405E66A75\",\"PreviousFields\":{\"Balance\":\"5116214428\",\"Sequence\":18466977},\"PreviousTxnID\":\"48DF68A5C9D50C2CB2FE750E3D3A40B041FDD12FD2185DF4F97B2A0CA379DCB0\",\"PreviousTxnLgrSeq\":66093873}},{\"ModifiedNode\":{\"FinalFields\":{\"Flags\":0,\"Owner\":\"rnruxxLTbJUMNtFNBJ7X2xSiy1KE7ajUuH\",\"RootIndex\":\"FBD0BC6A9DCBC5AEFB9C773EE6351AF11E244DBD1370EDF6801FD607F01D3DF8\"},\"LedgerEntryType\":\"DirectoryNode\",\"LedgerIndex\":\"FBD0BC6A9DCBC5AEFB9C773EE6351AF11E244DBD1370EDF6801FD607F01D3DF8\"}}],\"TransactionIndex\":40,\"TransactionResult\":\"tesSUCCESS\"},\"status\":\"closed\",\"transaction\":{\"Account\":\"rnruxxLTbJUMNtFNBJ7X2xSiy1KE7ajUuH\",\"Fee\":\"12\",\"Flags\":0,\"LastLedgerSequence\":66093884,\"OfferSequence\":18466973,\"Sequence\":18466977,\"SigningPubKey\":\"026B8A4318970123B0BB3DC528C4DA62C874AD4A01F399DBEF21D621DDC32F6C81\",\"TakerGets\":\"8221180253\",\"TakerPays\":{\"currency\":\"CNY\",\"issuer\":\"rJ1adrpGS3xsnQMb9Cw54tWJVFPuSdZHK\",\"value\":\"69817.9622410017\"},\"TransactionType\":\"OfferCreate\",\"TxnSignature\":\"304402200E0821A9FC8A0A7CA72DC0CEC3BD2AC1317A8DCFAAE1F27EB7C69C79EB475DD3022046BBFA7DFAD9B7186CAEA798358C0959014B27B2B1EF3D6CCEF5EC0EA346D692\",\"date\":683942752,\"hash\":\"775266C42CED11D5FC6DB61686177FCEA689E7A79E6B0017586E95FA3E9EDD10\",\"owner_funds\":\"5071214380\"},\"type\":\"transaction\",\"validated\":true}";
            runner.client.connection.OnMessage(jsonString);

            while (isDone == false)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        [TestMethod]
        public void TestEmitsLedger()
        {
            runner.client.OnLedgerClosed += r =>
            {
                //Assert.IsTrue(r.Type == ResponseStreamType.ledgerClosed);
            };

            string jsonString = "{\"fee_base\":10,\"fee_ref\":10,\"ledger_hash\":\"B3980C722D71873D6708723E71B7A28C826BC66C58712ADCEC61603415305CD1\",\"ledger_index\":66093872,\"ledger_time\":683942720,\"reserve_base\":20000000,\"reserve_inc\":5000000,\"txn_count\":70,\"type\":\"ledgerClosed\",\"validated_ledgers\":\"65201743-66093872\"}";
            runner.client.connection.OnMessage(jsonString);
        }

        [TestMethod]
        public void TestEmitsPeerStatusChange()
        {
            runner.client.OnPeerStatusChange += r =>
            {
                Assert.IsTrue(r.Type == ResponseStreamType.consensusPhase);
            };

            string jsonString = "{\"action\":\"CLOSING_LEDGER\",\"date\":508546525,\"ledger_hash\":\"4D4CD9CD543F0C1EF023CC457F5BEFEA59EEF73E4552542D40E7C4FA08D3C320\",\"ledger_index\":18853106,\"ledger_index_max\":18853106,\"ledger_index_min\":18852082,\"type\":\"peerStatusChange\"}";
            runner.client.connection.OnMessage(jsonString);
        }

        [TestMethod]
        public void TestEmitsPathFind()
        {
            runner.client.OnPathFind += r =>
            {
                Assert.IsTrue(r.Type == ResponseStreamType.path_find);
            };

            string jsonString = "{\"alternatives\":[{\"paths_computed\":[[{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"type\":48}],[{\"currency\":\"USD\",\"issuer\":\"rhub8VRN55s94qWKDv6jmDy1pUykJzF3wq\",\"type\":48},{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"type\":48}],[{\"currency\":\"USD\",\"issuer\":\"rhub8VRN55s94qWKDv6jmDy1pUykJzF3wq\",\"type\":48},{\"account\":\"rhub8VRN55s94qWKDv6jmDy1pUykJzF3wq\",\"type\":1},{\"account\":\"rpix35SSFEukMTm64NB4k4BPBS7fXJrLJM\",\"type\":1}],[{\"currency\":\"CNY\",\"issuer\":\"rKiCet8SdvWxPXnAgYarFUXMh1zCPz432Y\",\"type\":48},{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"type\":48}]],\"source_amount\":\"786\"}],\"destination_account\":\"rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn\",\"destination_amount\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.001\"},\"full_reply\":true,\"id\":8,\"source_account\":\"rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn\",\"type\":\"path_find\"}";
            runner.client.connection.OnMessage(jsonString);
        }

        [TestMethod]
        public void TestEmitsValidationReceived()
        {
            runner.client.OnManifestReceived += r =>
            {
                Assert.IsTrue(r.Type == ResponseStreamType.validationReceived);
            };

            string jsonString = "{\"type\":\"validationReceived\",\"amendments\":[\"42426C4D4F1009EE67080A9B7965B44656D7714D104A72F9B4369F97ABF044EE\",\"4C97EBA926031A7CF7D7B36FDE3ED66DDA5421192D63DE53FFB46E43B9DC8373\",\"6781F8368C4771B83E8B821D88F580202BCB4228075297B19E4FDC5233F1EFDC\",\"C1B8D934087225F509BEB5A8EC24447854713EE447D277F69545ABFA0E0FD490\",\"DA1BD556B42D85EA9C84066D028D355B52416734D3283F85E216EA5DA6DB7E13\"],\"base_fee\":10,\"flags\":2147483649,\"full\":true,\"ledger_hash\":\"EC02890710AAA2B71221B0D560CFB22D64317C07B7406B02959AD84BAD33E602\",\"ledger_index\":\"6\",\"load_fee\":256000,\"master_key\":\"nHUon2tpyJEHHYGmxqeGu37cvPYHzrMtUNQFVdCgGNvEkjmCpTqK\",\"reserve_base\":20000000,\"reserve_inc\":5000000,\"signature\":\"3045022100E199B55643F66BC6B37DBC5E185321CF952FD35D13D9E8001EB2564FFB94A07602201746C9A4F7A93647131A2DEB03B76F05E426EC67A5A27D77F4FF2603B9A528E6\",\"signing_time\":515115322,\"validation_public_key\":\"n94Gnc6svmaPPRHUAyyib1gQUov8sYbjLoEwUBYPH39qHZXuo8ZT\"}";
            runner.client.connection.OnMessage(jsonString);
        }

        //[TestMethod]
        //public void TestEmitsManifest()
        //{
        //    runner.client.OnManifestReceived += r =>
        //    {
        //        Assert.IsTrue(r.Type == ResponseStreamType.manifest);
        //    };

        //    string jsonString = "{\"type\":\"manifestReceived\"}";
        //    runner.client.connection.OnMessage(jsonString);
        //}
    }
}


using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Ripple.Core.ShaMapTree;
using Ripple.Core.Tests.Properties;
using Ripple.Core.Types;

namespace Ripple.Core.Tests
{
    [TestClass]
    public class ShaMapTests
    {
        [TestMethod]
        public void EmptyMapHasZeroHash()
        {
            var shamap = new ShaMap();
            Assert.AreEqual(Hash256.Zero, shamap.Hash());
        }

        [TestMethod]
        public void LedgerFull38129Test()
        {
            TestLedgerTreeHashing(Utils.ParseJObject(Resources.LedgerFull38129));
        }

        [TestMethod]
        public void LedgerFull40000Test()
        {
            TestLedgerTreeHashing(Utils.ParseJObject(Resources.LedgerFull40000));
        }

        [TestMethod, Ignore]
        public void LedgerFromFileTest()
        {
            const string ledgerJson = @"Z:\windowsshare\ledger-full-1000000.json";
            if (!File.Exists(ledgerJson)) return;
            var ledger1E6 = Utils.FileToByteArray(ledgerJson);
            var ledger = (JObject)Utils.ParseJson(ledger1E6);
            TestLedgerTreeHashing(ledger);
        }

        [TestMethod, Ignore]
        public void HistoryLoaderTest()
        {
            const string history = @"Z:\windowsshare\history.bin";
            if (!File.Exists(history)) return;
            var loader = new HistoryLoader(StReader.FromFile(history));
            loader.ParseFast((header, state, txns) => true);
        }

        [TestMethod, Ignore]
        public void AccountStateTest()
        {
            const string path = @"Z:\windowsshare\as-ledger-4320278.json";
            JArray state;
            if (!ParseJsonArray(path, out state)) return;
            var stateMap = ParseAccountState(state);
            Assert.AreEqual("CF37E77AE0C3BE12369133B0CA212CE7B0FCB282F5B3F1079739B69944FB0D2E",
                stateMap.Hash().ToString());
        }

        private static bool ParseJsonArray(string path, out JArray state)
        {
            state = null;
            if (!File.Exists(path))
            {
                return false;
            }
            var ledger1E6 = Utils.FileToByteArray(path);
            state = (JArray)Utils.ParseJson(ledger1E6);
            return true;
        }

        private static ShaMap ParseAccountState(JArray state)
        {
            var stateMap = new ShaMap();
            var entries = state.Select((t) =>
            {
                StObject so = t["json"];
                Assert.AreEqual(t["binary"].ToString(), so.ToHex(), t.ToString() + " " + so.ToJson());
                return new LedgerEntry(so);
            });
            foreach (var ledgerEntry in entries)
            {
                stateMap.AddItem(ledgerEntry.Index(), ledgerEntry);
            }
            return stateMap;
        }

        private static void TestLedgerTreeHashing(JObject ledger)
        {
            var txMap = new ShaMap();
            var stateMap = new ShaMap();

            var expectedTxHash = ledger["transaction_hash"].ToString();
            var expectedStateHash = ledger["account_hash"].ToString();

            var transactions = ledger["transactions"].Select(TransactionResult.FromJson);
            var state = ledger["accountState"].Select(t => new LedgerEntry(t));

            foreach (var tr in transactions)
            {
                txMap.AddItem(tr.Hash(), tr);
            }

            foreach (var le in state)
            {
                stateMap.AddItem(le.Index(), le);
            }

            Assert.AreEqual(expectedTxHash, txMap.Hash().ToString());
            Assert.AreEqual(expectedStateHash, stateMap.Hash().ToString());
        }
    }
}
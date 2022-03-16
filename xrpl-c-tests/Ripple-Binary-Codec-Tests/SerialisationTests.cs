using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Ripple.Core.Binary;
using Ripple.Core.Tests.Properties;
using Ripple.Core.Types;
using Ripple.Core.Util;
using Ripple.Signing;
using static System.Text.Encoding;

namespace Ripple.Core.Tests
{

    [TestClass]
    public class SerialisationTests
    {
        public static readonly string MessageBytes = (
            "53545800" +
            "1200002280000000240000" +
            "00016140000000000003E868400000" +
            "000000000A7321EDD3993CDC664789" +
            "6C455F136648B7750723B011475547" +
            "AF60691AA3D7438E021D8114C0A5AB" +
            "EF242802EFED4B041E8F2D4A8CC86A" +
            "E3D18314B5F762798A53D543A014CA" +
            "F8B297CFF8F2F937E8");

        public static readonly string ExpectedSig = "C3646313B08EED6AF4392261A31B961F" +
                                                    "10C66CB733DB7F6CD9EAB079857834C8" +
                                                    "B0334270A2C037E63CDCCC1932E08328" +
                                                    "82B7B7066ECD2FAEDEB4A83DF8AE6303";

        public static readonly string TxJson = @"{
                'Account': 'rJZdUusLDtY9NEsGea7ijqhVrXv98rYBYN',
                'Amount': '1000',
                'Destination': 'rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh',
                'Fee': '10',
                'Flags': 2147483648,
                'Sequence': 1,
                'SigningPubKey': 'EDD3993CDC6647896C455F136648B7750723B011475547AF60691AA3D7438E021D',
                'TransactionType' : 'Payment'
            }";

        [TestMethod]
        public void TransactionSigningTest()
        {
            var json = JObject.Parse(TxJson);
            var obj = StObject.FromJson(json);
            var hex = obj.ToHex();
            
            // The MessageBytes includes the HashPrefix
            Assert.AreEqual(MessageBytes.Substring(8), hex);
            Seed seed = Seed.FromPassPhrase("niq").SetEd25519();
            
            // The ed25519 Signature
            var sig = seed.KeyPair().Sign(B16.Decode(MessageBytes));
            var expectedSig = ExpectedSig;
            Assert.AreEqual(expectedSig, B16.Encode(sig));
        }

        private static JObject GetTestsJson()
        {
            return (JObject) Utils.ParseJson(Resources.DataDrivenTests);
        }

        private static JArray GetTransactionsWithMetaJson()
        {
            return (JArray) Utils.ParseJson(Resources.TransactionsWithMeta);
        }

        [TestMethod]
        public void DataDrivenTransactionWithMetaSerialisationTest()
        {
            var obj = GetTransactionsWithMetaJson();
            var count = 0;
            foreach (var test in obj)
            {
                count ++;
                test["test_count"] = count;
                AssertRecycles("tx_json", "rawTx", test);
                AssertRecycles("meta", "rawMeta", test);
            }
            Assert.AreEqual(2000, count);
        }

        public static void AssertRecycles(string jsonKey, string binaryKey, JToken test)
        {
            var json = test[jsonKey];
            var binary = test[binaryKey];

            string expectedHex = binary.ToString();
            var fromHex = StObject.FromHex(expectedHex);
            AssertDeepEqual(json, fromHex.ToJson(), test);
            StObject o = json;
            var actualHex = o.ToHex();
            Assert.AreEqual(expectedHex, actualHex, $"{test}");
        }

        [TestMethod]
        public void DataDrivenTransactionSerialisationTest()
        {
            var obj = GetTestsJson();
            foreach (var whole in obj["whole_objects"])
            {
                StObject txn = whole["tx_json"];
                Assert.AreEqual(whole["blob_with_no_signing"], txn.ToHex());
                AssertDeepEqual(whole["tx_json"], txn.ToJson(), null);

                var txnFromBinary = StObject.FromHex($"{whole["blob_with_no_signing"]}");
                AssertDeepEqual(whole["tx_json"], txnFromBinary.ToJson(), null);
            }
        }

        [TestMethod]
        public void DataDrivenAmountSerialisationTest()
        {
            var obj = GetTestsJson();
            Func<JToken, bool> predicate = t => t["type"].ToString() == "Amount" && 
                                                 t["expected_hex"] != null;

            var enumerable = obj["values_tests"].Where(predicate);
            var array = enumerable.ToArray();
            var passed = array.Count(test =>
            {
                var expected = test["expected_hex"].ToString();
                var testJson = test["test_json"];
                var parsedAmount = Amount.FromJson(testJson);
                var actual = parsedAmount.ToHex();
                var debugInfo = testJson.Type + " typed: " + test.ToString();
                Assert.AreEqual(expected, actual, debugInfo);
                Assert.AreEqual(test["is_native"], parsedAmount.IsNative, debugInfo);
                // TODO, fix these tests
                //if (test["exponent"] != null)
                //{
                //    Assert.AreEqual(test["exponent"], parsedAmount.Exponent, debugInfo);
                //}
                var reJsonified = parsedAmount.ToJson();
                AssertAmountEqual(testJson, reJsonified);

                return true;
            });
            Assert.AreEqual(array.Length, passed);
        }

        [TestMethod]
        public void CanDeserializeEscrowTransaction()
        {
            string binary = "1200012280000000240000001C201B0055E830202421E4C840202521E376C061400000000098968068400000000000000C7321024B1C46885AD9DEEE7A413026D74BA6161C2F68FA9BD621022CF34CA00FB6FAEC7446304402201E931F36789387DF59058D34345D5EEBF2BD18159FEB8A8601E495054D4065F802200EBD2738EA138BA7C0D5AC07E02B950A84F563EF1AC94F8B4BCAA99595B402A48114656CFDA8B366CAFE7EDC195A6DE87921FB70C2318314A2D0815DD52160FF1979A60C50B00C09ECD669D4";
            StObject stObject = StObject.FromHex(binary);
            Assert.IsNotNull(stObject);          
        }

        private static void AssertAmountEqual(JToken expected, JToken actual)
        {
            if (expected.Type == JTokenType.String)
            {
                Assert.AreEqual(expected.ToString(), actual.ToString());
            }
            if (!JToken.DeepEquals(expected, actual))
            {
                Assert.AreEqual(IouValue.FromString(expected["value"].ToString()).ToString(),
                                IouValue.FromString(expected["value"].ToString()).ToString(),
                                          $"expected: {expected}\n" +
                                          $"actual: {actual}");
            }
        }

        private static void AssertDeepEqual(JToken expected, JToken actual, JToken json)
        {
            Assert.IsTrue(JToken.DeepEquals(actual, expected), 
                          $"expected: {expected}\n" +
                          $"actual: {actual}");
        }
    }
}
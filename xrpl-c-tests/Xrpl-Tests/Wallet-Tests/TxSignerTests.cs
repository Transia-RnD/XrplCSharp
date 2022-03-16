using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Ripple.Core;

namespace Ripple.TxSigning.Tests
{
    [TestClass]
    public class TxSignerTests
    {
        public const string ExpectedTxBlob =
                "12000022800000002400000001614000" +
                "0000000003E868400000000000000A73" +
                "21EDD3993CDC6647896C455F136648B7" +
                "750723B011475547AF60691AA3D7438E" +
                "021D7440C3646313B08EED6AF4392261" +
                "A31B961F10C66CB733DB7F6CD9EAB079" +
                "857834C8B0334270A2C037E63CDCCC19" +
                "32E0832882B7B7066ECD2FAEDEB4A83D" +
                "F8AE63038114C0A5ABEF242802EFED4B" +
                "041E8F2D4A8CC86AE3D18314B5F76279" +
                "8A53D543A014CAF8B297CFF8F2F937E8";

        public const string ExpectedSigningPubKey = "EDD3993CDC6647896C455F136648B7750" +
                                             "723B011475547AF60691AA3D7438E021D";

        public const string ExpectedTxnSignature = "C3646313B08EED6AF4392261A31B961F" +
                                          "10C66CB733DB7F6CD9EAB079857834C8" +
                                          "B0334270A2C037E63CDCCC1932E08328" +
                                          "82B7B7066ECD2FAEDEB4A83DF8AE6303";

        public const string ExpectedHash = "A8A9C869671D35A18DFB69AFB7741062" +
                                           "DF43F73C8A5942AD94EE58ED31477AC6";

        public const string UnsignedTxJson = @"{
            'Account': 'rJZdUusLDtY9NEsGea7ijqhVrXv98rYBYN',
            'Amount': '1000',
            'Destination': 'rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh',
            'Fee': '10',
            'Flags': 2147483648,
            'Sequence': 1,
            'TransactionType' : 'Payment'
        }";

        public const string Secret = "sEd7rBGm5kxzauRTAV2hbsNz7N45X91";

        [TestMethod]
        public void StaticSignJson_ValidTransactionAndEd25559Secret_ValidSignature()
        {
            AssertOk(TxSigner.SignJson(JObject.Parse(UnsignedTxJson), Secret));
        }

        private static void AssertOk(SignedTx signed)
        {
            Assert.AreEqual(ExpectedTxnSignature, signed.TxJson["TxnSignature"]);
            Assert.AreEqual(ExpectedSigningPubKey, signed.TxJson["SigningPubKey"]);
            Assert.AreEqual(ExpectedHash, signed.Hash);
            Assert.AreEqual(ExpectedTxBlob, signed.TxBlob);
        }

        /*

        Notes re: test fixtures

        We test that we can recreate the second transaction submitted here, which
        was signed with an ed25519 key and has deterministic signature, more
        friendly for testing purposes. At the time of writing rippled didn't yet
        support rfc6979 deterministic signatures for secp256k1.

        ➜  rippled git:(develop) ✗ build/clang.debug/rippled submit masterpassphrase '
                    {"TransactionType" : "Payment",  
                     "Amount" : "1000000000", 
                     "Destination" : "rJZdUusLDtY9NEsGea7ijqhVrXv98rYBYN", 
                     "Account" : "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh"}'

        Loading: "/Users/redacted/rippled/rippled/rippled.cfg"
        Connecting to 127.0.0.1:5005
        {
           "result" : {
              "engine_result" : "tesSUCCESS",
              "engine_result_code" : 0,
              "engine_result_message" : "The transaction was applied. Only final in a validated ledger.",
              "status" : "success",
              "tx_blob" : "1200002280000000240000000161400000003B9ACA0068400000000000000A73210330E7FC9D56BB25D6893BA3F317AE5BCF33B3291BD63DB32654A313222F7FD02074473045022100D6744E1234FCDF3C022F5F910B8609D39D745379D68D4C27AF4F077D361377A102202AF52E5C762F6E507E7CEC3E947D24638E01628CFE2CD70279176CC87309F3E78114B5F762798A53D543A014CAF8B297CFF8F2F937E88314C0A5ABEF242802EFED4B041E8F2D4A8CC86AE3D1",
              "tx_json" : {
                 "Account" : "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh",
                 "Amount" : "1000000000",
                 "Destination" : "rJZdUusLDtY9NEsGea7ijqhVrXv98rYBYN",
                 "Fee" : "10",
                 "Flags" : 2147483648,
                 "Sequence" : 1,
                 "SigningPubKey" : "0330E7FC9D56BB25D6893BA3F317AE5BCF33B3291BD63DB32654A313222F7FD020",
                 "TransactionType" : "Payment",
                 "TxnSignature" : "3045022100D6744E1234FCDF3C022F5F910B8609D39D745379D68D4C27AF4F077D361377A102202AF52E5C762F6E507E7CEC3E947D24638E01628CFE2CD70279176CC87309F3E7",
                 "hash" : "4E15DC0DA7C7DD057D4BF6EE2D8B104644F7E5F93B0740523313108F2C58C782"
              }
           }
        }

        ➜  rippled git:(develop) ✗ build/clang.debug/rippled ledger_accept                                                                                       
        Loading: "/Users/redacted/rippled/rippled/rippled.cfg"
        Connecting to 127.0.0.1:5005
        {
           "result" : {
              "ledger_current_index" : 4,
              "status" : "success"
           }
        }


        ➜  rippled git:(develop) ✗ build/clang.debug/rippled submit 120000228000000024000000016140000000000003E868400000000000000A7321EDD3993CDC6647896C455F136648B7750723B011475547AF60691AA3D7438E021D7440C3646313B08EED6AF4392261A31B961F10C66CB733DB7F6CD9EAB079857834C8B0334270A2C037E63CDCCC1932E0832882B7B7066ECD2FAEDEB4A83DF8AE63038114C0A5ABEF242802EFED4B041E8F2D4A8CC86AE3D18314B5F762798A53D543A014CAF8B297CFF8F2F937E8
        Loading: "/Users/redacted/rippled/rippled/rippled.cfg"
        Connecting to 127.0.0.1:5005
        {
           "result" : {
              "engine_result" : "tesSUCCESS",
              "engine_result_code" : 0,
              "engine_result_message" : "The transaction was applied. Only final in a validated ledger.",
              "status" : "success",
              "tx_blob" : "120000228000000024000000016140000000000003E868400000000000000A7321EDD3993CDC6647896C455F136648B7750723B011475547AF60691AA3D7438E021D7440C3646313B08EED6AF4392261A31B961F10C66CB733DB7F6CD9EAB079857834C8B0334270A2C037E63CDCCC1932E0832882B7B7066ECD2FAEDEB4A83DF8AE63038114C0A5ABEF242802EFED4B041E8F2D4A8CC86AE3D18314B5F762798A53D543A014CAF8B297CFF8F2F937E8",
              "tx_json" : {
                 "Account" : "rJZdUusLDtY9NEsGea7ijqhVrXv98rYBYN",
                 "Amount" : "1000",
                 "Destination" : "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh",
                 "Fee" : "10",
                 "Flags" : 2147483648,
                 "Sequence" : 1,
                 "SigningPubKey" : "EDD3993CDC6647896C455F136648B7750723B011475547AF60691AA3D7438E021D",
                 "TransactionType" : "Payment",
                 "TxnSignature" : "C3646313B08EED6AF4392261A31B961F10C66CB733DB7F6CD9EAB079857834C8B0334270A2C037E63CDCCC1932E0832882B7B7066ECD2FAEDEB4A83DF8AE6303",
                 "hash" : "A8A9C869671D35A18DFB69AFB7741062DF43F73C8A5942AD94EE58ED31477AC6"
              }
           }
        }


    */
    }
}


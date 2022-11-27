

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/offerCancel.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUOfferCreate
    {
        [TestMethod]
        public async Task TestVerify_Valid_OfferCreate1()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r3rhWeE31Jt5sWmi4QiGLMZnY3ENgqw96W" },
                {"Fee", "10"},
                {"Flags", 0u},
                {"LastLedgerSequence", 65453019u},
                {"Sequence", 40949322u},
                {"SigningPubKey", "03C48299E57F5AE7C2BE1391B581D313F1967EA2301628C07AC412092FDC15BA22"},
                {"Expiration", 10u},
                {"OfferSequence", 12u},
                {"TakerGets", new Dictionary<string,dynamic>()
                {
                    {"currency", "DSH"},
                    {"issuer", "rcXY84C4g14iFp6taFXjjQGVeHqSCh9RX"},
                    {"value", "43.11584856965009"},
                }},
                {"TakerPays", "12928290425"},
                {"TransactionType", "OfferCreate"},
                {"TxnSignature", "3045022100D874CDDD6BB24ED66E83B1D3574D3ECAC753A78F26DB7EBA89EAB8E7D72B95F802207C8CCD6CEA64E4AE2014E59EE9654E02CA8F03FE7FCE0539E958EAE182234D91"},

            };
            await Validation.ValidateOfferCreate(tx);
            await Validation.Validate(tx);
        }
        [TestMethod]
        public async Task TestVerify_Valid_OfferCreate2()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r3rhWeE31Jt5sWmi4QiGLMZnY3ENgqw96W" },
                {"Fee", "10"},
                {"Flags", 0u},
                {"LastLedgerSequence", 65453019u},
                {"Sequence", 40949322u},
                {"SigningPubKey", "03C48299E57F5AE7C2BE1391B581D313F1967EA2301628C07AC412092FDC15BA22"},
                {"TakerPays", new Dictionary<string,dynamic>()
                {
                    {"currency", "DSH"},
                    {"issuer", "rcXY84C4g14iFp6taFXjjQGVeHqSCh9RX"},
                    {"value", "43.11584856965009"},
                }},
                {"TakerGets", "12928290425"},
                {"TransactionType", "OfferCreate"},
                {"TxnSignature", "3045022100D874CDDD6BB24ED66E83B1D3574D3ECAC753A78F26DB7EBA89EAB8E7D72B95F802207C8CCD6CEA64E4AE2014E59EE9654E02CA8F03FE7FCE0539E958EAE182234D91"},

            };
            await Validation.ValidateOfferCreate(tx);
            await Validation.Validate(tx);
        }
        [TestMethod]
        public async Task TestVerify_Valid_OfferCreate3()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r3rhWeE31Jt5sWmi4QiGLMZnY3ENgqw96W" },
                {"Fee", "10"},
                {"Flags", 0u},
                {"LastLedgerSequence", 65453019u},
                {"Sequence", 40949322u},
                {"SigningPubKey", "03C48299E57F5AE7C2BE1391B581D313F1967EA2301628C07AC412092FDC15BA22"},
                {"TakerGets", new Dictionary<string,dynamic>()
                {
                    {"currency", "DSH"},
                    {"issuer", "rcXY84C4g14iFp6taFXjjQGVeHqSCh9RX"},
                    {"value", "43.11584856965009"},
                }},
                {"TakerPays", new Dictionary<string,dynamic>()
                {
                    {"currency", "DSH"},
                    {"issuer", "rcXY84C4g14iFp6taFXjjQGVeHqSCh9RX"},
                    {"value", "43.11584856965009"},
                }},
                {"TransactionType", "OfferCreate"},
                {"TxnSignature", "3045022100D874CDDD6BB24ED66E83B1D3574D3ECAC753A78F26DB7EBA89EAB8E7D72B95F802207C8CCD6CEA64E4AE2014E59EE9654E02CA8F03FE7FCE0539E958EAE182234D91"},

            };
            await Validation.ValidateOfferCreate(tx);
            await Validation.Validate(tx);
        }
        [TestMethod]
        public async Task TestVerify_InValid_Expiration()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r3rhWeE31Jt5sWmi4QiGLMZnY3ENgqw96W" },
                {"Fee", "10"},
                {"Flags", 0u},
                {"LastLedgerSequence", 65453019u},
                {"Sequence", 40949322u},
                {"SigningPubKey", "03C48299E57F5AE7C2BE1391B581D313F1967EA2301628C07AC412092FDC15BA22"},
                {"Expiration", "11"},
                {"OfferSequence", 12u},
                {"TakerGets", new Dictionary<string,dynamic>()
                {
                    {"currency", "DSH"},
                    {"issuer", "rcXY84C4g14iFp6taFXjjQGVeHqSCh9RX"},
                    {"value", "43.11584856965009"},
                }},
                {"TakerPays", "12928290425"},
                {"TransactionType", "OfferCreate"},
                {"TxnSignature", "3045022100D874CDDD6BB24ED66E83B1D3574D3ECAC753A78F26DB7EBA89EAB8E7D72B95F802207C8CCD6CEA64E4AE2014E59EE9654E02CA8F03FE7FCE0539E958EAE182234D91"},

            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateOfferCreate(tx), "OfferCreate: invalid Expiration");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(tx), "OfferCreate: invalid Expiration");
        }
        [TestMethod]
        public async Task TestVerify_InValid_OfferSequence()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r3rhWeE31Jt5sWmi4QiGLMZnY3ENgqw96W" },
                {"Fee", "10"},
                {"Flags", 0u},
                {"LastLedgerSequence", 65453019u},
                {"Sequence", 40949322u},
                {"SigningPubKey", "03C48299E57F5AE7C2BE1391B581D313F1967EA2301628C07AC412092FDC15BA22"},
                {"Expiration", 10u},
                {"OfferSequence", "11"},
                {"TakerGets", new Dictionary<string,dynamic>()
                {
                    {"currency", "DSH"},
                    {"issuer", "rcXY84C4g14iFp6taFXjjQGVeHqSCh9RX"},
                    {"value", "43.11584856965009"},
                }},
                {"TakerPays", "12928290425"},
                {"TransactionType", "OfferCreate"},
                {"TxnSignature", "3045022100D874CDDD6BB24ED66E83B1D3574D3ECAC753A78F26DB7EBA89EAB8E7D72B95F802207C8CCD6CEA64E4AE2014E59EE9654E02CA8F03FE7FCE0539E958EAE182234D91"},

            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateOfferCreate(tx), "OfferCreate: invalid OfferSequence");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(tx), "OfferCreate: invalid OfferSequence");
        }
        [TestMethod]
        public async Task TestVerify_InValid_TakerPays()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r3rhWeE31Jt5sWmi4QiGLMZnY3ENgqw96W" },
                {"Fee", "10"},
                {"Flags", 0u},
                {"LastLedgerSequence", 65453019u},
                {"Sequence", 40949322u},
                {"SigningPubKey", "03C48299E57F5AE7C2BE1391B581D313F1967EA2301628C07AC412092FDC15BA22"},
                {"Expiration", 10u},
                {"OfferSequence", 12u},
                {"TakerGets", new Dictionary<string,dynamic>()
                {
                    {"currency", "DSH"},
                    {"issuer", "rcXY84C4g14iFp6taFXjjQGVeHqSCh9RX"},
                    {"value", "43.11584856965009"},
                }},
                {"TakerPays", 10},
                {"TransactionType", "OfferCreate"},
                {"TxnSignature", "3045022100D874CDDD6BB24ED66E83B1D3574D3ECAC753A78F26DB7EBA89EAB8E7D72B95F802207C8CCD6CEA64E4AE2014E59EE9654E02CA8F03FE7FCE0539E958EAE182234D91"},
            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateOfferCreate(tx), "OfferCreate: invalid TakerPays");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(tx), "OfferCreate: invalid TakerPays");
        }
        [TestMethod]
        public async Task TestVerify_InValid_TakerGets()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r3rhWeE31Jt5sWmi4QiGLMZnY3ENgqw96W" },
                {"Fee", "10"},
                {"Flags", 0u},
                {"LastLedgerSequence", 65453019u},
                {"Sequence", 40949322u},
                {"SigningPubKey", "03C48299E57F5AE7C2BE1391B581D313F1967EA2301628C07AC412092FDC15BA22"},
                {"Expiration", 10u},
                {"OfferSequence", 12u},
                {"TakerGets", 11},
                {"TakerPays",new Dictionary<string,dynamic>()
                {
                    {"currency", "DSH"},
                    {"issuer", "rcXY84C4g14iFp6taFXjjQGVeHqSCh9RX"},
                    {"value", "43.11584856965009"},
                }},
                {"TransactionType", "OfferCreate"},
                {"TxnSignature", "3045022100D874CDDD6BB24ED66E83B1D3574D3ECAC753A78F26DB7EBA89EAB8E7D72B95F802207C8CCD6CEA64E4AE2014E59EE9654E02CA8F03FE7FCE0539E958EAE182234D91"},
            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateOfferCreate(tx), "OfferCreate: invalid TakerGets");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(tx), "OfferCreate: invalid TakerGets");
        }
    }

}


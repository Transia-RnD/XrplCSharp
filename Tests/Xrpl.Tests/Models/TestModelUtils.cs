

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/utils.ts

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Threading.Tasks;

using Xrpl.Models.Transaction;
using Xrpl.Models.Utils;

namespace XrplTests.Xrpl.Models
{
    public class TestModelUtils
    {
        public TestModelUtils()
        {
        }    /// <summary>
             /// ModelUtils Transaction Verification Testing.<br/>
             /// Providing runtime verification testing for each specific transaction type.
             /// </summary>
        [TestClass]
        public class TestUModelUtils
        {

            [TestMethod]
            public async Task TestVerifyValid_isFlagEnabled()
            {
                uint flags = 0x00000000;
                uint flag1 = 0x00010000;
                uint flag2 = 0x00020000;

                //verifies a flag is enabled
                flags |= flag1 | flag2;

                Assert.IsTrue(Index.IsFlagEnabled(flags, flag1));
                //verifies a flag is not enabled
                flags = 0x00000000;
                flags |= flag2;
                Assert.IsFalse(Index.IsFlagEnabled(flags, flag1));
            }
            [TestMethod]
            public async Task TestVerifyValid_setTransactionFlagsToNumber()
            {
                var offerCrete = new Dictionary<string, dynamic>
                {
                    {"Account", "r3rhWeE31Jt5sWmi4QiGLMZnY3ENgqw96W"},
                    {"TakerGets",new Dictionary<string,dynamic>()
                    {
                        {"currency","DSH"},
                        {"issuer","rcXY84C4g14iFp6taFXjjQGVeHqSCh9RX"},
                        {"value","43.11584856965009"}
                    }},
                    {"TakerPays", "12928290425"},
                    {"Fee", "10"},
                    {"TransactionType", "OfferCreate"},
                    {"TxnSignature", "3045022100D874CDDD6BB24ED66E83B1D3574D3ECAC753A78F26DB7EBA89EAB8E7D72B95F802207C8CCD6CEA64E4AE2014E59EE9654E02CA8F03FE7FCE0539E958EAE182234D91"},
                    {"Flags", new Dictionary<string,dynamic>()
                    {
                        {"tfPassive",true},
                        {"tfImmediateOrCancel",false},
                        {"tfFillOrKill",true},
                        {"tfSell",true},
                    }}
                };
            }
        }

    }
}


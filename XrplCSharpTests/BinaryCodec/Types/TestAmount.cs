using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodecLib.Types;
using Xrpl.BinaryCodecLib;
using Xrpl.AddressCodecLib;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

// https://github.com/XRPLF/xrpl-py/blob/master/tests/unit/core/binarycodec/types/test_amount.py

namespace XrplTests.BinaryCodecLib.Types
{
    [TestClass]
    public class TestAmount
    {
        static JObject IouCases;
        static Dictionary<string, string> XrpCases = new Dictionary<string, string>
        {
            { "100", "4000000000000064" },
            { "100000000000000000", "416345785D8A0000" },
        };
        private static void GetTestsJson()
        {
            string jsonString = "[[{\"value\":\"0\",\"currency\":\"USD\",\"issuer\":\"rDgZZ3wyprx4ZqrGQUkquE9Fs2Xs8XBcdw\",},\"80000000000000000000000000000000000000005553440000\"\"0000008B1CE810C13D6F337DAC85863B3D70265A24DF44\",],[{\"value\":\"1\",\"currency\":\"USD\",\"issuer\":\"rDgZZ3wyprx4ZqrGQUkquE9Fs2Xs8XBcdw\",},\"D4838D7EA4C680000000000000000000000000005553440000\"\"0000008B1CE810C13D6F337DAC85863B3D70265A24DF44\",],[{\"value\":\"2\",\"currency\":\"USD\",\"issuer\":\"rDgZZ3wyprx4ZqrGQUkquE9Fs2Xs8XBcdw\",},\"D4871AFD498D00000000000000000000000000005553440000\"\"0000008B1CE810C13D6F337DAC85863B3D70265A24DF44\",],[{\"value\":\"-2\",\"currency\":\"USD\",\"issuer\":\"rDgZZ3wyprx4ZqrGQUkquE9Fs2Xs8XBcdw\",},\"94871AFD498D00000000000000000000000000005553440000\"\"0000008B1CE810C13D6F337DAC85863B3D70265A24DF44\",],[{\"value\":\"2.1\",\"currency\":\"USD\",\"issuer\":\"rDgZZ3wyprx4ZqrGQUkquE9Fs2Xs8XBcdw\",},\"D48775F05A0740000000000000000000000000005553440000\"\"0000008B1CE810C13D6F337DAC85863B3D70265A24DF44\",],[{\"currency\":\"XRP\",\"value\":\"2.1\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrrhoLvTp\",},\"D48775F05A07400000000000000000000000000000000000\"\"000000000000000000000000000000000000000000000000\",],[{\"currency\":\"USD\",\"value\":\"1111111111111111\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",},\"D843F28CB71571C700000000000000000000000055534400\"\"000000000000000000000000000000000000000000000001\",],]";
            IouCases = (JObject)JToken.Parse(jsonString);
        }

        [TestMethod]
        public void TestAssertXrpIsValidPasses()
        {
            string validZero = "0";
            string validAmount = "1000";

            Amount.VerifyXrpValue(validZero);
            Amount.VerifyXrpValue(validAmount);
        }

        [TestMethod]
        public void TestAssertXrpIsValidRaises()
        {
            string validLarge = "01e20";
            string validSmall = "1e-7";
            string valueDecimal = "1.234";

            Assert.ThrowsException<TypeInitializationException>(() => Amount.VerifyXrpValue(validLarge));
            Assert.ThrowsException<TypeInitializationException>(() => Amount.VerifyXrpValue(validSmall));
            Assert.ThrowsException<BinaryCodecException>(() => Amount.VerifyXrpValue(valueDecimal));
        }

        [TestMethod]
        public void TestIOUIsValid()
        {
            string[] variables = {
                "0",
                "0.0",
                "1",
                "1.1111",
                "-1",
                "-1.1",
                "1111111111111111.0",
                "-1111111111111111.0",
                "0.00000000001",
                "0.00000000001",
                "-0.00000000001",
                "1.111111111111111e-3",
                "-1.111111111111111e-3",
                "2E+2"
            };
            for (var i = 0; i < variables.Length; i++)
            {
                Amount.VerifyIouValue(variables[i]);
            }
        }

        [TestMethod]
        public void TestFromValueIssuedCurrency()
        {
            GetTestsJson();
            for (var i = 0; i < IouCases.Count; i++)
            {
                dynamic json = IouCases[i][0];
                string binary = (string)IouCases[i][1];
                Amount amount = Amount.FromValue(json);
                Assert.AreEqual(amount, binary);
            }
        }

        [TestMethod]
        public void TestFromValueXrp()
        {
            foreach (var item in XrpCases)
            {
                Amount amount = Amount.FromValue(item.Key);
                //Debug.WriteLine(amount.ToBytes());
                Assert.AreEqual(amount.ToString(), item.Value);
            }
        }
    }
}


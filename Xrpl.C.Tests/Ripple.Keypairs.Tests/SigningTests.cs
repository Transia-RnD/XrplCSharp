using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Utilities.Encoders;
using System.Text;
using System.Diagnostics;
using Currency = Xrpl.Client.Model.Currency;
using System.Collections;
using System;
using Ripple.Binary.Codec.Util;

namespace Ripple.Keypairs.Tests
{
    using Seed = Seed;

    [TestClass]
    public class SigningTests
    {
        readonly byte[] _message = { 0xb, 0xe, 0xe, 0xf };
        readonly Currency Amount = new Currency { ValueAsXrp = 1 };
        public byte[] CHANNEL_SIG_BYTES = { 190, 50, 164, 198, 141, 107, 164, 120, 114, 100, 238, 231, 96, 222, 255, 180, 240, 90, 218, 201, 153, 186, 243, 172, 99, 244, 233, 245, 66, 223, 7, 250, 231, 131, 253, 112, 112, 113, 135, 151, 116, 160, 34, 232, 122, 106, 136, 73, 182, 102, 182, 221, 170, 34, 54, 23, 133, 34, 77, 54, 183, 203, 196, 6 };
        public byte[] CHANNEL_DATA_BYTES = { 67, 76, 77, 0, 147, 30, 107, 108, 39, 141, 183, 172, 10, 97, 205, 146, 174, 165, 55, 62, 141, 245, 158, 88, 54, 230, 208, 209, 236, 170, 83, 211, 140, 60, 78, 30, 0, 0, 0, 0, 0, 15, 66, 64 };

        public static byte[] GetBytes(byte[] data, int start, int end)
        {
            byte[] copy = new byte[end - start];
            System.Buffer.BlockCopy(data, start, copy, 0, copy.Length);
            return copy;
        }

        [TestMethod]
        public void DecodeChannelHash()
        {
            byte[] HASH_CHANNEL_SIGN = { 0x43, 0x4C, 0x4D, 0x00 };
            byte[] newBytes = GetBytes(CHANNEL_DATA_BYTES, 0, 4);
            Assert.AreEqual(HASH_CHANNEL_SIGN.Length, newBytes.Length);
        }

        [TestMethod]
        public void DecodeChannelHex()
        {
            byte[] HEX_CHANNEL_SIGN = { 147, 30, 107, 108, 39, 141, 183, 172, 10, 97, 205, 146, 174, 165, 55, 62, 141, 245, 158, 88, 54, 230, 208, 209, 236, 170, 83, 211, 140, 60, 78, 30 };
            byte[] newBytes = GetBytes(CHANNEL_DATA_BYTES, 4, 36);
            Assert.AreEqual(HEX_CHANNEL_SIGN.Length, newBytes.Length);
        }

        [TestMethod]
        public void DecodeChannelAmount()
        {
            byte[] AMOUNT_CHANNEL_SIGN = { 0, 0, 0, 0, 0, 15, 66, 64 };
            byte[] newBytes = GetBytes(CHANNEL_DATA_BYTES, 36, CHANNEL_DATA_BYTES.Length);
            Assert.AreEqual(AMOUNT_CHANNEL_SIGN.Length, newBytes.Length);
        }

        [TestMethod]
        public void ConvertChannelAmount()
        {
            byte[] AMOUNT_CHANNEL_SIGN = { 0, 0, 0, 0, 0, 15, 66, 64 };
            Array.Reverse(AMOUNT_CHANNEL_SIGN);
            long value = BitConverter.ToInt64(AMOUNT_CHANNEL_SIGN, 0);
            Assert.AreEqual(value, 1000000);
        }

        [TestMethod]
        public void ChannelEncodeAndSign()
        {
            var keypair = Seed.FromPassPhrase("niq").SetEd25519().KeyPair();
            var channelEncoded = ChannelUtils.EncodeChannel(
                "931E6B6C278DB7AC0A61CD92AEA5373E8DF59E5836E6D0D1ECAA53D38C3C4E1E",
                (long)Amount.ValueAsNumber
            );
            Assert.IsNotNull(channelEncoded);
            Assert.AreEqual(channelEncoded.Length, CHANNEL_DATA_BYTES.Length);
            var sig = keypair.Sign(channelEncoded);
            Assert.IsTrue(keypair.Verify(channelEncoded, sig));
        }

        [TestMethod]
        public void K256SanityTest()
        {
            var keypair = Seed.FromPassPhrase("niq").KeyPair();
            var sig = keypair.Sign(_message);
            Assert.IsTrue(keypair.Verify(_message, sig));
        }

        [TestMethod]
        public void Ed25519SanityTest()
        {
            var keypair = Seed.FromPassPhrase("niq").SetEd25519().KeyPair();
            var sig = keypair.Sign(_message);
            Assert.IsTrue(keypair.Verify(_message, sig));
        }

        public string ToHex(byte[] val)
        {
            return Hex.ToHexString(val).ToUpper();
        }

        [TestMethod]
        public void Rfc6979DeterminismTest()
        {
            var keypair = Seed.FromPassPhrase("niq").KeyPair();

            var fixtures = new string[]
            {
                "30440220312B2E0894B81A2E070ACE566C5DFC70CDD18E67D44E2CFEF2EB5495F7DE2DAC02205E155C0019502948C265209DFDD7D84C4A05BD2C38CEE6ECD7C33E9C9B12BEC2",
                "304402202A5860A12C15EBB8E91AA83F8E19D85D4AC05B272FC0C4083519339A7A76F2B802200852F9889E1284CF407DC7F73D646E62044C5AB432EAEF3FFF3F6F8EE9A0F24C",
                "3045022100B1658C88D1860D9F8BEB25B79B3E5137BBC2C382D08FE7A068FFC6AB8978C8040220644F64B97EA144EE7D5CCB71C2372DD730FA0A659E4C18241A80D6C915350263",
                "3045022100F3E541330FF79FFC42EB0491EDE1E47106D94ECFE3CDB2D9DD3BC0E8861F6D45022013F62942DD626D6C9731E317F372EC5C1F72885C4727FDBEE9D9321BC530D7B2",
                "3045022100998ABE378F4119D8BEE9843482C09F0D5CE5C6012921548182454C610C57A269022036BD8EB71235C4B2C67339DE6A59746B1F7E5975987B7AB99B313D124A69BB9F",
                "304402200754DE2379B3333B0BC29DB74F5E1C2F4A65FF090E2B5A52D9691A2983CE73E102204CD07D7E8A02374CA00DEDEA4B17223AC782D424EE43BC9C4355CC2D45741949",
                "3045022100D96FFA0F7D347FE655067CB985B4C13190CE66ECCA73AA305788C673F7640B7502204E6E961EE5C519288D5D3FEE637A914138E5DEBF15182D47C92AB8C301D5958A",
                "304402202FEF8C6ECB139DD942F193E75778BAD324108DA23ECA4B47698962ECFD79005302202240F584E4D7E53BD1247033429A627F18DE585ED02D70A4659381B72C4050FB",
                "30440220304E276143E54CA1C8C070DF9D285BDD1DC3CDDEEEB24E9024AAE5AF373DDBC70220604F078C46D499E6193130AA8A89C2E91F3632F08E1D387114047DEBC3BE6C18",
                "3044022068080CEBD70C2FFABDB6697D38744674336B2D6441ADD825BDE6186628148502022021AA14A3CF55231404305B420C129FD97DD5C6096E1F0046E03FCCA056D1D8E2",
                "304402205A7589B193B3F1EAAD7C9B25B29B6B586FD358FE0C44F23D43F27C55284A5AB702206AD9879DB089D33C8E10A6CB889760A725DDD963DD412072A19B7F5BE119B52D",
                "3044022001D45AF8B61EA8F782238A2C330475E7BA83353146B67BAA5BF2CD91F366A8D0022010B5065CDC83A015B508C72E4E1578BBC58964510525DBC4960F02A0D1A29A4E"
            };

            for (var i = 0; i < fixtures.Length; i++)
            {
                var messageBytes = new[] { (byte)i };
                Assert.AreEqual(fixtures[i], ToHex(keypair.Sign(messageBytes)));
            }
        }

        [TestMethod]
        public void Ed25519DeterminismTest()
        {
            var keypair = Seed.FromPassPhrase("niq")
                              .SetEd25519()
                              .KeyPair();

            var messageBytes = Hex.Decode(
                  "535458001200002280000000240000" +
                  "00016140000000000003E868400000" +
                  "000000000A7321EDD3993CDC664789" +
                  "6C455F136648B7750723B011475547" +
                  "AF60691AA3D7438E021D8114C0A5AB" +
                  "EF242802EFED4B041E8F2D4A8CC86A" +
                  "E3D18314B5F762798A53D543A014CA" +
                  "F8B297CFF8F2F937E8");

            string expectedSig =
                  "C3646313B08EED6AF4392261A31B961F" +
                  "10C66CB733DB7F6CD9EAB079857834C8" +
                  "B0334270A2C037E63CDCCC1932E08328" +
                  "82B7B7066ECD2FAEDEB4A83DF8AE6303";

            Assert.AreEqual(expectedSig, ToHex(keypair.Sign(messageBytes)));
        }
    }
}
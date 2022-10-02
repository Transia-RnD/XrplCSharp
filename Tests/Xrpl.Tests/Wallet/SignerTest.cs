using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.BinaryCodec;
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/wallet/signer.ts

namespace XrplTests.Xrpl.WalletLib
{

    [TestClass]
    public class TestUSigner
    {
        //static Dictionary<string, dynamic> multisignTxToCombine1 = new Dictionary<string, dynamic>
        //{
        //    { "Account", "rEuLyBCvcw4CFmzv8RepSiAoNgF8tTGJQC" },
        //    { "Fee", "30000" },
        //    { "Flags", 262144 },
        //    { "LimitAmount", new Dictionary<string, dynamic> {
        //        { "currency", "USD" },
        //        { "issuer", "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh" },
        //        { "value", "100" }
        //    } },
        //    { "Sequence", 2 },
        //    { "Signers", new Dictionary<string, dynamic> {
        //        { "Signer", new Dictionary<string, dynamic> {
        //            { "Account", "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW" },
        //            { "SigningPubKey", "02B3EC4E5DD96029A647CFA20DA07FE1F85296505552CCAC114087E66B46BD77DF" },
        //            { "TxnSignature", "30450221009C195DBBF7967E223D8626CA19CF02073667F2B22E206727BFE848FF42BEAC8A022048C323B0BED19A988BDBEFA974B6DE8AA9DCAE250AA82BBD1221787032A864E5" },
        //        } },
        //    } },
        //    { "SigningPubKey", "" },
        //    { "TransactionType", "TrustSet" },
        //};

        //static Dictionary<string, dynamic> multisignTxToCombine2 = new Dictionary<string, dynamic>
        //{
        //    { "Account", "rEuLyBCvcw4CFmzv8RepSiAoNgF8tTGJQC" },
        //    { "Fee", "30000" },
        //    { "Flags", 262144 },
        //    { "LimitAmount", new Dictionary<string, dynamic> {
        //        { "currency", "USD" },
        //        { "issuer", "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh" },
        //        { "value", "100" }
        //    } },
        //    { "Sequence", 2 },
        //    { "Signers", new Dictionary<string, dynamic> {
        //        { "Signer", new Dictionary<string, dynamic> {
        //            { "Account", "rJvuSQhQR37czfxRou4vNWaM97uEhT4ShE" },
        //            { "SigningPubKey", "02B78EEA571B2633180834CC6E7B4ED84FBF6811D12ECB59410E0C92D13B7726F5" },
        //            { "TxnSignature", "304502210098009CEFA61EE9843BB7FC29B78CFFAACF28352A4A7CF3AAE79EF12D79BA50910220684F116266E5E4519A7A33F7421631EB8494082BE51A8B03FECCB3E59F77154A" },
        //        } },
        //    } },
        //    { "SigningPubKey", "" },
        //    { "TransactionType", "TrustSet" },
        //};

        //static Dictionary<string, dynamic> multisignJson = new Dictionary<string, dynamic>
        //{
        //    { "Account", "rEuLyBCvcw4CFmzv8RepSiAoNgF8tTGJQC" },
        //    { "Fee", "30000" },
        //    { "Flags", 262144 },
        //    { "LimitAmount", new Dictionary<string, dynamic> {
        //        { "currency", "USD" },
        //        { "issuer", "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh" },
        //        { "value", "100" }
        //    } },
        //    { "Sequence", 2 },
        //    { "Signers", new Dictionary<string, dynamic> {
        //        { "Signer", new Dictionary<string, dynamic> {
        //            { "Account", "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW" },
        //            { "SigningPubKey", "02B3EC4E5DD96029A647CFA20DA07FE1F85296505552CCAC114087E66B46BD77DF" },
        //            { "TxnSignature", "30450221009C195DBBF7967E223D8626CA19CF02073667F2B22E206727BFE848FF42BEAC8A022048C323B0BED19A988BDBEFA974B6DE8AA9DCAE250AA82BBD1221787032A864E5" },
        //        } },
        //        { "Signer", new Dictionary<string, dynamic> {
        //            { "Account", "rJvuSQhQR37czfxRou4vNWaM97uEhT4ShE" },
        //            { "SigningPubKey", "02B78EEA571B2633180834CC6E7B4ED84FBF6811D12ECB59410E0C92D13B7726F5" },
        //            { "TxnSignature", "304502210098009CEFA61EE9843BB7FC29B78CFFAACF28352A4A7CF3AAE79EF12D79BA50910220684F116266E5E4519A7A33F7421631EB8494082BE51A8B03FECCB3E59F77154A" },
        //        } },
        //    } },
        //    { "SigningPubKey", "" },
        //    { "TransactionType", "TrustSet" },
        //};
        //static string expectedMultisign = BinaryCodec.Encode(multisignJson);

        static string publicKey = "030E58CDD076E798C84755590AAF6237CA8FAE821070A59F648B517A30DC6F589D";
        static string privateKey = "00141BA006D3363D2FB2785E8DF4E44D3A49908780CB4FB51F6D217C08C021429F";
        static string address = "rhvh5SrgBL5V8oeV9EpDuVszeJSSCEkbPc";
        XrplWallet verifyWallet = new XrplWallet(publicKey, privateKey, address);

        static Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
        {
            { "TransactionType", "Payment" },
            { "Account", address },
            { "Destination", "rQ3PTWGLCbPz8ZCicV5tCX3xuymojTng5r" },
            { "Amount", "20000000" },
            { "Sequence", 1 },
            { "Fee", "12" },
            { "SigningPubKey", publicKey },
        };

        //[TestMethod]
        //public void TestVerifyMultisignTransactions()
        //{
        //    Dictionary<string, dynamic>[] transactions = { multisignTxToCombine1, multisignTxToCombine2 };
        //    Assert.AreEqual(
        //        Signer.Multisign(transactions),
        //        expectedMultisign
        //    );
        //}

        //[TestMethod]
        //public void TestVerifyMultisignXAddress()
        //{
        //    multisignTxToCombine1["Account"] = "XVJfK5FpouB7gtk3kaZHqbgV4Bswir4ccz3rsJw9oMf71tc";
        //    multisignTxToCombine2["Account"] = "XVJfK5FpouB7gtk3kaZHqbgV4Bswir4ccz3rsJw9oMf71tc";
        //    Dictionary<string, dynamic>[] transactions = { multisignTxToCombine1, multisignTxToCombine2 };
        //    Assert.AreEqual(
        //        Signer.Multisign(transactions),
        //        expectedMultisign
        //    );
        //}

        //[TestMethod]
        //public void TestInvalidMultisignTransactions()
        //{
        //    Dictionary<string, dynamic>[] transactions = { };
        //    // THROW
        //}

        //[TestMethod]
        //public void VerifyMultisignTxBlobs()
        //{
        //    Dictionary<string, dynamic>[] transactions = { multisignTxToCombine1, multisignTxToCombine2 };
        //    string[] encodedTransactions = transactions.Map((t) => BinaryCodec.Encode(t));
        //    Assert.AreEqual(
        //        Signer.Multisign(transactions),
        //        expectedMultisign
        //    );
        //}

        [TestMethod]
        public void TestAuthorizeChannelSECP()
        {
            XrplWallet secpWallet = XrplWallet.FromSeed("snGHNrPbHrdUcszeuDEigMdC1Lyyd");
            string channelId = "5DB01B7FFED6B67E6B0414DED11E051D2EE2B7619CE0EAA6286D67A3A4D5BDB3";
            string amount = "1000000";
            Assert.AreEqual(
                Signer.AuthorizeChannel(secpWallet, channelId, amount),
                "304402204E7052F33DDAFAAA55C9F5B132A5E50EE95B2CF68C0902F61DFE77299BC893740220353640B951DCD24371C16868B3F91B78D38B6F3FD1E826413CDF891FA8250AAC"
            );
        }

        [TestMethod]
        public void TestAuthorizeChannelED()
        {
            XrplWallet edWallet = XrplWallet.FromSeed("sEdSuqBPSQaood2DmNYVkwWTn1oQTj2");
            string channelId = "5DB01B7FFED6B67E6B0414DED11E051D2EE2B7619CE0EAA6286D67A3A4D5BDB3";
            string amount = "1000000";
            Assert.AreEqual(
                Signer.AuthorizeChannel(edWallet, channelId, amount),
                "7E1C217A3E4B3C107B7A356E665088B4FBA6464C48C58267BEF64975E3375EA338AE22E6714E3F5E734AE33E6B97AAD59058E1E196C1F92346FC1498D0674404"
            );
        }

        [TestMethod]
        public void TestVerifySignatureBlob()
        {
            SignatureResult signedTx = verifyWallet.Sign(tx);
            Assert.IsTrue(Signer.VerifySignature(signedTx.TxBlob));
        }

        [TestMethod]
        public void TestVerifySignatureJson()
        {
            SignatureResult signedTx = verifyWallet.Sign(tx);
            Debug.WriteLine(signedTx.TxBlob);
            Assert.IsTrue(Signer.VerifySignature(XrplBinaryCodec.Decode(signedTx.TxBlob).ToObject<Dictionary<string, dynamic>>()));
        }

        [TestMethod]
        public void TestVerifyInvalidSigningKey()
        {
            SignatureResult signedTx = verifyWallet.Sign(tx);
            Dictionary<string, dynamic> decodedTx = XrplBinaryCodec.Decode(signedTx.TxBlob).ToObject<Dictionary<string, dynamic>>();
            decodedTx["SigningPubKey"] = "0330E7FC9D56BB25D6893BA3F317AE5BCF33B3291BD63DB32654A313222F7FD020";
            Assert.IsFalse(Signer.VerifySignature(decodedTx));
        }
    }
}
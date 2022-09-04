using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec;
using Ripple.Binary.Codec.Types;
using Ripple.Keypairs;
using Xrpl.Client.Exceptions;
using Xrpl.XrplWallet;
using static Xrpl.XrplWallet.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/wallet/index.ts

namespace Xrpl.Tests.XrplWallet.Tests
{
    [TestClass]
    public class TestUConstructor
    {
        [TestMethod]
        public void Test()
        {
            string masterAddress = "rUAi7pipxGpYfPNg3LtPcf2ApiS8aw9A93";
            rKeypair regularKeyPair = new rKeypair
            {
                PublicKey = "aBRNH5wUurfhZcoyR6nRwDSa95gMBkovBJ8V4cp1C1pM28H7EPL1",
                PrivateKey = "sh8i92YRnEjJy3fpFkL8txQSCVo79",
            };
            Wallet wallet = new Wallet(
                regularKeyPair.PublicKey,
                regularKeyPair.PrivateKey,
                masterAddress
            );
            Assert.AreEqual(wallet.PublicKey, regularKeyPair.PublicKey);
            Assert.AreEqual(wallet.PrivateKey, regularKeyPair.PrivateKey);
            Assert.AreEqual(wallet.ClassicAddress, masterAddress);
        }
    }

    [TestClass]
    public class TestUGenerate
    {

        static string classicAddressPrefix = "r";
        static string ed25519KeyPrefix = "ED";
        static string secp256k1PrivateKeyPrefix = "00";

        [TestMethod]
        public void TestDefaultAlgorithm()
        {
            Wallet wallet = Wallet.Generate();

            //Assert.IsInstanceOfType(wallet.PublicKey, string);
            //Assert.IsInstanceOfType(wallet.PrivateKey, string);
            //Assert.IsInstanceOfType(wallet.ClassicAddress, string);
            //Assert.IsInstanceOfType(wallet.Seed, string);
            Assert.IsTrue(wallet.PublicKey.StartsWith(ed25519KeyPrefix));
            Assert.IsTrue(wallet.PrivateKey.StartsWith(ed25519KeyPrefix));
            Assert.IsTrue(wallet.ClassicAddress.StartsWith(classicAddressPrefix));
        }

        [TestMethod]
        public void TestSECPAlgorithm()
        {
            Wallet wallet = Wallet.Generate("secp256k1");

            //Assert.IsInstanceOfType(wallet.PublicKey, string);
            //Assert.IsInstanceOfType(wallet.PrivateKey, string);
            //Assert.IsInstanceOfType(wallet.ClassicAddress, string);
            //Assert.IsInstanceOfType(wallet.Seed, string);
            Assert.IsTrue(wallet.PrivateKey.StartsWith(secp256k1PrivateKeyPrefix));
            Assert.IsTrue(wallet.ClassicAddress.StartsWith(classicAddressPrefix));
        }

        [TestMethod]
        public void TestEDAlgorithm()
        {
            Wallet wallet = Wallet.Generate("ed25519");

            //Assert.IsInstanceOfType(wallet.PublicKey, string);
            //Assert.IsInstanceOfType(wallet.PrivateKey, string);
            //Assert.IsInstanceOfType(wallet.ClassicAddress, string);
            //Assert.IsInstanceOfType(wallet.Seed, string);
            Assert.IsTrue(wallet.PublicKey.StartsWith(ed25519KeyPrefix));
            Assert.IsTrue(wallet.PrivateKey.StartsWith(ed25519KeyPrefix));
            Assert.IsTrue(wallet.ClassicAddress.StartsWith(classicAddressPrefix));
        }
    }

    [TestClass]
    public class TestUSeed
    {

        static string seed = "ssL9dv2W5RK8L3tuzQxYY6EaZhSxW";
        static string publicKey = "030E58CDD076E798C84755590AAF6237CA8FAE821070A59F648B517A30DC6F589D";
        static string privateKey = "00141BA006D3363D2FB2785E8DF4E44D3A49908780CB4FB51F6D217C08C021429F";

        [TestMethod]
        public void TestDefaultAlgorithm()
        {
            Wallet wallet = Wallet.FromSeed(seed);

            Assert.AreEqual(wallet.PublicKey, publicKey);
            Assert.AreEqual(wallet.PrivateKey, privateKey);
        }

        [TestMethod]
        public void TestSECPAlgorithm()
        {
            Wallet wallet = Wallet.FromSeed(seed, null, "secp256k1");

            Assert.AreEqual(wallet.PublicKey, publicKey);
            Assert.AreEqual(wallet.PrivateKey, privateKey);
        }

        [TestMethod]
        public void TestEDAlgorithm()
        {
            Wallet wallet = Wallet.FromSeed(seed, null, "ed25519");

            Assert.AreEqual(wallet.PublicKey, publicKey);
            Assert.AreEqual(wallet.PrivateKey, privateKey);
        }

        [TestMethod]
        public void TestRegularKeypair()
        {
            string masterAddress = "rUAi7pipxGpYfPNg3LtPcf2ApiS8aw9A93";
            string seed = "sh8i92YRnEjJy3fpFkL8txQSCVo79";
            rKeypair regularKeyPair = new rKeypair
            {
                PublicKey = "03AEEFE1E8ED4BBC009DE996AC03A8C6B5713B1554794056C66E5B8D1753C7DD0E",
                PrivateKey = "004265A28F3E18340A490421D47B2EB8DBC2C0BF2C24CEFEA971B61CED2CABD233",
            };
            Wallet wallet = Wallet.FromSeed(seed, masterAddress, "ed25519");

            Assert.AreEqual(wallet.PublicKey, regularKeyPair.PublicKey);
            Assert.AreEqual(wallet.PrivateKey, regularKeyPair.PrivateKey);
            Assert.AreEqual(wallet.ClassicAddress, masterAddress);
        }
    }

    [TestClass]
    public class TestUEntropy
    {

        static string publicKey = "0390A196799EE412284A5D80BF78C3E84CBB80E1437A0AECD9ADF94D7FEAAFA284";
        static string privateKey = "002512BBDFDBB77510883B7DCCBEF270B86DEAC8B64AC762873D75A1BEE6298665";

        static string publicKeyED25519 = "ED1A7C082846CFF58FF9A892BA4BA2593151CCF1DBA59F37714CC9ED39824AF85F";
        static string privateKeyED25519 = "ED0B6CBAC838DFE7F47EA1BD0DF00EC282FDF45510C92161072CCFB84035390C4D";

        static byte[] entropy = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        [TestMethod]
        public void TestDefaultAlgorithm()
        {
            Wallet wallet = Wallet.FromEntropy(entropy);

            Assert.AreEqual(wallet.PublicKey, publicKeyED25519);
            Assert.AreEqual(wallet.PrivateKey, privateKeyED25519);
        }

        [TestMethod]
        public void TestSECPAlgorithm()
        {
            Wallet wallet = Wallet.FromEntropy(entropy, "secp256k1");

            Assert.AreEqual(wallet.PublicKey, publicKeyED25519);
            Assert.AreEqual(wallet.PrivateKey, privateKeyED25519);
        }

        [TestMethod]
        public void TestEDAlgorithm()
        {
            Wallet wallet = Wallet.FromEntropy(entropy, "ed25519");

            Assert.AreEqual(wallet.PublicKey, publicKeyED25519);
            Assert.AreEqual(wallet.PrivateKey, privateKeyED25519);
        }

        [TestMethod]
        public void TestRegularKeypair()
        {
            string masterAddress = "rUAi7pipxGpYfPNg3LtPcf2ApiS8aw9A93";
            Wallet wallet = Wallet.FromEntropy(entropy, masterAddress);

            Assert.AreEqual(wallet.PublicKey, publicKeyED25519);
            Assert.AreEqual(wallet.PrivateKey, privateKeyED25519);
            Assert.AreEqual(wallet.ClassicAddress, masterAddress);
        }
    }

    [TestClass]
    public class TestUSign
    {
        static string requestFixture = "{\"hashLedger\":{\"header\":{\"account_hash\":\"D9ABF622DA26EEEE48203085D4BC23B0F77DC6F8724AC33D975DA3CA492D2E44\",\"close_time\":492656470,\"parent_close_time\":492656460,\"close_flags\":0,\"ledger_index\":\"15202439\",\"close_time_human\":\"2015-Aug-12 01:01:10.000000000 UTC\",\"close_time_resolution\":10,\"closed\":true,\"hash\":\"F4D865D83EB88C1A1911B9E90641919A1314F36E1B099F8E95FE3B7C77BE3349\",\"ledger_hash\":\"F4D865D83EB88C1A1911B9E90641919A1314F36E1B099F8E95FE3B7C77BE3349\",\"parent_hash\":\"12724A65B030C15A1573AA28B1BBB5DF3DA4589AA3623675A31CAE69B23B1C4E\",\"total_coins\":\"99998831688050493\",\"transaction_hash\":\"325EACC5271322539EEEC2D6A5292471EF1B3E72AE7180533EFC3B8F0AD435C8\"},\"transactions\":[{\"TransactionType\":\"Payment\",\"Flags\":2147483648,\"Sequence\":1608,\"LastLedgerSequence\":15202446,\"Amount\":\"120000000\",\"Fee\":\"15000\",\"SigningPubKey\":\"03BC0973F997BC6384BE455B163519A3E96BC2D725C37F7172D5FED5DD38E2A357\",\"TxnSignature\":\"3045022100D80A1802B00AEEF9FDFDE594B0D568217A312D54E6337B8519C0D699841EFB96022067F6913B13D0EC2354C5A67CE0A41AE4181A09CD08A1BB0638D128D357961006\",\"Account\":\"rDPL68aNpdfp9h59R4QT5R6B1Z2W9oRc51\",\"Destination\":\"rE4S4Xw8euysJ3mt7gmK8EhhYEwmALpb3R\",\"metaData\":{\"TransactionIndex\":6,\"AffectedNodes\":[{\"ModifiedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"PreviousTxnLgrSeq\":15202381,\"PreviousTxnID\":\"8FFB65C6907C9679C5F8AADA97072CD1B8FE4955FC6A614AC87408AE7C9088AD\",\"LedgerIndex\":\"B07B367ABF05243A536986DEC74684E983BBBDDF443ADE9CDC43A22D6E6A1420\",\"PreviousFields\":{\"Sequence\":1608,\"Balance\":\"61455842701\"},\"FinalFields\":{\"Flags\":0,\"Sequence\":1609,\"OwnerCount\":0,\"Balance\":\"61335827701\",\"Account\":\"rDPL68aNpdfp9h59R4QT5R6B1Z2W9oRc51\"}}},{\"ModifiedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"PreviousTxnLgrSeq\":15202438,\"PreviousTxnID\":\"B01591A2353CD39EFAC989D542EE37591F60CF9BB2B66526C8C958774813407E\",\"LedgerIndex\":\"F77EB82FA9593E695F22155C00C569A570CF32316BEFDFF0B16BADAFF2ACFF19\",\"PreviousFields\":{\"Balance\":\"26762033252\"},\"FinalFields\":{\"Flags\":0,\"Sequence\":6448,\"OwnerCount\":3,\"Balance\":\"26882033252\",\"Account\":\"rE4S4Xw8euysJ3mt7gmK8EhhYEwmALpb3R\"}}}],\"TransactionResult\":\"tesSUCCESS\"}},{\"TransactionType\":\"Payment\",\"Flags\":2147483648,\"Sequence\":18874,\"LastLedgerSequence\":15202446,\"Amount\":\"120000000\",\"Fee\":\"15000\",\"SigningPubKey\":\"035D097E75D4B35345CEB30F9B1D18CB81165FE6ADD02481AA5B02B5F9C8107EE1\",\"TxnSignature\":\"304402203D80E8BC71908AB345948AB71FB7B8DE239DD79636D96D3C5BDA2B2F192A5EEA0220686413D69BF0D813FC61DABD437AEFAAE69925D3E10FCD5B2C4D90B5AF7B883D\",\"Account\":\"rnHScgV6wSP9sR25uYWiMo3QYNA5ybQ7cH\",\"Destination\":\"rwnnfHDaEAwXaVji52cWWizbHVMs2Cz5K9\",\"metaData\":{\"TransactionIndex\":5,\"AffectedNodes\":[{\"ModifiedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"PreviousTxnLgrSeq\":15202429,\"PreviousTxnID\":\"B1F39887411C1771998F38502EDF33170F9F5659503DB9DE642EBA896B5F198B\",\"LedgerIndex\":\"2AAA3361C593C4DE7ABD9A607B3CA7070A3F74E3C3F2FDE4DDB9484E47ED056E\",\"PreviousFields\":{\"Sequence\":18874,\"Balance\":\"13795295558367\"},\"FinalFields\":{\"Flags\":0,\"Sequence\":18875,\"OwnerCount\":0,\"Balance\":\"13795175543367\",\"Account\":\"rnHScgV6wSP9sR25uYWiMo3QYNA5ybQ7cH\"}}},{\"ModifiedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"PreviousTxnLgrSeq\":15202416,\"PreviousTxnID\":\"00CF9C7BE3EBAF76893C6A3F6D10B4D89F8D856C97B9D44938CF1682132ACEB8\",\"LedgerIndex\":\"928582D6F6942B18F3462FA04BA99F476B64FEB9921BFAD583182DC28CB74187\",\"PreviousFields\":{\"Balance\":\"17674359316\"},\"FinalFields\":{\"Flags\":0,\"Sequence\":1710,\"OwnerCount\":0,\"Balance\":\"17794359316\",\"Account\":\"rwnnfHDaEAwXaVji52cWWizbHVMs2Cz5K9\"}}}],\"TransactionResult\":\"tesSUCCESS\"}},{\"TransactionType\":\"Payment\",\"Flags\":2147483648,\"Sequence\":1615,\"LastLedgerSequence\":15202446,\"Amount\":\"400000000\",\"Fee\":\"15000\",\"SigningPubKey\":\"03ACFAA11628C558AB5E7FA64705F442BDAABA6E9D318B30E010BC87CDEA8D1D7D\",\"TxnSignature\":\"3045022100A3530C2E983FB05DFF27172C649494291F7BEBA2E6A59EEAF945CB9728D1DB5E022015BCA0E9D69760224DD7C2B68F3BC1F239D89C3397161AA3901C2E04EE31C18F\",\"Account\":\"razcSDpwds1aTeqDphqzBr7ay1ZELYAWTm\",\"Destination\":\"rhuqJAE2UfhGCvkR7Ve35bvm39JmRvFML4\",\"metaData\":{\"TransactionIndex\":4,\"AffectedNodes\":[{\"ModifiedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"PreviousTxnLgrSeq\":15202394,\"PreviousTxnID\":\"99E8F8988390F5A8DF69BBA4F04705E5085EE91B27583D28210D37B7513F10BB\",\"LedgerIndex\":\"17CF549DFC0813DDC44559C89E99B4C1D033D59FF379AD948CBEC141F179293D\",\"PreviousFields\":{\"Sequence\":1615,\"Balance\":\"45875786250\"},\"FinalFields\":{\"Flags\":0,\"Sequence\":1616,\"OwnerCount\":0,\"Balance\":\"45475771250\",\"Account\":\"razcSDpwds1aTeqDphqzBr7ay1ZELYAWTm\"}}},{\"ModifiedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"PreviousTxnLgrSeq\":15202438,\"PreviousTxnID\":\"9EC0784393DA95BB3B38FABC59FEFEE34BA8487DD892B9EAC1D70E483D1B0FA6\",\"LedgerIndex\":\"EB13399E9A69F121BEDA810F1AE9CB4023B4B09C5055CB057B572029B2FC8DD4\",\"PreviousFields\":{\"Balance\":\"76953067090\"},\"FinalFields\":{\"Flags\":0,\"Sequence\":601,\"OwnerCount\":4,\"Balance\":\"77353067090\",\"Account\":\"rhuqJAE2UfhGCvkR7Ve35bvm39JmRvFML4\"}}}],\"TransactionResult\":\"tesSUCCESS\"}},{\"TransactionType\":\"Payment\",\"Flags\":2147483648,\"Sequence\":1674,\"LastLedgerSequence\":15202446,\"Amount\":\"800000000\",\"Fee\":\"15000\",\"SigningPubKey\":\"028F28D78FDA74222F4008F012247DF3BBD42B90CE4CFD87E29598196108E91B52\",\"TxnSignature\":\"3044022065A003194D91E774D180BE47D4E086BB2624BC8F6DB7C655E135D5C6C03BBC7C02205DC961C2B7A06D701B29C2116ACF6F84CC84205FF44411576C15507852ECC31C\",\"Account\":\"rQGLp9nChtWkdgcHjj6McvJithN2S2HJsP\",\"Destination\":\"rEUubanepAAugnNJY1gxEZLDnk9W5NCoFU\",\"metaData\":{\"TransactionIndex\":3,\"AffectedNodes\":[{\"ModifiedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"PreviousTxnLgrSeq\":15202409,\"PreviousTxnID\":\"6A9B73C13B8A74BCDB64B5ADFE3D8FFEAC7928B82CFD6C9A35254D7798AD0688\",\"LedgerIndex\":\"D1A7795E8E997E7DE65D64283FD7CEEB5E43C2E5C4A794C2CFCEC6724E03F464\",\"PreviousFields\":{\"Sequence\":1674,\"Balance\":\"8774844732\"},\"FinalFields\":{\"Flags\":0,\"Sequence\":1675,\"OwnerCount\":0,\"Balance\":\"7974829732\",\"Account\":\"rQGLp9nChtWkdgcHjj6McvJithN2S2HJsP\"}}},{\"ModifiedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"PreviousTxnLgrSeq\":15202388,\"PreviousTxnID\":\"ECE994DA817228D9170D22C01CE1BF5B17FFE1AE6404FF215719C1049E9939E0\",\"LedgerIndex\":\"E5EA9215A6D41C4E20C831ACE436E5B75F9BA2A9BD4325BA65BD9D44F5E13A08\",\"PreviousFields\":{\"Balance\":\"9077529029\"},\"FinalFields\":{\"Flags\":0,\"Sequence\":1496,\"OwnerCount\":0,\"Balance\":\"9877529029\",\"Account\":\"rEUubanepAAugnNJY1gxEZLDnk9W5NCoFU\"}}}],\"TransactionResult\":\"tesSUCCESS\"}},{\"TransactionType\":\"OfferCreate\",\"Sequence\":289444,\"OfferSequence\":289443,\"LastLedgerSequence\":15202441,\"TakerPays\":{\"value\":\"19.99999999991\",\"currency\":\"EUR\",\"issuer\":\"rMwjYedjc7qqtKYVLiAccJSmCwih4LnE2q\"},\"TakerGets\":{\"value\":\"20.88367500010602\",\"currency\":\"USD\",\"issuer\":\"rMwjYedjc7qqtKYVLiAccJSmCwih4LnE2q\"},\"Fee\":\"10000\",\"SigningPubKey\":\"024D129D4F5A12D4C5A9E9D1E4AC447BBE3496F182FAE82F7709C7EB9F12DBC697\",\"TxnSignature\":\"3044022041EBE6B06BA493867F4FFBD72E5D6253F97306E1E82DABDF9649E15B1151B59F0220539C589F40174471C067FDC761A2B791F36F1A3C322734B43DB16880E489BD81\",\"Account\":\"rD8LigXE7165r3VWhSQ4FwzJy7PNrTMwUq\",\"Memos\":[{\"Memo\":{\"MemoType\":\"6F666665725F636F6D6D656E74\",\"MemoData\":\"72655F6575722368656467655F726970706C65\",\"parsed_memo_type\":\"offer_comment\"}}],\"metaData\":{\"TransactionIndex\":2,\"AffectedNodes\":[{\"CreatedNode\":{\"LedgerEntryType\":\"Offer\",\"LedgerIndex\":\"2069A6F3B349C246630536B3A0D18FECF0B088D6846ED74D56762096B972ADBE\",\"NewFields\":{\"Sequence\":289444,\"BookDirectory\":\"D3C7DF102A0CEDB307D6F471B0CE679C5C206D8227D9BB2E5422061A1FB5AF31\",\"TakerPays\":{\"value\":\"19.99999999991\",\"currency\":\"EUR\",\"issuer\":\"rMwjYedjc7qqtKYVLiAccJSmCwih4LnE2q\"},\"TakerGets\":{\"value\":\"20.88367500010602\",\"currency\":\"USD\",\"issuer\":\"rMwjYedjc7qqtKYVLiAccJSmCwih4LnE2q\"},\"Account\":\"rD8LigXE7165r3VWhSQ4FwzJy7PNrTMwUq\"}}},{\"ModifiedNode\":{\"LedgerEntryType\":\"DirectoryNode\",\"LedgerIndex\":\"68E8826D6545315B54943AF0D6A45264598F2DE8A71CB9EFA97C9F4456078BE8\",\"FinalFields\":{\"Flags\":0,\"RootIndex\":\"68E8826D6545315B54943AF0D6A45264598F2DE8A71CB9EFA97C9F4456078BE8\",\"Owner\":\"rD8LigXE7165r3VWhSQ4FwzJy7PNrTMwUq\"}}},{\"DeletedNode\":{\"LedgerEntryType\":\"Offer\",\"LedgerIndex\":\"9AC6C83397287FDFF4DB7ED6D96DA060CF32ED6593B18C332EEDFE833AE48E1C\",\"FinalFields\":{\"Flags\":0,\"Sequence\":289443,\"PreviousTxnLgrSeq\":15202438,\"BookNode\":\"0000000000000000\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"6C1B0818CA470DBD5EFC28FC863862B0DF9D9F659475612446806401C56E3B28\",\"BookDirectory\":\"D3C7DF102A0CEDB307D6F471B0CE679C5C206D8227D9BB2E5422061A1FB5AF31\",\"TakerPays\":{\"value\":\"19.99999999991\",\"currency\":\"EUR\",\"issuer\":\"rMwjYedjc7qqtKYVLiAccJSmCwih4LnE2q\"},\"TakerGets\":{\"value\":\"20.88367500010602\",\"currency\":\"USD\",\"issuer\":\"rMwjYedjc7qqtKYVLiAccJSmCwih4LnE2q\"},\"Account\":\"rD8LigXE7165r3VWhSQ4FwzJy7PNrTMwUq\"}}},{\"ModifiedNode\":{\"LedgerEntryType\":\"DirectoryNode\",\"LedgerIndex\":\"D3C7DF102A0CEDB307D6F471B0CE679C5C206D8227D9BB2E5422061A1FB5AF31\",\"FinalFields\":{\"Flags\":0,\"ExchangeRate\":\"5422061A1FB5AF31\",\"RootIndex\":\"D3C7DF102A0CEDB307D6F471B0CE679C5C206D8227D9BB2E5422061A1FB5AF31\",\"TakerPaysCurrency\":\"0000000000000000000000004555520000000000\",\"TakerPaysIssuer\":\"DD39C650A96EDA48334E70CC4A85B8B2E8502CD3\",\"TakerGetsCurrency\":\"0000000000000000000000005553440000000000\",\"TakerGetsIssuer\":\"DD39C650A96EDA48334E70CC4A85B8B2E8502CD3\"}}},{\"ModifiedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"PreviousTxnLgrSeq\":15202438,\"PreviousTxnID\":\"6C1B0818CA470DBD5EFC28FC863862B0DF9D9F659475612446806401C56E3B28\",\"LedgerIndex\":\"D8614A045CBA0F0081B23FD80CA87E7D08651FA02450C7BEE1B480836F0DC95D\",\"PreviousFields\":{\"Sequence\":289444,\"Balance\":\"3712981021\"},\"FinalFields\":{\"Flags\":0,\"Sequence\":289445,\"OwnerCount\":13,\"Balance\":\"3712971021\",\"Account\":\"rD8LigXE7165r3VWhSQ4FwzJy7PNrTMwUq\"}}}],\"TransactionResult\":\"tesSUCCESS\"}},{\"TransactionType\":\"AccountSet\",\"Flags\":2147483648,\"Sequence\":387262,\"LastLedgerSequence\":15202440,\"Fee\":\"10500\",\"SigningPubKey\":\"027DFE042DC2BD07D2E88DD526A5FBF816C831C25CA0BB62A3BF320A3B2BA6DB5C\",\"TxnSignature\":\"30440220572D89688D9F9DB9874CDDDD3EBDCB5808A836982864C81F185FBC54FAD1A7B902202E09AAA6D65EECC9ACDEA7F70D8D2EE024152C7B288FA9E42C427260CF922F58\",\"Account\":\"rn6uAt46Xi6uxA2dRCtqaJyM3aaP6V9WWM\",\"metaData\":{\"TransactionIndex\":1,\"AffectedNodes\":[{\"ModifiedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"PreviousTxnLgrSeq\":15202429,\"PreviousTxnID\":\"212D4BFAD4DFB0887B57AB840A8385F31FC2839FFD4169A824280565CC2885C0\",\"LedgerIndex\":\"317481AD6274D399F50E13EF447825DA628197E6262B80642DAE0D8300D77E55\",\"PreviousFields\":{\"Sequence\":387262,\"Balance\":\"207020609\"},\"FinalFields\":{\"Flags\":0,\"Sequence\":387263,\"OwnerCount\":22,\"Balance\":\"207010109\",\"Account\":\"rn6uAt46Xi6uxA2dRCtqaJyM3aaP6V9WWM\"}}}],\"TransactionResult\":\"tesSUCCESS\"}},{\"TransactionType\":\"Payment\",\"Flags\":2147483648,\"Sequence\":1673,\"LastLedgerSequence\":15202446,\"Amount\":\"1700000000\",\"Fee\":\"15000\",\"SigningPubKey\":\"02C26CF5D395A1CB352BE10D5AAB73FE27FC0AFAE0BD6121E55D097EBDCF394E11\",\"TxnSignature\":\"304402204190B6DC7D14B1CC8DDAA87F1C01FEDA6D67D598D65E1AA19D4ADE937ED14B720220662EE404438F415AD3335B9FBA1A4C2A5F72AA387740D8A011A8C53346481B1D\",\"Account\":\"rEE77T1E5vEFcEB9zM92jBD3rPs3kPdS1j\",\"Destination\":\"r3AsrDRMNYaKNCofo9a5Us7R66RAzTigiU\",\"metaData\":{\"TransactionIndex\":0,\"AffectedNodes\":[{\"ModifiedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"PreviousTxnLgrSeq\":15202352,\"PreviousTxnID\":\"6B3D159578F8E1CEBB268DBC5209ADB35DD075F463855886421D307026D27C67\",\"LedgerIndex\":\"AB5EBD00C6F12DEC32B1687A51948ADF07DC2ABDD7485E9665DCE5268039B461\",\"PreviousFields\":{\"Balance\":\"23493344926\"},\"FinalFields\":{\"Flags\":0,\"Sequence\":1775,\"OwnerCount\":0,\"Balance\":\"25193344926\",\"Account\":\"r3AsrDRMNYaKNCofo9a5Us7R66RAzTigiU\"}}},{\"ModifiedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"PreviousTxnLgrSeq\":15202236,\"PreviousTxnID\":\"A2C23A20377BA7A90F77F01F8E337B64E22C929C5490E2E9698A7A9BFFEC592A\",\"LedgerIndex\":\"C67232D5308CBE1A8C3D75284D98CC1623D906DB30774C06B3F4934BC1DE5CEE\",\"PreviousFields\":{\"Sequence\":1673,\"Balance\":\"17034504878\"},\"FinalFields\":{\"Flags\":0,\"Sequence\":1674,\"OwnerCount\":0,\"Balance\":\"15334489878\",\"Account\":\"rEE77T1E5vEFcEB9zM92jBD3rPs3kPdS1j\"}}}],\"TransactionResult\":\"tesSUCCESS\"}}]},\"getOrderbook\":{\"normal\":{\"takerPays\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\"},\"takerGets\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\"}},\"withXRP\":{\"takerPays\":{\"currency\":\"USD\",\"issuer\":\"rp8rJYTpodf8qbSCHVTNacf8nSW8mRakFw\"},\"takerGets\":{\"currency\":\"XRP\"}}},\"sign\":{\"normal\":{\"TransactionType\":\"AccountSet\",\"Flags\":2147483648,\"Sequence\":23,\"LastLedgerSequence\":8820051,\"Fee\":\"12\",\"SigningPubKey\":\"02A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F544\",\"Domain\":\"6578616D706C652E636F6D\",\"Account\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\"},\"signAs\":{\"Account\":\"rnUy2SHTrB9DubsPmkJZUXTf5FcNDGrYEA\",\"Amount\":\"1000000000\",\"Destination\":\"rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh\",\"Fee\":\"50\",\"Sequence\":2,\"TransactionType\":\"Payment\"},\"ticket\":{\"TransactionType\":\"TicketCreate\",\"TicketCount\":1,\"Account\":\"r4SDqUD1ZcfoZrhnsZ94XNFKxYL4oHYJyA\",\"Sequence\":0,\"TicketSequence\":23,\"Fee\":\"10000\"},\"escrow\":{\"TransactionType\":\"EscrowFinish\",\"Account\":\"rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh\",\"Owner\":\"rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh\",\"OfferSequence\":2,\"Condition\":\"712C36933822AD3A3D136C5DF97AA863B69F9CE88B2D6CE6BDD11BFDE290C19D\",\"Fulfillment\":\"74686973206D757374206861766520333220636861726163746572732E2E2E2E\",\"Flags\":2147483648,\"LastLedgerSequence\":102,\"Fee\":\"12\",\"Sequence\":1}},\"signPaymentChannelClaim\":{\"channel\":\"3E18C05AD40319B809520F1A136370C4075321B285217323396D6FD9EE1E9037\",\"amount\":\".00001\"}}";
        Dictionary<string, dynamic> requestJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(requestFixture);

        static string responseFixture = "{\"generateAddress\":{\"xAddress\":\"XVLcsWWNiFdUEqoDmSwgxh1abfddG1LtbGFk7omPgYpbyE8\",\"classicAddress\":\"rGCkuB7PBr5tNy68tPEABEtcdno4hE6Y7f\",\"secret\":\"sp6JS7f14BuwFY8Mw6bTtLKWauoUs\"},\"generateXAddress\":{\"xAddress\":\"XVLcsWWNiFdUEqoDmSwgxh1abfddG1LtbGFk7omPgYpbyE8\",\"secret\":\"sp6JS7f14BuwFY8Mw6bTtLKWauoUs\"},\"getAccountObjects\":{\"account\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"account_objects\":[{\"Balance\":{\"currency\":\"ASP\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"0\"},\"Flags\":65536,\"HighLimit\":{\"currency\":\"ASP\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"ASP\",\"issuer\":\"r3vi7mWxru9rJCxETCyA1CHvzL96eZWx5z\",\"value\":\"10\"},\"LowNode\":\"0000000000000000\",\"PreviousTxnID\":\"BF7555B0F018E3C5E2A3FF9437A1A5092F32903BE246202F988181B9CED0D862\",\"PreviousTxnLgrSeq\":1438879,\"index\":\"2243B0B630EA6F7330B654EFA53E27A7609D9484E535AB11B7F946DF3D247CE9\"},{\"Balance\":{\"currency\":\"XAU\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"0\"},\"Flags\":3342336,\"HighLimit\":{\"currency\":\"XAU\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"XAU\",\"issuer\":\"r3vi7mWxru9rJCxETCyA1CHvzL96eZWx5z\",\"value\":\"0\"},\"LowNode\":\"0000000000000000\",\"PreviousTxnID\":\"79B26D7D34B950AC2C2F91A299A6888FABB376DD76CFF79D56E805BF439F6942\",\"PreviousTxnLgrSeq\":5982530,\"index\":\"9ED4406351B7A511A012A9B5E7FE4059FA2F7650621379C0013492C315E25B97\"},{\"Balance\":{\"currency\":\"USD\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"0\"},\"Flags\":1114112,\"HighLimit\":{\"currency\":\"USD\",\"issuer\":\"rMwjYedjc7qqtKYVLiAccJSmCwih4LnE2q\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"USD\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"5\"},\"LowNode\":\"0000000000000000\",\"PreviousTxnID\":\"6FE8C824364FB1195BCFEDCB368DFEE3980F7F78D3BF4DC4174BB4C86CF8C5CE\",\"PreviousTxnLgrSeq\":10555014,\"index\":\"2DECFAC23B77D5AEA6116C15F5C6D4669EBAEE9E7EE050A40FE2B1E47B6A9419\"},{\"Balance\":{\"currency\":\"MXN\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"481.992867407479\"},\"Flags\":65536,\"HighLimit\":{\"currency\":\"MXN\",\"issuer\":\"rHpXfibHgSb64n8kK9QWDpdbfqSpYbM9a4\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"MXN\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"1000\"},\"LowNode\":\"0000000000000000\",\"PreviousTxnID\":\"A467BACE5F183CDE1F075F72435FE86BAD8626ED1048EDEFF7562A4CC76FD1C5\",\"PreviousTxnLgrSeq\":3316170,\"index\":\"EC8B9B6B364AF6CB6393A423FDD2DDBA96375EC772E6B50A3581E53BFBDFDD9A\"},{\"Balance\":{\"currency\":\"EUR\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"0.793598266778297\"},\"Flags\":1114112,\"HighLimit\":{\"currency\":\"EUR\",\"issuer\":\"rLEsXccBGNR3UPuPu2hUXPjziKC3qKSBun\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"EUR\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"1\"},\"LowNode\":\"0000000000000000\",\"PreviousTxnID\":\"E9345D44433EA368CFE1E00D84809C8E695C87FED18859248E13662D46A0EC46\",\"PreviousTxnLgrSeq\":5447146,\"index\":\"4513749B30F4AF8DA11F077C448128D6486BF12854B760E4E5808714588AA915\"},{\"Balance\":{\"currency\":\"CNY\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"0\"},\"Flags\":2228224,\"HighLimit\":{\"currency\":\"CNY\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"3\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"CNY\",\"issuer\":\"rnuF96W4SZoCJmbHYBFoJZpR8eCaxNvekK\",\"value\":\"0\"},\"LowNode\":\"0000000000000008\",\"PreviousTxnID\":\"2FDDC81F4394695B01A47913BEC4281AC9A283CC8F903C14ADEA970F60E57FCF\",\"PreviousTxnLgrSeq\":5949673,\"index\":\"578C327DA8944BDE2E10C9BA36AFA2F43E06C8D1E8819FB225D266CBBCFDE5CE\"},{\"Balance\":{\"currency\":\"DYM\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"1.336889190631542\"},\"Flags\":65536,\"HighLimit\":{\"currency\":\"DYM\",\"issuer\":\"rGwUWgN5BEg3QGNY3RX2HfYowjUTZdid3E\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"DYM\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"3\"},\"LowNode\":\"0000000000000000\",\"PreviousTxnID\":\"6DA2BD02DFB83FA4DAFC2651860B60071156171E9C021D9E0372A61A477FFBB1\",\"PreviousTxnLgrSeq\":8818732,\"index\":\"5A2A5FF12E71AEE57564E624117BBA68DEF78CD564EF6259F92A011693E027C7\"},{\"Balance\":{\"currency\":\"CHF\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"-0.3488146605801446\"},\"Flags\":131072,\"HighLimit\":{\"currency\":\"CHF\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"0\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"CHF\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0\"},\"LowNode\":\"000000000000008C\",\"PreviousTxnID\":\"722394372525A13D1EAAB005642F50F05A93CF63F7F472E0F91CDD6D38EB5869\",\"PreviousTxnLgrSeq\":2687590,\"index\":\"F2DBAD20072527F6AD02CE7F5A450DBC72BE2ABB91741A8A3ADD30D5AD7A99FB\"},{\"Balance\":{\"currency\":\"BTC\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"0\"},\"Flags\":131072,\"HighLimit\":{\"currency\":\"BTC\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"3\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0\"},\"LowNode\":\"0000000000000043\",\"PreviousTxnID\":\"03EDF724397D2DEE70E49D512AECD619E9EA536BE6CFD48ED167AE2596055C9A\",\"PreviousTxnLgrSeq\":8317037,\"index\":\"767C12AF647CDF5FEB9019B37018748A79C50EDAF87E8D4C7F39F78AA7CA9765\"},{\"Balance\":{\"currency\":\"USD\",\"issuer\":\"rrrrrrrrrrrrrrrrrrrrBZbvji\",\"value\":\"-16.00534471983042\"},\"Flags\":131072,\"HighLimit\":{\"currency\":\"USD\",\"issuer\":\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"value\":\"5000\"},\"HighNode\":\"0000000000000000\",\"LedgerEntryType\":\"RippleState\",\"LowLimit\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0\"},\"LowNode\":\"000000000000004A\",\"PreviousTxnID\":\"CFFF5CFE623C9543308C6529782B6A6532207D819795AAFE85555DB8BF390FE7\",\"PreviousTxnLgrSeq\":14365854,\"index\":\"826CF5BFD28F3934B518D0BDF3231259CBD3FD0946E3C3CA0C97D2C75D2D1A09\"}],\"ledger_hash\":\"053DF17D2289D1C4971C22F235BC1FCA7D4B3AE966F842E5819D0749E0B8ECD3\",\"ledger_index\":14378733,\"validated\":true},\"getBalances\":[{\"value\":\"922.913243\",\"currency\":\"XRP\"},{\"value\":\"0\",\"currency\":\"ASP\",\"issuer\":\"r3vi7mWxru9rJCxETCyA1CHvzL96eZWx5z\"},{\"value\":\"0\",\"currency\":\"XAU\",\"issuer\":\"r3vi7mWxru9rJCxETCyA1CHvzL96eZWx5z\"},{\"value\":\"2.497605752725159\",\"currency\":\"USD\",\"issuer\":\"rMwjYedjc7qqtKYVLiAccJSmCwih4LnE2q\"},{\"value\":\"481.992867407479\",\"currency\":\"MXN\",\"issuer\":\"rHpXfibHgSb64n8kK9QWDpdbfqSpYbM9a4\"},{\"value\":\"0.793598266778297\",\"currency\":\"EUR\",\"issuer\":\"rLEsXccBGNR3UPuPu2hUXPjziKC3qKSBun\"},{\"value\":\"0\",\"currency\":\"CNY\",\"issuer\":\"rnuF96W4SZoCJmbHYBFoJZpR8eCaxNvekK\"},{\"value\":\"1.294889190631542\",\"currency\":\"DYM\",\"issuer\":\"rGwUWgN5BEg3QGNY3RX2HfYowjUTZdid3E\"},{\"value\":\"0.3488146605801446\",\"currency\":\"CHF\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\"},{\"value\":\"2.114103174931847\",\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\"},{\"value\":\"0\",\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\"},{\"value\":\"-0.00111\",\"currency\":\"BTC\",\"issuer\":\"rpgKWEmNqSDAGFhy5WDnsyPqfQxbWxKeVd\"},{\"value\":\"-0.1010780000080207\",\"currency\":\"BTC\",\"issuer\":\"rBJ3YjwXi2MGbg7GVLuTXUWQ8DjL7tDXh4\"},{\"value\":\"1\",\"currency\":\"USD\",\"issuer\":\"rLEsXccBGNR3UPuPu2hUXPjziKC3qKSBun\"},{\"value\":\"8.07619790068559\",\"currency\":\"CNY\",\"issuer\":\"razqQKzJRdB4UxFPWf5NEpEG3WMkmwgcXA\"},{\"value\":\"7.292695098901099\",\"currency\":\"JPY\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\"},{\"value\":\"0\",\"currency\":\"AUX\",\"issuer\":\"r3vi7mWxru9rJCxETCyA1CHvzL96eZWx5z\"},{\"value\":\"0\",\"currency\":\"USD\",\"issuer\":\"r9vbV3EHvXWjSkeQ6CAcYVPGeq7TuiXY2X\"},{\"value\":\"12.41688780720394\",\"currency\":\"EUR\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\"},{\"value\":\"35\",\"currency\":\"USD\",\"issuer\":\"rfF3PNkwkq1DygW2wum2HK3RGfgkJjdPVD\"},{\"value\":\"-5\",\"currency\":\"JOE\",\"issuer\":\"rwUVoVMSURqNyvocPCcvLu3ygJzZyw8qwp\"},{\"value\":\"0\",\"currency\":\"USD\",\"issuer\":\"rE6R3DWF9fBD7CyiQciePF9SqK58Ubp8o2\"},{\"value\":\"0\",\"currency\":\"JOE\",\"issuer\":\"rE6R3DWF9fBD7CyiQciePF9SqK58Ubp8o2\"},{\"value\":\"0\",\"currency\":\"015841551A748AD2C1F76FF6ECB0CCCD00000000\",\"issuer\":\"rs9M85karFkCRjvc6KMWn8Coigm9cbcgcx\"},{\"value\":\"0\",\"currency\":\"USD\",\"issuer\":\"rEhDDUUNxpXgEHVJtC2cjXAgyx5VCFxdMF\"}],\"getServerInfo\":{\"build_version\":\"0.24.0-rc1\",\"complete_ledgers\":\"32570-6595042\",\"hostid\":\"ARTS\",\"io_latency_ms\":1,\"last_close\":{\"converge_time_s\":2.007,\"proposers\":4},\"load_factor\":1,\"peers\":53,\"pubkey_node\":\"n94wWvFUmaKGYrKUGgpv1DyYgDeXRGdACkNQaSe7zJiy5Znio7UC\",\"server_state\":\"full\",\"validated_ledger\":{\"age\":5,\"base_fee_xrp\":1.0e-5,\"hash\":\"4482DEE5362332F54A4036ED57EE1767C9F33CF7CE5A6670355C16CECE381D46\",\"reserve_base_xrp\":20,\"reserve_inc_xrp\":5,\"seq\":6595042},\"validation_quorum\":3},\"signPaymentChannelClaim\":\"3045022100B5C54654221F154347679B97AE7791CBEF5E6772A3F894F9C781B8F1B400F89F022021E466D29DC5AEB5DFAFC76E8A88D2E388EBD25A84143B6AC3B647F479CB89B7\",\"getOrderbook\":{\"normal\":{\"buy\":[{\"Account\":\"r49y2xKuKVG2dPkNHgWQAV61cjxk8gryjQ\",\"BookDirectory\":\"20294C923E80A51B487EB9547B3835FD483748B170D2D0A4520B15A60037FFCF\",\"BookNode\":\"0000000000000000\",\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"544932DC56D72E845AF2B738821FE07865E32EC196270678AB0D947F54E9F49F\",\"PreviousTxnLgrSeq\":10679000,\"Sequence\":434,\"TakerGets\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"3205.1\"},\"TakerPays\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"10\"},\"index\":\"CE457115A4ADCC8CB351B3E35A0851E48DE16605C23E305017A9B697B156DE5A\",\"owner_funds\":\"41952.95917199965\",\"quality\":\"0.003120027456241615\"},{\"Account\":\"rDYCRhpahKEhCFV25xScg67Bwf4W9sTYAm\",\"BookDirectory\":\"20294C923E80A51B487EB9547B3835FD483748B170D2D0A4520B1A2BC2EC5000\",\"BookNode\":\"0000000000000000\",\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"F68F9658AB3D462FEB027E6C380F054BC6D2514B43EC3C6AD46EE19C59BF1CC3\",\"PreviousTxnLgrSeq\":10704238,\"Sequence\":233,\"TakerGets\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"1599.063669386278\"},\"TakerPays\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"4.99707396683212\"},\"index\":\"BF14FBB305159DBCAEA91B7E848408F5B559A91B160EBCB6D244958A6A16EA6B\",\"owner_funds\":\"3169.910902910102\",\"quality\":\"0.003125\"},{\"Account\":\"raudnGKfTK23YKfnS7ixejHrqGERTYNFXk\",\"BookDirectory\":\"20294C923E80A51B487EB9547B3835FD483748B170D2D0A4520B2BF1C2F4D4C9\",\"BookNode\":\"0000000000000000\",\"Expiration\":472785284,\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"00000000000008F0\",\"PreviousTxnID\":\"446410E1CD718AC01929DD16B558FCF6B3A7B8BF208C420E67A280C089C5C59B\",\"PreviousTxnLgrSeq\":10713576,\"Sequence\":110104,\"TakerGets\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"143.1050962074379\"},\"TakerPays\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.4499999999999999\"},\"index\":\"67924B0EAA15784CC00CCD5FDD655EE2D6D2AE40341776B5F14E52341E7FC73E\",\"owner_funds\":\"0\",\"quality\":\"0.003144542101755081\",\"taker_gets_funded\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0\"},\"taker_pays_funded\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0\"}},{\"Account\":\"rDVBvAQScXrGRGnzrxRrcJPeNLeLeUTAqE\",\"BookDirectory\":\"20294C923E80A51B487EB9547B3835FD483748B170D2D0A4520B2CD7A2BFBB75\",\"BookNode\":\"0000000000000000\",\"Expiration\":472772651,\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"00000000000003CD\",\"PreviousTxnID\":\"D49164AB68DDA3AEC9DFCC69A35685C4F532B5C231D3C1D25FEA7D5D0224FB84\",\"PreviousTxnLgrSeq\":10711128,\"Sequence\":35625,\"TakerGets\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"254.329207354604\"},\"TakerPays\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.8\"},\"index\":\"567BF2825173E3FB28FC94E436B6EB30D9A415FC2335E6D25CDE1BE47B25D120\",\"owner_funds\":\"0\",\"quality\":\"0.003145529403882357\",\"taker_gets_funded\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0\"},\"taker_pays_funded\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0\"}},{\"Account\":\"rwBYyfufTzk77zUSKEu4MvixfarC35av1J\",\"BookDirectory\":\"20294C923E80A51B487EB9547B3835FD483748B170D2D0A4520B3621DF140FDA\",\"BookNode\":\"0000000000000000\",\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000008\",\"PreviousTxnID\":\"2E371E2B287C8A9FBB3424E4204B17AD9FA1BAA9F3B33C7D2261E3B038AFF083\",\"PreviousTxnLgrSeq\":10716291,\"Sequence\":387756,\"TakerGets\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"390.4979\"},\"TakerPays\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"1.23231134568807\"},\"index\":\"8CA23E55BF9F46AC7E803D3DB40FD03225EFCA66650D4CF0CBDD28A7CCDC8400\",\"owner_funds\":\"5704.824764087842\",\"quality\":\"0.003155743848271834\"},{\"Account\":\"rwjsRktX1eguUr1pHTffyHnC4uyrvX58V1\",\"BookDirectory\":\"20294C923E80A51B487EB9547B3835FD483748B170D2D0A4520B3A4D41FF4211\",\"BookNode\":\"0000000000000000\",\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"91763FA7089C63CC4D5D14CBA6A5A5BF7ECE949B0D34F00FD35E733AF9F05AF1\",\"PreviousTxnLgrSeq\":10716292,\"Sequence\":208927,\"TakerGets\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"1\"},\"TakerPays\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.003160328237957649\"},\"index\":\"7206866E39D9843623EE79E570242753DEE3C597F3856AEFB4631DD5AD8B0557\",\"owner_funds\":\"45.55665106096075\",\"quality\":\"0.003160328237957649\"},{\"Account\":\"r49y2xKuKVG2dPkNHgWQAV61cjxk8gryjQ\",\"BookDirectory\":\"20294C923E80A51B487EB9547B3835FD483748B170D2D0A4520B4748E68669A7\",\"BookNode\":\"0000000000000000\",\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"3B3CF6FF1A336335E78513CF77AFD3A784ACDD7B1B4D3F1F16E22957A060BFAE\",\"PreviousTxnLgrSeq\":10639969,\"Sequence\":429,\"TakerGets\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"4725\"},\"TakerPays\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"15\"},\"index\":\"42894809370C7E6B23498EF8E22AD4B05F02B94F08E6983357A51EA96A95FF7F\",\"quality\":\"0.003174603174603175\"},{\"Account\":\"rDVBvAQScXrGRGnzrxRrcJPeNLeLeUTAqE\",\"BookDirectory\":\"20294C923E80A51B487EB9547B3835FD483748B170D2D0A4520B72A555B981A3\",\"BookNode\":\"0000000000000000\",\"Expiration\":472772652,\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"00000000000003CD\",\"PreviousTxnID\":\"146C8DBB047BAAFAE5B8C8DECCCDACD9DFCD7A464E5AB273230FF975E9B83CF7\",\"PreviousTxnLgrSeq\":10711128,\"Sequence\":35627,\"TakerGets\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"496.5429474010489\"},\"TakerPays\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"1.6\"},\"index\":\"50CAA04E81D0009115B61C132FC9887FA9E5336E0CB8A2E7D3280ADBF6ABC043\",\"quality\":\"0.003222279177208227\",\"taker_gets_funded\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0\"},\"taker_pays_funded\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0\"}},{\"Account\":\"r49y2xKuKVG2dPkNHgWQAV61cjxk8gryjQ\",\"BookDirectory\":\"20294C923E80A51B487EB9547B3835FD483748B170D2D0A4520B730474DD96E5\",\"BookNode\":\"0000000000000000\",\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"624F9ADA85EC3BE845EAC075B47E01E4F89288EAF27823C715777B3DFFB21F24\",\"PreviousTxnLgrSeq\":10639989,\"Sequence\":431,\"TakerGets\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"3103\"},\"TakerPays\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"10\"},\"index\":\"8A319A496288228AD9CAD74375E32FA81805C56A9AD84798A26756A8B3F9EE23\",\"quality\":\"0.003222687721559781\"},{\"Account\":\"rwBYyfufTzk77zUSKEu4MvixfarC35av1J\",\"BookDirectory\":\"6EAB7C172DEFA430DBFAD120FDC373B5F5AF8B191649EC98570B9980E49C7DE8\",\"BookNode\":\"0000000000000000\",\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000008\",\"PreviousTxnID\":\"92DBA0BE18B331AC61FB277211477A255D3B5EA9C5FE689171DE689FB45FE18A\",\"PreviousTxnLgrSeq\":10714030,\"Sequence\":386940,\"TakerGets\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.2849323720855092\"},\"TakerPays\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"93.030522464522\"},\"index\":\"8092033091034D94219BC1131AF7A6B469D790D81831CB479AB6F67A32BE4E13\",\"owner_funds\":\"31.77682120227525\",\"quality\":\"326.5003614141928\"},{\"Account\":\"rwjsRktX1eguUr1pHTffyHnC4uyrvX58V1\",\"BookDirectory\":\"6EAB7C172DEFA430DBFAD120FDC373B5F5AF8B191649EC98570BBF1EEFA2FB0A\",\"BookNode\":\"0000000000000000\",\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"C6BDA152363E3CFE18688A6830B49F3DB2B05976110B5908EA4EB66D93DEEB1F\",\"PreviousTxnLgrSeq\":10714031,\"Sequence\":207855,\"TakerGets\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.00302447007930511\"},\"TakerPays\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"1\"},\"index\":\"8DB3520FF9CB16A0EA955056C49115F8CFB03A587D0A4AFC844F1D220EFCE0B9\",\"owner_funds\":\"0.0670537912615556\",\"quality\":\"330.6364334177034\"},{\"Account\":\"rUeCeioKJkbYhv4mRGuAbZpPcqkMCoYq6N\",\"BookDirectory\":\"6EAB7C172DEFA430DBFAD120FDC373B5F5AF8B191649EC98570D0069F50EA028\",\"BookNode\":\"0000000000000000\",\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000012\",\"PreviousTxnID\":\"F0E8ABF07F83DF0B5EF5B417E8E29A45A5503BA8F26FBC86447CC6B1FAD6A1C4\",\"PreviousTxnLgrSeq\":10447672,\"Sequence\":5255,\"TakerGets\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.5\"},\"TakerPays\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"182.9814890090516\"},\"index\":\"D652DCE4B19C6CB43912651D3A975371D3B2A16A034EDF07BC11BF721AEF94A4\",\"owner_funds\":\"0.225891986027944\",\"quality\":\"365.9629780181032\",\"taker_gets_funded\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.2254411038203033\"},\"taker_pays_funded\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"82.50309772176658\"}}],\"sell\":[{\"Account\":\"rDbsCJr5m8gHDCNEHCZtFxcXHsD4S9jH83\",\"BookDirectory\":\"20294C923E80A51B487EB9547B3835FD483748B170D2D0A4520B58077ED03C1B\",\"BookNode\":\"0000000000000000\",\"Flags\":131072,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000001\",\"PreviousTxnID\":\"98F3F2D02D3BB0AEAC09EECCF2F24BBE5E1AB2C71C40D7BD0A5199E12541B6E2\",\"PreviousTxnLgrSeq\":10715839,\"Sequence\":110099,\"TakerGets\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"1.24252537879871\"},\"TakerPays\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.003967400879423823\"},\"index\":\"F4404D6547149419D3607F81D7080979FBB3AFE2661F9A933E2F6C07AC1D1F6D\",\"owner_funds\":\"73.52163803897041\",\"quality\":\"0.003193013959408667\"},{\"Account\":\"raudnGKfTK23YKfnS7ixejHrqGERTYNFXk\",\"BookDirectory\":\"6EAB7C172DEFA430DBFAD120FDC373B5F5AF8B191649EC98570BC3A506FC016F\",\"BookNode\":\"0000000000000000\",\"Expiration\":472785283,\"Flags\":131072,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"00000000000008F0\",\"PreviousTxnID\":\"77E763F1D02F58965CD1AD94F557B37A582FAC7760B71F391B856959836C2F7B\",\"PreviousTxnLgrSeq\":10713576,\"Sequence\":110103,\"TakerGets\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.3\"},\"TakerPays\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"99.34014894048333\"},\"index\":\"9ECDFD31B28643FD3A54658398C5715D6DAD574F83F04529CB24765770F9084D\",\"owner_funds\":\"4.021116654525635\",\"quality\":\"331.1338298016111\"},{\"Account\":\"rPyYxUGK8L4dgEvjPs3aRc1B1jEiLr3Hx5\",\"BookDirectory\":\"6EAB7C172DEFA430DBFAD120FDC373B5F5AF8B191649EC98570BCB85BCA78000\",\"BookNode\":\"0000000000000000\",\"Flags\":131072,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"D22993C68C94ACE3F2FCE4A334EBEA98CC46DCA92886C12B5E5B4780B5E17D4E\",\"PreviousTxnLgrSeq\":10711938,\"Sequence\":392,\"TakerGets\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.8095\"},\"TakerPays\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"268.754\"},\"index\":\"18B136E08EF50F0DEE8521EA22D16A950CD8B6DDF5F6E07C35F7FDDBBB09718D\",\"owner_funds\":\"0.8095132334507441\",\"quality\":\"332\",\"taker_gets_funded\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.8078974385735969\"},\"taker_pays_funded\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"268.2219496064341\"}},{\"Account\":\"raudnGKfTK23YKfnS7ixejHrqGERTYNFXk\",\"BookDirectory\":\"6EAB7C172DEFA430DBFAD120FDC373B5F5AF8B191649EC98570C00450D461510\",\"BookNode\":\"0000000000000000\",\"Expiration\":472785284,\"Flags\":131072,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"00000000000008F0\",\"PreviousTxnID\":\"1F4D9D859D9AABA888C0708A572B38919A3AEF2C8C1F5A13F58F44C92E5FF3FB\",\"PreviousTxnLgrSeq\":10713576,\"Sequence\":110105,\"TakerGets\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.4499999999999999\"},\"TakerPays\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"152.0098333185607\"},\"index\":\"9F380E0B39E2AF8AA9608C3E39A5A8628E6D0F44385C6D12BE06F4FEC8D83351\",\"quality\":\"337.7996295968016\"},{\"Account\":\"rDbsCJr5m8gHDCNEHCZtFxcXHsD4S9jH83\",\"BookDirectory\":\"6EAB7C172DEFA430DBFAD120FDC373B5F5AF8B191649EC98570C560B764D760C\",\"BookNode\":\"0000000000000000\",\"Flags\":131072,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000001\",\"PreviousTxnID\":\"9A0B6B76F0D86614F965A2FFCC8859D8607F4E424351D4CFE2FBE24510F93F25\",\"PreviousTxnLgrSeq\":10708382,\"Sequence\":110061,\"TakerGets\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.003768001830745216\"},\"TakerPays\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"1.308365894430151\"},\"index\":\"B971769686CE1B9139502770158A4E7C011CFF8E865E5AAE5428E23AAA0E146D\",\"owner_funds\":\"0.2229210189326514\",\"quality\":\"347.2306949944844\"},{\"Account\":\"rDVBvAQScXrGRGnzrxRrcJPeNLeLeUTAqE\",\"BookDirectory\":\"6EAB7C172DEFA430DBFAD120FDC373B5F5AF8B191649EC98570C87DF25DC4FC6\",\"BookNode\":\"0000000000000000\",\"Expiration\":472783298,\"Flags\":131072,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"00000000000003D2\",\"PreviousTxnID\":\"E5F9A10F29A4BB3634D5A84FC96931E17267B58E0D2D5ADE24FFB751E52ADB9E\",\"PreviousTxnLgrSeq\":10713533,\"Sequence\":35788,\"TakerGets\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.5\"},\"TakerPays\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"176.3546101589987\"},\"index\":\"D2CB71038AD0ECAF4B5FF0A953AD1257225D0071E6F3AF9ADE67F05590B45C6E\",\"owner_funds\":\"6.617688680663627\",\"quality\":\"352.7092203179974\"},{\"Account\":\"rN6jbxx4H6NxcnmkzBxQnbCWLECNKrgSSf\",\"BookDirectory\":\"6EAB7C172DEFA430DBFAD120FDC373B5F5AF8B191649EC98570CC0B8E0E2C000\",\"BookNode\":\"0000000000000000\",\"Flags\":131072,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"2E16ACFEAC2306E3B3483D445787F3496FACF9504F7A5E909620C1A73E2EDE54\",\"PreviousTxnLgrSeq\":10558020,\"Sequence\":491,\"TakerGets\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.5\"},\"TakerPays\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"179.48\"},\"index\":\"DA853913C8013C9471957349EDAEE4DF4846833B8CCB92008E2A8994E37BEF0D\",\"owner_funds\":\"0.5\",\"quality\":\"358.96\",\"taker_gets_funded\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.499001996007984\"},\"taker_pays_funded\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"179.1217564870259\"}},{\"Account\":\"rDVBvAQScXrGRGnzrxRrcJPeNLeLeUTAqE\",\"BookDirectory\":\"6EAB7C172DEFA430DBFAD120FDC373B5F5AF8B191649EC98570CD2F24C9C145D\",\"BookNode\":\"0000000000000000\",\"Expiration\":472783299,\"Flags\":131072,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"00000000000003D2\",\"PreviousTxnID\":\"B1B12E47043B4260223A2C4240D19E93526B55B1DB38DEED335DACE7C04FEB23\",\"PreviousTxnLgrSeq\":10713534,\"Sequence\":35789,\"TakerGets\":{\"currency\":\"BTC\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"0.8\"},\"TakerPays\":{\"currency\":\"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\",\"value\":\"288.7710263794967\"},\"index\":\"B89AD580E908F7337CCBB47A0BAAC6417EF13AC3465E34E8B7DD3BED016EA833\",\"quality\":\"360.9637829743709\"}]},\"withXRP\":{\"buy\":[{\"Account\":\"rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh\",\"BookDirectory\":\"A118405CF7C2C89AB0CC084417187B86870DC14325C861A0470E1AEE5CBE20D9\",\"BookNode\":\"0000000000000000\",\"Flags\":0,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"9DD36CC7338FEB9E501A33EAAA4C00DBE4ED3A692704C62DDBD1848EE1F6E762\",\"PreviousTxnLgrSeq\":11,\"Sequence\":5,\"TakerGets\":\"254391353000000\",\"TakerPays\":{\"currency\":\"USD\",\"issuer\":\"rp8rJYTpodf8qbSCHVTNacf8nSW8mRakFw\",\"value\":\"10.1\"},\"index\":\"BF656DABDD84E6128A45039F8D557C9477D4DA31F5B00868F2191F0A11FE3798\",\"owner_funds\":\"99999998959999928\",\"quality\":\"3970260734451929e-29\"}],\"sell\":[{\"Account\":\"rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh\",\"BookDirectory\":\"A118405CF7C2C89AB0CC084417187B86870DC14325C861A0561BB6E89EFF509C\",\"BookNode\":\"0000000000000000\",\"Flags\":131072,\"LedgerEntryType\":\"Offer\",\"OwnerNode\":\"0000000000000000\",\"PreviousTxnID\":\"CFB5786459E568DFC504E7319C515658DED657A7F4EFB5957B33E5E3BD9A1353\",\"PreviousTxnLgrSeq\":13,\"Sequence\":6,\"TakerGets\":{\"currency\":\"USD\",\"issuer\":\"rp8rJYTpodf8qbSCHVTNacf8nSW8mRakFw\",\"value\":\"10453252347.1\"},\"TakerPays\":\"134000000\",\"index\":\"C72CDC1BA4DA529B062871F22C6D175A4D97D4F1160D0D7E646E60699278B5B5\",\"quality\":\"78.0093458738806\"}]}},\"getLedger\":{\"full\":{\"account_hash\":\"2C23D15B6B549123FB351E4B5CDE81C564318EB845449CD43C3EA7953C4DB452\",\"close_time\":\"410424200\",\"close_flags\":0,\"close_time_resolution\":10,\"ledger_hash\":\"E6DB7365949BF9814D76BCC730B01818EB9136A89DB224F3F9F5AAE4569D758E\",\"ledger_index\":38129,\"parent_close_time\":\"410424200\",\"parent_hash\":\"3401E5B2E5D3A53EB0891088A5F2D9364BBB6CE5B37A337D2C0660DAF9C4175E\",\"total_coins\":\"99999999999996310\",\"transaction_hash\":\"DB83BF807416C5B3499A73130F843CF615AB8E797D79FE7D330ADF1BFA93951A\",\"transactions\":[{\"Account\":\"r3kmLJN5D28dHuH8vZNUZpMC43pEHpaocV\",\"Amount\":\"10000000000\",\"Destination\":\"rLQBHVhFnaC5gLEkgr6HgBJJ3bgeZHg9cj\",\"Fee\":\"10\",\"Flags\":0,\"Sequence\":62,\"SigningPubKey\":\"034AADB09CFF4A4804073701EC53C3510CDC95917C2BB0150FB742D0C66E6CEE9E\",\"TransactionType\":\"Payment\",\"TxnSignature\":\"3045022022EB32AECEF7C644C891C19F87966DF9C62B1F34BABA6BE774325E4BB8E2DD62022100A51437898C28C2B297112DF8131F2BB39EA5FE613487DDD611525F1796264639\",\"hash\":\"3B1A4E1C9BB6A7208EB146BCDB86ECEA6068ED01466D933528CA2B4C64F753EF\",\"metaData\":{\"AffectedNodes\":[{\"CreatedNode\":{\"LedgerEntryType\":\"AccountRoot\",\"LedgerIndex\":\"4C6ACBD635B0F07101F7FA25871B0925F8836155462152172755845CE691C49E\",\"NewFields\":{\"Account\":\"rLQBHVhFnaC5gLEkgr6HgBJJ3bgeZHg9cj\",\"Balance\":\"10000000000\",\"Sequence\":1}}},{\"ModifiedNode\":{\"FinalFields\":{\"Account\":\"r3kmLJN5D28dHuH8vZNUZpMC43pEHpaocV\",\"Balance\":\"981481999380\",\"Flags\":0,\"OwnerCount\":0,\"Sequence\":63},\"LedgerEntryType\":\"AccountRoot\",\"LedgerIndex\":\"B33FDD5CF3445E1A7F2BE9B06336BEBD73A5E3EE885D3EF93F7E3E2992E46F1A\",\"PreviousFields\":{\"Balance\":\"991481999390\",\"Sequence\":62},\"PreviousTxnID\":\"2485FDC606352F1B0785DA5DE96FB9DBAF43EB60ECBB01B7F6FA970F512CDA5F\",\"PreviousTxnLgrSeq\":31317}}],\"TransactionIndex\":0,\"TransactionResult\":\"tesSUCCESS\"}}]}},\"sign\":{\"normal\":{\"signedTransaction\":\"12000322800000002400000017201B0086955368400000000000000C732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F54474463044022025464FA5466B6E28EEAD2E2D289A7A36A11EB9B269D211F9C76AB8E8320694E002205D5F99CB56E5A996E5636A0E86D029977BEFA232B7FB64ABA8F6E29DC87A9E89770B6578616D706C652E636F6D81145E7B112523F68D2F5E879DB4EAC51C6698A69304\",\"id\":\"93F6C6CE73C092AA005103223F3A1F557F4C097A2943D96760F6490F04379917\"},\"ticket\":{\"signedTransaction\":\"12000A2400000000202800000001202900000017684000000000002710732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F54474473045022100896CFE083767C88B9539DE2F28894429BC2760865161792D576C090BB93E1EAA02203015D30BC59245C8CFB8A9D78386B91B251DCB946A4C0FAB12A5FA41C202FF3A8114EB1FC04FDA0248FB6DE5BA4235425773D61DF0F3\",\"id\":\"0AC60B1E1F063904D9D9D0E9D03F2E9C8D41BC6FC872D5B8BF87E15BBF9669BB\"},\"escrow\":{\"signedTransaction\":\"12000222800000002400000001201900000002201B0000006668400000000000000C732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F5447446304402204652E8572AEED964451C603EB110AC9945A65E3C5C288D144BB02F259755F6E202205B64E27293248F0650A3F7A4FD66BC16A61F4883AC3ED8EE8A48EF569C06812070102074686973206D757374206861766520333220636861726163746572732E2E2E2E701120712C36933822AD3A3D136C5DF97AA863B69F9CE88B2D6CE6BDD11BFDE290C19D8114B5F762798A53D543A014CAF8B297CFF8F2F937E88214B5F762798A53D543A014CAF8B297CFF8F2F937E8\",\"id\":\"645B7676DF057E4F5E83F970A18B3751B6813807F1030A8D2F482D02DC885106\"},\"signAs\":{\"signedTransaction\":\"120000240000000261400000003B9ACA00684000000000000032730081142E244E6F20104E57C0C60BD823CB312BF10928C78314B5F762798A53D543A014CAF8B297CFF8F2F937E8F3E010732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F54474473045022100B3F8205578C6A68D3BBD27650F5D2E983718D502C250C5147F07B7EDD8E8583E02207B892818BD58E328C2797F15694A505937861586D527849065B582523E390B128114B3263BD0A9BF9DFDBBBBD07F536355FF477BF0E9E1F1\",\"id\":\"D8CF5FC93CFE5E131A34599AFB7CE186A5B8D1B9F069E35F4634AD3B27837E35\"}}}";
        Dictionary<string, dynamic> responseJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(responseFixture);

        //static Wallet wallet;

        //[ClassInitialize]
        //public static void MyClassInitialize()
        //{
        //    wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");
        //}

        [TestMethod]
        public void TestSignSuccessful()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");
            JObject some = requestJson["sign"]["normal"];
            SignatureResult result = wallet.Sign(some.ToObject<Dictionary<string, dynamic>>());
            Assert.AreEqual(result.TxBlob, (string)responseJson["sign"]["normal"]["signedTransaction"]);
            Assert.AreEqual(result.Hash, "93F6C6CE73C092AA005103223F3A1F557F4C097A2943D96760F6490F04379917");
        }

        //[TestMethod]
        //public void TestSignLowercaseHexMemo()
        //{
        //    var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");
        //    Dictionary<string, dynamic> json = new Dictionary<string, dynamic>();
        //    SignatureResult result = wallet.Sign(some.ToObject<Dictionary<string, dynamic>>());
        //    Assert.AreEqual(result.TxBlob, (string)responseJson["sign"]["normal"]["signedTransaction"]);
        //    Assert.AreEqual(result.Hash, "93F6C6CE73C092AA005103223F3A1F557F4C097A2943D96760F6490F04379917");
        //}

        [TestMethod]
        public void TestSignEscrow()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");
            JObject some = requestJson["sign"]["escrow"];
            SignatureResult result = wallet.Sign(some.ToObject<Dictionary<string, dynamic>>());
            Assert.AreEqual(result.TxBlob, (string)responseJson["sign"]["escrow"]["signedTransaction"]);
            Assert.AreEqual(result.Hash, "645B7676DF057E4F5E83F970A18B3751B6813807F1030A8D2F482D02DC885106");
        }

        [TestMethod]
        public void TestSignAs()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");
            JObject some = requestJson["sign"]["signAs"];
            SignatureResult result = wallet.Sign(some.ToObject<Dictionary<string, dynamic>>());
            Assert.AreEqual(result.TxBlob, (string)responseJson["sign"]["signAs"]["signedTransaction"]);
            Assert.AreEqual(result.Hash, "D8CF5FC93CFE5E131A34599AFB7CE186A5B8D1B9F069E35F4634AD3B27837E35");
        }

        //[TestMethod]
        //public void TestSignAsXAddress()
        //{
        //    var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");
        //    JObject some = requestJson["sign"]["signAs"];
        //    SignatureResult result = wallet.Sign(some.ToObject<Dictionary<string, dynamic>>());
        //    Assert.AreEqual(result.TxBlob, (string)responseJson["sign"]["signAs"]["signedTransaction"]);
        //    Assert.AreEqual(result.Hash, "D8CF5FC93CFE5E131A34599AFB7CE186A5B8D1B9F069E35F4634AD3B27837E35");
        //}

        //[TestMethod]
        //public void TestSignAsXAddressMultisign()
        //{
        //    var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");
        //    JObject some = requestJson["sign"]["signAs"];
        //    SignatureResult result = wallet.Sign(some.ToObject<Dictionary<string, dynamic>>());
        //    Assert.AreEqual(result.TxBlob, (string)responseJson["sign"]["signAs"]["signedTransaction"]);
        //    Assert.AreEqual(result.Hash, "D8CF5FC93CFE5E131A34599AFB7CE186A5B8D1B9F069E35F4634AD3B27837E35");
        //}

        [TestMethod]
        public void TestSignPaymentNoFlags()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "Payment" },
                { "Account", "r45Rev1EXGxy2hAUmJPCne97KUE7qyrD3j" },
                { "Destination", "rQ3PTWGLCbPz8ZCicV5tCX3xuymojTng5r" },
                { "Amount", "20000000" },
                { "Sequence", 1 },
                { "Fee", "12" },
            };
            SignatureResult result = wallet.Sign(tx);
            Assert.AreEqual(result.TxBlob, "1200002400000001614000000001312D0068400000000000000C732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F5447446304402201C0A74EE8ECF5ED83734D7171FB65C01D90D67040DEDCC66414BD546CE302B5802205356843841BFFF60D15F5F5F9FB0AB9D66591778140AB2D137FF576D9DEC44BC8114EE3046A5DDF8422C40DDB93F1D522BB4FE6419158314FDB08D07AAA0EB711793A3027304D688E10C3648");
            Assert.AreEqual(result.Hash, "E22186AE9FE477821BF361358174C2B0AC2D3289AA6F7E8C1102B3D270C41204");

            JToken decoded = BinaryCodec.Decode(result.TxBlob);
            Assert.AreEqual(decoded["Flags"], null);
        }

        [TestMethod]
        public void TestSignPaymentSourceDestMin()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "Payment" },
                { "Account", "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59" },
                { "Destination", "rEX4LtGJubaUcMWCJULcy4NVxGT9ZEMVRq" },
                { "Amount", new Dictionary<string, dynamic> {
                   { "currency", "USD" },
                   { "issuer", "rMaa8VLBTjwTJWA2kSme4Sqgphhr6Lr6FH" },
                   { "value", "999999999999999900000000000000000000000000000000000000000000000000000000000000000000000000000000" }
                } },
                { "Flags", 2147614720 },
                { "SendMax", new Dictionary<string, dynamic> {
                   { "currency", "GBP" },
                   { "issuer", "rpat5TmYjDsnFSStmgTumFgXCM9eqsWPro" },
                   { "value", "0.1" }
                } },
                { "DeliverMin", new Dictionary<string, dynamic> {
                   { "currency", "USD" },
                   { "issuer", "rMaa8VLBTjwTJWA2kSme4Sqgphhr6Lr6FH" },
                   { "value", "0.1248548562296331" }
                } },
                { "Sequence", 23 },
                { "LastLedgerSequence", 8820051 },
                { "Fee", "12" },
            };
            SignatureResult result = wallet.Sign(tx);
            Assert.AreEqual(result.TxBlob, "12000022800200002400000017201B0086955361EC6386F26FC0FFFF0000000000000000000000005553440000000000DC596C88BCDE4E818D416FCDEEBF2C8656BADC9A68400000000000000C69D4438D7EA4C6800000000000000000000000000047425000000000000C155FFE99C8C91F67083CEFFDB69EBFE76348CA6AD4446F8C5D8A5E0B0000000000000000000000005553440000000000DC596C88BCDE4E818D416FCDEEBF2C8656BADC9A732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F544744630440220297E0C7670C7DA491E0D649E62C123D988BA93FD7EA1B9141F1D376CDDF902F502205AF1936B22B18BBA7793A88ABEEABADB4CE0E4C3BE583066480F2F476B5ED08E81145E7B112523F68D2F5E879DB4EAC51C6698A6930483149F500E50C2F016CA01945E5A1E5846B61EF2D376");
            Assert.AreEqual(result.Hash, "FB2813E9E673EF56609070A4BA9640FAD0508DA567320AE9D92FB5A356A03D84");

            JToken decoded = BinaryCodec.Decode(result.TxBlob);
            Assert.AreEqual(decoded["Flags"], null);
        }

        [TestMethod]
        public void TestSignInvalidSmallFee()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "Flags", 2147483648 },
                { "TransactionType", "AccountSet" },
                { "Account", "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59" },
                { "Domain", "6578616D706C652E636F6D" },
                { "LastLedgerSequence", 8820051 },
                { "Fee", "1.2" },
                { "Sequence", 23 },
                { "SigningPubKey", "02F89EAEC7667B30F33D0687BBA86C3FE2A08CCA40A9186C5BDE2DAA6FA97A37D8" },
            };
            Assert.ThrowsException<InvalidJsonException>(() => wallet.Sign(tx));
        }

        [TestMethod]
        public void TestSignInvalidLargeFee()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "Flags", 2147483648 },
                { "TransactionType", "AccountSet" },
                { "Account", "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59" },
                { "Domain", "6578616D706C652E636F6D" },
                { "LastLedgerSequence", 8820051 },
                { "Fee", "1123456.7" },
                { "Sequence", 23 },
                { "SigningPubKey", "02F89EAEC7667B30F33D0687BBA86C3FE2A08CCA40A9186C5BDE2DAA6FA97A37D8" },
            };
            Assert.ThrowsException<InvalidJsonException>(() => wallet.Sign(tx));
        }

        [TestMethod]
        public void TestSignTicket()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");
            JObject some = requestJson["sign"]["ticket"];
            SignatureResult result = wallet.Sign(some.ToObject<Dictionary<string, dynamic>>());
            Assert.AreEqual(result.TxBlob, (string)responseJson["sign"]["ticket"]["signedTransaction"]);
            Assert.AreEqual(result.Hash, "0AC60B1E1F063904D9D9D0E9D03F2E9C8D41BC6FC872D5B8BF87E15BBF9669BB");
        }

        [TestMethod]
        public void TestSignPaymentWithPaths()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "Payment" },
                { "Account", "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59" },
                { "Destination", "rEX4LtGJubaUcMWCJULcy4NVxGT9ZEMVRq" },
                { "Amount", new Dictionary<string, dynamic> {
                   { "currency", "USD" },
                   { "issuer", "rMaa8VLBTjwTJWA2kSme4Sqgphhr6Lr6FH" },
                   { "value", "999999999999999900000000000000000000000000000000000000000000000000000000000000000000000000000000" }
                } },
                { "Flags", 2147614720 },
                { "SendMax", "100" },
                { "DeliverMin", new Dictionary<string, dynamic> {
                   { "currency", "USD" },
                   { "issuer", "rVnYNK9yuxBz4uP8zC8LEFokM2nqH3poc" },
                   { "value", "0.00004579644712312366" }
                } },
                //{ "Paths", new Dictionary<string, dynamic> {
                //   { new Dictionary<string, dynamic> {
                //       { "currency", "USD" },
                //       { "issuer", "rVnYNK9yuxBz4uP8zC8LEFokM2nqH3poc" },
                //    } }
                //} },
                { "Sequence", 1 },
                { "LastLedgerSequence", 15696358 },
                { "Fee", "12" },
            };
            SignatureResult result = wallet.Sign(tx);
            Assert.AreEqual(result.TxBlob, "12000022800200002400000001201B00EF81E661EC6386F26FC0FFFF0000000000000000000000005553440000000000054F6F784A58F9EFB0A9EB90B83464F9D166461968400000000000000C6940000000000000646AD3504529A0465E2E0000000000000000000000005553440000000000054F6F784A58F9EFB0A9EB90B83464F9D1664619732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F54474463044022049AD75980A5088EBCD768547E06427736BD8C4396B9BD3762CA8C1341BD7A4F9022060C94071C3BDF99FAB4BEB7C0578D6EBEE083157B470699645CCE4738A41D61081145E7B112523F68D2F5E879DB4EAC51C6698A693048314CA6EDC7A28252DAEA6F2045B24F4D7C333E146170112300000000000000000000000005553440000000000054F6F784A58F9EFB0A9EB90B83464F9D166461900");
            Assert.AreEqual(result.Hash, "71D0B4AA13277B32E2C2E751566BB0106764881B0CAA049905A0EDAC73257745");
        }

        [TestMethod]
        public void TestSignPreparedPayment()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "Payment" },
                { "Account", "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59" },
                { "Destination", "rQ3PTWGLCbPz8ZCicV5tCX3xuymojTng5r" },
                { "Amount", "1" },
                { "Flags", 2147483648 },
                { "Sequence", 23 },
                { "LastLedgerSequence", 8819954 },
                { "Fee", "12" },
            };
            SignatureResult result = wallet.Sign(tx);
            Assert.AreEqual(result.TxBlob, "12000022800000002400000017201B008694F261400000000000000168400000000000000C732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F54474473045022100E8929B68B137AB2AAB1AD3A4BB253883B0C8C318DC8BB39579375751B8E54AC502206893B2D61244AFE777DAC9FA3D9DDAC7780A9810AF4B322D629784FD626B8CE481145E7B112523F68D2F5E879DB4EAC51C6698A693048314FDB08D07AAA0EB711793A3027304D688E10C3648");
            Assert.AreEqual(result.Hash, "AA1D2BDC59E504AA6C2416E864C615FB18042C1AB4457BEB883F7194D8C452B5");
        }

        [TestMethod]
        public void TestSignInvalidAmount()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "Payment" },
                { "Account", "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59" },
                { "Destination", "rQ3PTWGLCbPz8ZCicV5tCX3xuymojTng5r" },
                { "Amount", "1.1234567" },
                { "Sequence", 23 },
                { "LastLedgerSequence", 8819954 },
                { "Fee", "12" },
            };
            Assert.ThrowsException<InvalidJsonException>(() => wallet.Sign(tx));
        }

        static Dictionary<string, dynamic> icPayment = new Dictionary<string, dynamic>
        {
            { "TransactionType", "Payment" },
            { "Account", "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59" },
            { "Destination", "rQ3PTWGLCbPz8ZCicV5tCX3xuymojTng5r" },
            { "Amount", new Dictionary<string, dynamic> {
                { "currency", "foo" },
                { "issuer", "rnURbz5HLbvqEq69b1B4TX6cUTNMmcrBqi" },
                { "value", "123.40" }
            } },
            { "Sequence", 23 },
            { "LastLedgerSequence", 8819954 },
            { "Fee", "12" },
        };

        [TestMethod]
        public void TestSignICLowerCase()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> payment = icPayment;
            payment["Amount"] = new Dictionary<string, dynamic> {
                { "currency", "foo" },
                { "issuer", "rnURbz5HLbvqEq69b1B4TX6cUTNMmcrBqi" },
                { "value", "123.40" }
            };
            SignatureResult result = wallet.Sign(payment);
            Assert.AreEqual(result.TxBlob, "12000022800000002400000017201B008694F261D504625103A72000000000000000000000000000666F6F00000000002E099DD75FDD96EB4A603037844F964832FED86B68400000000000000C732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F54474473045022100D32EBD44F86FB6D0BE239A410B62A73A8B0C26CE3767321913D6FB7BE6FAC2410220430C011C25091DA9CD75E7C99BE406572FBB57B92132E39B4BF873863E744E2E81145E7B112523F68D2F5E879DB4EAC51C6698A693048314FDB08D07AAA0EB711793A3027304D688E10C3648");
            Assert.AreEqual(result.Hash, "F822EA1D7B2A3026E4654A9152896652C3843B5690F8A56C4217CB4690C5C95A");
        }

        [TestMethod]
        public void TestSignStdHex()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> payment = icPayment;
            payment["Amount"] = new Dictionary<string, dynamic> {
                { "currency", "***" },
                { "issuer", "rnURbz5HLbvqEq69b1B4TX6cUTNMmcrBqi" },
                { "value", "123.40" }
            };

            Dictionary<string, dynamic> payment2 = icPayment;
            payment2["Amount"] = new Dictionary<string, dynamic> {
                { "currency", "0000000000000000000000002A2A2A0000000000" },
                { "issuer", "rnURbz5HLbvqEq69b1B4TX6cUTNMmcrBqi" },
                { "value", "123.40" }
            };

            SignatureResult result = wallet.Sign(payment);
            SignatureResult result2 = wallet.Sign(payment2);
            Assert.AreEqual(result.TxBlob, result2.TxBlob);
            Assert.AreEqual(result.Hash, result2.Hash);
        }

        [TestMethod]
        public void TestSignInvalidICXRP()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> payment = icPayment;
            payment["Amount"] = new Dictionary<string, dynamic> {
                { "currency", "XRP" },
                { "issuer", "rnURbz5HLbvqEq69b1B4TX6cUTNMmcrBqi" },
                { "value", "123.40" }
            };
            // TODO: InvalidJsonException should be different
            Assert.ThrowsException<InvalidJsonException>(() => wallet.Sign(payment));
        }

        [TestMethod]
        public void TestSignICXRPHex()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> payment = icPayment;
            payment["Amount"] = new Dictionary<string, dynamic> {
                { "currency", "0000000000000000000000007872700000000000" },
                { "issuer", "rnURbz5HLbvqEq69b1B4TX6cUTNMmcrBqi" },
                { "value", "123.40" }
            };
            SignatureResult result = wallet.Sign(payment);
            Assert.AreEqual(result.TxBlob, "12000022800000002400000017201B008694F261D504625103A7200000000000000000000000000078727000000000002E099DD75FDD96EB4A603037844F964832FED86B68400000000000000C732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F5447446304402202CD2BE27480860765B1B8DB6C499D299734C533F4FFA66317E46D1ADE5181EB7022066D2C65B975A6A9FEE56AB55211D5F2F65D6F988C8280019211874D11771A05D81145E7B112523F68D2F5E879DB4EAC51C6698A693048314FDB08D07AAA0EB711793A3027304D688E10C3648");
            Assert.AreEqual(result.Hash, "1FEAA7894E507E36D73F60DED89852CE28994366879BC7D3D806E4C50D10B1EE");
        }

        [TestMethod]
        public void TestSignICSymbol()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> payment = icPayment;
            payment["Amount"] = new Dictionary<string, dynamic> {
                { "currency", "***" },
                { "issuer", "rnURbz5HLbvqEq69b1B4TX6cUTNMmcrBqi" },
                { "value", "123.40" }
            };
            SignatureResult result = wallet.Sign(payment);
            Assert.AreEqual(result.TxBlob, "12000022800000002400000017201B008694F261D504625103A720000000000000000000000000002A2A2A00000000002E099DD75FDD96EB4A603037844F964832FED86B68400000000000000C732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F54474463044022073E71588750C3D47D7D9A541F00FB897823DA67ED198D0A74404B6FE6D5E4AB5022021BE798D4159F375EBE13D0545F50EE864DF834D5A9F9A31504212156A57934C81145E7B112523F68D2F5E879DB4EAC51C6698A693048314FDB08D07AAA0EB711793A3027304D688E10C3648");
            Assert.AreEqual(result.Hash, "95BF9931C1EA164960FE13A504D5FBAEB1E072C1D291D75B85BA3F22A50346DF");
        }

        [TestMethod]
        public void TestSignICNonStandard()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> payment = icPayment;
            payment["Amount"] = new Dictionary<string, dynamic> {
                { "currency", ":::" },
                { "issuer", "rnURbz5HLbvqEq69b1B4TX6cUTNMmcrBqi" },
                { "value", "123.40" }
            };
            SignatureResult result = wallet.Sign(payment);
            Assert.AreEqual(result.TxBlob, "12000022800000002400000017201B008694F261D504625103A720000000000000000000000000003A3A3A00000000002E099DD75FDD96EB4A603037844F964832FED86B68400000000000000C732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F5447446304402205952993DB235D3A6398E2CB5F91D7F0AD9067F02CB8E62FD335C516B64130F4702206777746CC516F95F39ADDD62CD395AF2F6BAFCCA355B5D23B9B4D9358474A11281145E7B112523F68D2F5E879DB4EAC51C6698A693048314FDB08D07AAA0EB711793A3027304D688E10C3648");
            Assert.AreEqual(result.Hash, "CE80072E6D70932BC7AA698B931BCF97B6CC3DD3984E08DF284B74E8CB4E543A");
        }

        [TestMethod]
        public void TestSignICTrailingZero()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> payment = new Dictionary<string, dynamic>
            {
                { "TransactionType", "Payment" },
                { "Account", "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59" },
                { "Destination", "rQ3PTWGLCbPz8ZCicV5tCX3xuymojTng5r" },
                { "Amount", new Dictionary<string, dynamic> {
                    { "currency", "FOO" },
                    { "issuer", "rnURbz5HLbvqEq69b1B4TX6cUTNMmcrBqi" },
                    { "value", "123.40" }
                } },
                { "Sequence", 23 },
                { "LastLedgerSequence", 8819954 },
                { "Fee", "12" },
            };
            SignatureResult result = wallet.Sign(payment);
            Assert.AreEqual(result.TxBlob, "12000022800000002400000017201B008694F261D504625103A72000000000000000000000000000464F4F00000000002E099DD75FDD96EB4A603037844F964832FED86B68400000000000000C732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F5447446304402206EBFC9B8061C3F82D521506CE62B6BBA99995B2175BFE0E1BC516775932AECEB0220172B9CE9C0EFB3F4870E19B79B4E817DD376E5785F034AB792708F92282C65F781145E7B112523F68D2F5E879DB4EAC51C6698A693048314FDB08D07AAA0EB711793A3027304D688E10C3648");
            Assert.AreEqual(result.Hash, "6235E5A3CC14DB97F75CAE2A4B5AA9B4134B7AD48D7DD8C15473D81631435FE4");
        }

        [TestMethod]
        public void TestSignICTrailingZeros()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> payment = new Dictionary<string, dynamic>
            {
                { "TransactionType", "Payment" },
                { "Account", "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59" },
                { "Destination", "rQ3PTWGLCbPz8ZCicV5tCX3xuymojTng5r" },
                { "Amount", new Dictionary<string, dynamic> {
                    { "currency", "FOO" },
                    { "issuer", "rnURbz5HLbvqEq69b1B4TX6cUTNMmcrBqi" },
                    { "value", "123.000" }
                } },
                { "Flags", 2147483648 },
                { "Sequence", 23 },
                { "LastLedgerSequence", 8819954 },
                { "Fee", "12" },
            };
            SignatureResult result = wallet.Sign(payment);
            Assert.AreEqual(result.TxBlob, "12000022800000002400000017201B008694F261D5045EADB112E000000000000000000000000000464F4F00000000002E099DD75FDD96EB4A603037844F964832FED86B68400000000000000C732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F54474473045022100C0C77D7D6D6453F0C5EDFF61DE60B5D3D6952C8F30D51543560936D72FA103B00220258CBFCEAC4D2DB5CC2B9417EB46225943E9F4B92944B303ADB810002530BFFB81145E7B112523F68D2F5E879DB4EAC51C6698A693048314FDB08D07AAA0EB711793A3027304D688E10C3648");
            Assert.AreEqual(result.Hash, "FADCD5EE33C01103AA129FCF0923637D543DB56250CD57A1A308EC386A211CBB");
        }

        [TestMethod]
        public void TestSignNFTMintLowerURI()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenMint" },
                { "Account", wallet.ClassicAddress },
                { "TransferFee", 314 },
                { "NFTokenTaxon", 0 },
                { "Flags", 8 },
                { "Fee", "10" },
                { "URI", "697066733a2f2f62616679626569676479727a74357366703775646d37687537367568377932366e6634646675796c71616266336f636c67747179353566627a6469" },
                { "Memos", new Dictionary<string, dynamic> {
                    { "Memo", new Dictionary<string, dynamic> {
                        { "MemoType", "687474703a2f2f6578616d706c652e636f6d2f6d656d6f2f67656e65726963" },
                        { "MemoData", "72656e74" },
                    } },
                } },
            };
            SignatureResult result = wallet.Sign(tx);
            Assert.AreEqual(result.TxBlob, "12001914013A2200000008202A0000000068400000000000000A732102A8A44DB3D4C73EEEE11DFE54D2029103B776AA8A8D293A91D645977C9DF5F5447446304402203795B6E9D6D0086FB26E2C6B7A8C02D50B8560D45C9D5C80DF271D3349515E5302203B0898A7D8C06243D7C2116D2011ACB68DF3123BB7336D6C27269FD388C12CC07542697066733A2F2F62616679626569676479727A74357366703775646D37687537367568377932366E6634646675796C71616266336F636C67747179353566627A64698114B3263BD0A9BF9DFDBBBBD07F536355FF477BF0E9F9EA7C1F687474703A2F2F6578616D706C652E636F6D2F6D656D6F2F67656E657269637D0472656E74E1F1");
            Assert.AreEqual(result.Hash, "2F359B3CFD1CE6D7BFB672F8ADCE98FE964B1FD04CFC337177FB3D8FBE889788");
        }

        [TestMethod]
        public void TestSignNFTMintInvalidURI()
        {
            var wallet = Wallet.FromSeed("ss1x3KLrSvfg7irFc1D929WXZ7z9H");

            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenMint" },
                { "Account", wallet.ClassicAddress },
                { "TransferFee", 314 },
                { "NFTokenTaxon", 0 },
                { "Flags", 8 },
                { "Fee", "10" },
                { "URI", "ipfs://bafybeigdyrzt5sfp7udm7hu76uh7y26nf4dfuylqabf3oclgtqy55fbzdi" },
                { "Memos", new Dictionary<string, dynamic> {
                    { "Memo", new Dictionary<string, dynamic> {
                        { "MemoType", "687474703a2f2f6578616d706c652e636f6d2f6d656d6f2f67656e65726963" },
                        { "MemoData", "72656e74" },
                    } },
                } },
            };
            SignatureResult result = wallet.Sign(tx);
        }
    }

    [TestClass]
    public class TestUVerify
    {

        static string publicKey = "030E58CDD076E798C84755590AAF6237CA8FAE821070A59F648B517A30DC6F589D";
        static string privateKey = "00141BA006D3363D2FB2785E8DF4E44D3A49908780CB4FB51F6D217C08C021429F";

        static Dictionary<string, dynamic> prepared = new Dictionary<string, dynamic>
        {
            { "signedTransaction", "1200002400000001614000000001312D0068400000000000000C7321030E58CDD076E798C84755590AAF6237CA8FAE821070A59F648B517A30DC6F589D74473045022100CAF99A63B241F5F62B456C68A593D2835397101533BB5D0C4DC17362AC22046F022016A2CA2CF56E777B10E43B56541A4C2FB553E7E298CDD39F7A8A844DA491E51D81142AF1861DEC1316AEEC995C94FF9E2165B1B784608314FDB08D07AAA0EB711793A3027304D688E10C3648" },
            { "id", "30D9ECA2A7FB568C5A8607E5850D9567572A9E7C6094C26BEFD4DC4C2CF2657A" }
        };

        [TestMethod]
        public void TestVerifySameWallet()
        {

            var wallet = new Wallet(publicKey, privateKey);
            bool isVerified = wallet.VerifyTransaction(prepared["signedTransaction"]);
            Assert.AreEqual(isVerified, true);
        }

        [TestMethod]
        public void TestVerifyDifferentWallet()
        {
            string diffPublicKey = "030E58CDD076E798C84755590AAF6237CA8FAE821070A59F648B517A30DC6F589D";
            string diffPrivateKey = "00141BA006D3363D2FB2785E8DF4E44D3A49908780CB4FB51F6D217C08C021429F";
            var wallet = new Wallet(diffPublicKey, diffPrivateKey);
            bool isVerified = wallet.VerifyTransaction(prepared["signedTransaction"]);
            Assert.AreEqual(isVerified, false);
        }
    }

    [TestClass]
    public class TestUXAddress
    {

        static string publicKey = "030E58CDD076E798C84755590AAF6237CA8FAE821070A59F648B517A30DC6F589D";
        static string privateKey = "00141BA006D3363D2FB2785E8DF4E44D3A49908780CB4FB51F6D217C08C021429F";
        static Wallet wallet = new Wallet(publicKey, privateKey);
        static int tag = 1337;
        static string mainnetXAddress = "X7gJ5YK8abHf2eTPWPFHAAot8Knck11QGqmQ7a6a3Z8PJvk";
        static string testnetXAddress = "T7bq3e7kxYq9pwDz8UZhqAZoEkcRGTXSNr5immvcj3DYRaV";

        [TestMethod]
        public void TestVerifyTestnetProvided()
        {
            string result = wallet.GetXAddress(tag, true);
            Assert.AreEqual(result, testnetXAddress);
        }

        [TestMethod]
        public void TestVerifyMainnetProvided()
        {
            string result = wallet.GetXAddress(tag, false);
            Assert.AreEqual(result, mainnetXAddress);
        }

        [TestMethod]
        public void TestVerifyMainnet()
        {
            string result = wallet.GetXAddress(tag);
            Assert.AreEqual(result, mainnetXAddress);
        }
    }
}

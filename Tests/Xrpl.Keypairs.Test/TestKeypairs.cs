using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using System.Collections.Generic;
using Xrpl.AddressCodec;
using Xrpl.Keypairs;

using static Xrpl.AddressCodec.XrplCodec;
using static Xrpl.AddressCodec.Utils;
using System.Diagnostics;
using System.Text;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-keypairs/test/api-test.js

namespace Xrpl.Keypairs.Tests
{
    [TestClass]
    public class TestUKeypairs
    {

        static string fixtures = "{\"secp256k1\":{\"seed\":\"sp5fghtJtpUorTwvof1NpDXAzNwf5\",\"keypair\":{\"privateKey\":\"00D78B9735C3F26501C7337B8A5727FD53A6EFDBC6AA55984F098488561F985E23\",\"publicKey\":\"030D58EB48B4420B1F7B9DF55087E0E29FEF0E8468F9A6825B01CA2C361042D435\"},\"validatorKeypair\":{\"privateKey\":\"001A6B48BF0DE7C7E425B61E0444E3921182B6529867685257CEDC3E7EF13F0F18\",\"publicKey\":\"03B462771E99AAE9C7912AF47D6120C0B0DA972A4043A17F26320A52056DA46EA8\"},\"address\":\"rU6K7V3Po4snVhBBaU29sesqs2qTQJWDw1\",\"message\":\"test message\",\"signature\":\"30440220583A91C95E54E6A651C47BEC22744E0B101E2C4060E7B08F6341657DAD9BC3EE02207D1489C7395DB0188D3A56A977ECBA54B36FA9371B40319655B1B4429E33EF2D\"},\"ed25519\":{\"seed\":\"sEdSKaCy2JT7JaM7v95H9SxkhP9wS2r\",\"keypair\":{\"privateKey\":\"EDB4C4E046826BD26190D09715FC31F4E6A728204EADD112905B08B14B7F15C4F3\",\"publicKey\":\"ED01FA53FA5A7E77798F882ECE20B1ABC00BB358A9E55A202D0D0676BD0CE37A63\"},\"validatorKeypair\":{\"privateKey\":\"EDB4C4E046826BD26190D09715FC31F4E6A728204EADD112905B08B14B7F15C4F3\",\"publicKey\":\"ED01FA53FA5A7E77798F882ECE20B1ABC00BB358A9E55A202D0D0676BD0CE37A63\"},\"address\":\"rLUEXYuLiQptky37CqLcm9USQpPiz5rkpD\",\"message\":\"test message\",\"signature\":\"CB199E1BFD4E3DAA105E4832EEDFA36413E1F44205E4EFB9E27E826044C21E3E2E848BBC8195E8959BADF887599B7310AD1B7047EF11B682E0D068F73749750E\"}}";
        Dictionary<string, dynamic> apiJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(fixtures);
        byte[] entropy = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        [TestMethod]
        public void TestGenerateSeedSECPRandom()
        {
            string seed = XrplKeypairs.GenerateSeed();
            Assert.AreEqual(seed[0].ToString(), "s");
            DecodedSeed decodedSeed = XrplCodec.DecodeSeed(seed);
            Assert.IsTrue(decodedSeed.Type == "secp256k1");
            Assert.IsTrue(decodedSeed.Bytes.Length == 16);
        }

        [TestMethod]
        public void TestGenerateSeedED()
        {
            Assert.AreEqual(XrplKeypairs.GenerateSeed(entropy, "ed25519").ToString(), (string)apiJson["ed25519"]["seed"]);
        }

        [TestMethod]
        public void TestGenerateSeedEDRandom()
        {
            string seed = XrplKeypairs.GenerateSeed(null, "ed25519");
            Assert.AreEqual(seed[0..3].ToString(), "sEd");
            DecodedSeed decodedSeed = XrplCodec.DecodeSeed(seed);
            Assert.IsTrue(decodedSeed.Type == "ed25519");
            Assert.IsTrue(decodedSeed.Bytes.Length == 16);
        }

        [TestMethod]
        public void TestDeriveKPSECP()
        {
            IXrplKeyPair keypair = XrplKeypairs.DeriveKeypair((string)apiJson["secp256k1"]["seed"]);
            Assert.AreEqual(keypair.Id(), (string)apiJson["secp256k1"]["keypair"]["publicKey"]);
            Assert.AreEqual(keypair.Pk(), (string)apiJson["secp256k1"]["keypair"]["privateKey"]);
        }

        [TestMethod]
        public void TestDeriveKPED()
        {
            IXrplKeyPair keypair = XrplKeypairs.DeriveKeypair((string)apiJson["ed25519"]["seed"]);
            Assert.AreEqual(keypair.Id(), (string)apiJson["ed25519"]["keypair"]["publicKey"]);
            Assert.AreEqual(keypair.Pk(), (string)apiJson["ed25519"]["keypair"]["privateKey"]);
        }

        [TestMethod]
        public void TestDeriveKPValidatorSECP()
        {
            IXrplKeyPair keypair = XrplKeypairs.DeriveKeypair((string)apiJson["secp256k1"]["seed"], null, true);
            Assert.AreEqual(keypair.Id(), (string)apiJson["secp256k1"]["validatorKeypair"]["publicKey"]);
            Assert.AreEqual(keypair.Pk(), (string)apiJson["secp256k1"]["validatorKeypair"]["privateKey"]);
        }

        [TestMethod]
        public void TestDeriveKPValidatorED()
        {
            IXrplKeyPair keypair = XrplKeypairs.DeriveKeypair((string)apiJson["ed25519"]["seed"], null, true);
            Assert.AreEqual(keypair.Id(), (string)apiJson["ed25519"]["validatorKeypair"]["publicKey"]);
            Assert.AreEqual(keypair.Pk(), (string)apiJson["ed25519"]["validatorKeypair"]["privateKey"]);
        }

        [TestMethod]
        public void TestDeriveKPAddressSECP()
        {
            string address = XrplKeypairs.DeriveAddress((string)apiJson["secp256k1"]["keypair"]["publicKey"]);
            Assert.AreEqual(address, (string)apiJson["secp256k1"]["address"]);
        }

        [TestMethod]
        public void TestDeriveKPAddressED()
        {
            string address = XrplKeypairs.DeriveAddress((string)apiJson["ed25519"]["keypair"]["publicKey"]);
            Assert.AreEqual(address, (string)apiJson["ed25519"]["address"]);
        }

        [TestMethod]
        public void TestSignSECP()
        {
            string privateKey = (string)apiJson["secp256k1"]["keypair"]["privateKey"];
            string message = (string)apiJson["secp256k1"]["message"];
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            string signature = XrplKeypairs.Sign(messageBytes, privateKey);
            Assert.AreEqual(signature, (string)apiJson["secp256k1"]["signature"]);
        }

        [TestMethod]
        public void TestVerifySECP()
        {
            string signature = (string)apiJson["secp256k1"]["signature"];
            string publicKey = (string)apiJson["secp256k1"]["keypair"]["publicKey"];
            string message = (string)apiJson["secp256k1"]["message"];
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            bool verified = XrplKeypairs.Verify(messageBytes, signature, publicKey);
            Assert.IsTrue(verified);
        }

        [TestMethod]
        public void TestSignSECP1()
        {
            string privateKey = "00141BA006D3363D2FB2785E8DF4E44D3A49908780CB4FB51F6D217C08C021429F";
            string message = "CF83E1357EEFB8BDF1542850D66D8007D620E4050B5715DC83F4A921D36CE9CE";
            byte[] messageBytes = message.FromHex();
            string signature = XrplKeypairs.Sign(messageBytes, privateKey);
            Assert.AreEqual(signature, "30440220600D60F6FF362A63C9B8484C5911F0B436047AB0FFE37D784BB115FFEF31894402200C87284F7FA540A454D20BD5D3EA1903B8D7AE4E991D7B44290DB30EF707B47D");
        }

        [TestMethod]
        public void TestVerifySECP1()
        {
            string signature = "30440220016DF49D23201FBA4C4D557B6199C6C791D6E985B56C6688108B8FD7CA5D2D1102206F4540727D43604C6992AF2FAE85D89F51763CE8912A545DC73313603E968D09";
            string publicKey = "030E58CDD076E798C84755590AAF6237CA8FAE821070A59F648B517A30DC6F589D";
            string message = "CF83E1357EEFB8BDF1542850D66D8007D620E4050B5715DC83F4A921D36CE9CE";
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            bool verified = XrplKeypairs.Verify(messageBytes, signature, publicKey);
            Assert.IsTrue(verified);
        }

        
        [TestMethod]
        public void TestSignED()
        {
            string privateKey = (string)apiJson["ed25519"]["keypair"]["privateKey"];
            string message = (string)apiJson["ed25519"]["message"];
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            string signature = XrplKeypairs.Sign(messageBytes, privateKey);
            Assert.AreEqual(signature, (string)apiJson["ed25519"]["signature"]);
        }

        [TestMethod]
        public void TestVerifyED()
        {
            string signature = (string)apiJson["ed25519"]["signature"];
            string publicKey = (string)apiJson["ed25519"]["keypair"]["publicKey"];
            string message = (string)apiJson["ed25519"]["message"];
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            bool verified = XrplKeypairs.Verify(messageBytes, signature, publicKey);
            Assert.IsTrue(verified);
        }
    }
}
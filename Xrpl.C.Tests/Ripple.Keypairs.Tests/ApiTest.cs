using System;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Utilities;
using Ripple.Address.Codec;
using static Ripple.Address.Codec.XrplCodec;
using System.Diagnostics;
using Ripple.Keypairs.Ed25519;
using Xrpl.Client.Extensions;
using Org.BouncyCastle.Bcpg.Sig;

//Debug.WriteLine(decodedSeed.Bytes.Length);

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-keypairs/test/api-test.js

namespace Ripple.Keypairs.Tests
{
    [TestClass]
    public class ApiTest
    {

        static string fixtures = "{\"secp256k1\":{\"seed\":\"sp5fghtJtpUorTwvof1NpDXAzNwf5\",\"keypair\":{\"privateKey\":\"00D78B9735C3F26501C7337B8A5727FD53A6EFDBC6AA55984F098488561F985E23\",\"publicKey\":\"030D58EB48B4420B1F7B9DF55087E0E29FEF0E8468F9A6825B01CA2C361042D435\"},\"validatorKeypair\":{\"privateKey\":\"001A6B48BF0DE7C7E425B61E0444E3921182B6529867685257CEDC3E7EF13F0F18\",\"publicKey\":\"03B462771E99AAE9C7912AF47D6120C0B0DA972A4043A17F26320A52056DA46EA8\"},\"address\":\"rU6K7V3Po4snVhBBaU29sesqs2qTQJWDw1\",\"message\":\"test message\",\"signature\":\"30440220583A91C95E54E6A651C47BEC22744E0B101E2C4060E7B08F6341657DAD9BC3EE02207D1489C7395DB0188D3A56A977ECBA54B36FA9371B40319655B1B4429E33EF2D\"},\"ed25519\":{\"seed\":\"sEdSKaCy2JT7JaM7v95H9SxkhP9wS2r\",\"keypair\":{\"privateKey\":\"EDB4C4E046826BD26190D09715FC31F4E6A728204EADD112905B08B14B7F15C4F3\",\"publicKey\":\"ED01FA53FA5A7E77798F882ECE20B1ABC00BB358A9E55A202D0D0676BD0CE37A63\"},\"validatorKeypair\":{\"privateKey\":\"EDB4C4E046826BD26190D09715FC31F4E6A728204EADD112905B08B14B7F15C4F3\",\"publicKey\":\"ED01FA53FA5A7E77798F882ECE20B1ABC00BB358A9E55A202D0D0676BD0CE37A63\"},\"address\":\"rLUEXYuLiQptky37CqLcm9USQpPiz5rkpD\",\"message\":\"test message\",\"signature\":\"CB199E1BFD4E3DAA105E4832EEDFA36413E1F44205E4EFB9E27E826044C21E3E2E848BBC8195E8959BADF887599B7310AD1B7047EF11B682E0D068F73749750E\"}}";
        Dictionary<string, dynamic> apiJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(fixtures);
        byte[] entropy = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        [TestMethod]
        public void TestGenerateSeedSECPRandom()
        {
            string seed = Keypairs.GenerateSeed();
            Assert.AreEqual(seed[0].ToString(), "s");
            DecodedSeed decodedSeed = XrplCodec.DecodeSeed(seed);
            Assert.IsTrue(decodedSeed.Type == "secp256k1");
            Assert.IsTrue(decodedSeed.Bytes.Length == 16);
        }

        [TestMethod]
        public void TestGenerateSeedED()
        {
            Assert.AreEqual(Keypairs.GenerateSeed(entropy, "ed25519").ToString(), (string)apiJson["ed25519"]["seed"]);
        }

        [TestMethod]
        public void TestGenerateSeedEDRandom()
        {
            string seed = Keypairs.GenerateSeed(null, "ed25519");
            Assert.AreEqual(seed[0..3].ToString(), "sEd");
            DecodedSeed decodedSeed = XrplCodec.DecodeSeed(seed);
            Assert.IsTrue(decodedSeed.Type == "ed25519");
            Assert.IsTrue(decodedSeed.Bytes.Length == 16);
        }

        [TestMethod]
        public void TestDeriveKPSECP()
        {
            IKeyPair keypair = Keypairs.DeriveKeypair((string)apiJson["secp256k1"]["seed"]);
            Assert.AreEqual(keypair.Id(), (string)apiJson["secp256k1"]["keypair"]["publicKey"]);
            Assert.AreEqual(keypair.Pk(), (string)apiJson["secp256k1"]["keypair"]["privateKey"]);
        }

        [TestMethod]
        public void TestDeriveKPED()
        {
            IKeyPair keypair = Keypairs.DeriveKeypair((string)apiJson["ed25519"]["seed"]);
            Assert.AreEqual(keypair.Id(), (string)apiJson["ed25519"]["keypair"]["publicKey"]);
            Assert.AreEqual(keypair.Pk(), (string)apiJson["ed25519"]["keypair"]["privateKey"]);
        }

        [TestMethod]
        public void TestDeriveKPValidatorSECP()
        {
            IKeyPair keypair = Keypairs.DeriveKeypair((string)apiJson["secp256k1"]["seed"], null, true);
            Assert.AreEqual(keypair.Id(), (string)apiJson["secp256k1"]["validatorKeypair"]["publicKey"]);
            Assert.AreEqual(keypair.Pk(), (string)apiJson["secp256k1"]["validatorKeypair"]["privateKey"]);
        }

        [TestMethod]
        public void TestDeriveKPValidatorED()
        {
            IKeyPair keypair = Keypairs.DeriveKeypair((string)apiJson["ed25519"]["seed"], null, true);
            Assert.AreEqual(keypair.Id(), (string)apiJson["ed25519"]["validatorKeypair"]["publicKey"]);
            Assert.AreEqual(keypair.Pk(), (string)apiJson["ed25519"]["validatorKeypair"]["privateKey"]);
        }

        [TestMethod]
        public void TestDeriveKPAddressSECP()
        {
            Debug.WriteLine((string)apiJson["secp256k1"]["publicKey"]);
            string address = Keypairs.DeriveAddress((string)apiJson["secp256k1"]["keypair"]["publicKey"]);
            Assert.AreEqual(address, (string)apiJson["secp256k1"]["address"]);
        }

        [TestMethod]
        public void TestDeriveKPAddressED()
        {
            Debug.WriteLine((string)apiJson["ed25519"]["publicKey"]);
            string address = Keypairs.DeriveAddress((string)apiJson["ed25519"]["keypair"]["publicKey"]);
            Assert.AreEqual(address, (string)apiJson["ed25519"]["address"]);
        }

        [TestMethod]
        public void TestSignSECP()
        {
            string privateKey = (string)apiJson["secp256k1"]["keypair"]["privateKey"];
            string message = (string)apiJson["secp256k1"]["message"];
            byte[] messageBytes = Ripple.Address.Codec.Utils.FromHexToBytes(message.ConvertStringToHex());
            string signature = Keypairs.Sign(messageBytes, privateKey);
            Assert.AreEqual(signature, (string)apiJson["secp256k1"]["signature"]);
        }

        [TestMethod]
        public void TestVerifySECP()
        {
            string signature = (string)apiJson["secp256k1"]["signature"];
            string publicKey = (string)apiJson["secp256k1"]["keypair"]["publicKey"];
            string message = (string)apiJson["secp256k1"]["message"];
            byte[] messageBytes = Ripple.Address.Codec.Utils.FromHexToBytes(message.ConvertStringToHex());
            bool verified = Keypairs.Verify(messageBytes, signature, publicKey);
            Assert.AreEqual(signature, (string)apiJson["secp256k1"]["signature"]);
        }

        [TestMethod]
        public void TestSignED()
        {
            string privateKey = (string)apiJson["ed25519"]["keypair"]["privateKey"];
            string message = (string)apiJson["ed25519"]["message"];
            byte[] messageBytes = Ripple.Address.Codec.Utils.FromHexToBytes(message.ConvertStringToHex());
            string signature = Keypairs.Sign(messageBytes, privateKey);
            Assert.AreEqual(signature, (string)apiJson["ed25519"]["signature"]);
        }
    }
}
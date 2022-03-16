using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ripple.Signing.Tests.Properties;

namespace Ripple.Signing.Tests
{
    using System;
    using System.IO;
    using Seed = Seed;

    [TestClass]
    public class DerivationTests
    {
        [TestMethod]
        public void GenerateRootAccountTest()
        {
            var passphrase = "masterpassphrase";
            var encodedSeed = "snoPBrXtMeMyMHUVTgbuqAfg1SUTb";
            var rootAccount = "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh";
            var seed = Seed.FromPassPhrase(passphrase);
            var pair = seed.KeyPair();
            Assert.AreEqual(encodedSeed, seed.ToString());
            Assert.AreEqual(rootAccount, pair.Id());
        }

        [TestMethod]
        public void GenerateNiqEd25519Test()
        {
            var passphrase = "niq";
            var encodedSeed = "sEd7rBGm5kxzauRTAV2hbsNz7N45X91";
            var accountID = "rJZdUusLDtY9NEsGea7ijqhVrXv98rYBYN";

            var idFromSeed = Seed.FromBase58(encodedSeed).KeyPair().Id();
            var seed = Seed.FromPassPhrase(passphrase).SetEd25519();
            var pair = seed.KeyPair();

            Assert.AreEqual(accountID, pair.Id());
            Assert.AreEqual(encodedSeed, seed.ToString());
            Assert.AreEqual(accountID, idFromSeed);
        }

        [TestMethod]
        public void GenerateNodeKeyTest()
        {
            var zeroBytes = new byte[16];
            var pair = new Seed(zeroBytes).SetNodeKey().KeyPair();
            Assert.AreEqual("n9LPxYzbDpWBZ1bC3J3Fdkgqoa3FEhVKCnS8yKp7RFQFwuvd8Q2c", 
                            pair.Id());
        }

        [TestMethod]
        public void Generate100EcdsaAccountIdsFromSecretTest()
        {
            using (var sr = new StringReader(Resources.ecdsa_100))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var s = line.Split(' ');
                    var accountId = s[0];
                    var secret = s[1];

                    var calculatedAccountId = Seed.FromBase58(secret).KeyPair().Id();

                    Assert.AreEqual(accountId, calculatedAccountId);
                }
            }
        }

        [TestMethod]
        [Ignore] // long running test (1-2 hours)        
        public void Generate1MillionEcdsaAccountIdsFromSecretTest()
        {
            using (var sr = new StringReader(Resources.ecdsa_1000000))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var s = line.Split(' ');
                    var accountId = s[0];
                    var secret = s[1];

                    var calculatedAccountId = Seed.FromBase58(secret).KeyPair().Id();

                    Assert.AreEqual(accountId, calculatedAccountId);
                }
            }
        }

        [TestMethod]
        [Ignore] // long running test (<1 hour)
        public void Generate1MillionEd25519AccountIdsFromSecretTest()
        {
            using (var sr = new StringReader(Resources.ed25519_1000000))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var s = line.Split(' ');
                    var accountId = s[0];
                    var secret = s[1];

                    var calculatedAccountId = Seed.FromBase58(secret).KeyPair().Id();

                    Assert.AreEqual(accountId, calculatedAccountId);
                }
            }
        }
    }
}


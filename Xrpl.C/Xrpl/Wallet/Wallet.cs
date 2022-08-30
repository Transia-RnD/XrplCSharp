using System;
using Ripple.Keypairs;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using System.Security.Cryptography.X509Certificates;
using Xrpl.Wallet;
using Ripple.Keypairs.Ed25519;
using Ripple.Keypairs.K256;
using System.Transactions;
using Ripple.Binary.Codec;
using Newtonsoft.Json.Linq;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/Wallet/index.ts

namespace Xrpl.Wallet
{
    public class rWallet
    {

        public static string DEFAULT_ALGORITHM = "ed25519";

        public readonly string PublicKey;
        public readonly string PrivateKey;
        public readonly string ClassicAddress;
        public readonly string Seed;

        public class SignatureResult
        {
            public string TxBlob;
            public string Hash;

            public SignatureResult(string txBlob, string hash)
            {
                TxBlob = txBlob;
                Hash = hash;
            }
        }

        /// <summary>
        /// Creates a new Wallet.
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="privateKey"></param>
        /// <param name="masterAddress"></param>
        /// <param name="seed"></param>
        /// <returns>A new Wallet.</returns>
        public rWallet(string publicKey, string privateKey, string? masterAddress, string? seed)
        {
            this.PublicKey = publicKey;
            this.PrivateKey = privateKey;
            //this.classicAddress = masterAddress ? EnsureClassicAddress(masterAddress) : DeriveAddress(publicKey)
            this.ClassicAddress = masterAddress;
            this.Seed = seed;
        }

        /// <summary>
        /// Generates a new Wallet using a generated seed.
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns>A new Wallet derived from a generated seed.</returns>
        public static rWallet Generate(string algorithm)
        {
            string seed = Keypairs.GenerateSeed(null, algorithm);
            return rWallet.FromSeed(seed, null, algorithm);
        }
        /// <summary>
        /// Derives a wallet from a seed.
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="masterAddress"></param>
        /// <returns>A Wallet derived from a seed.</returns>
        public static rWallet FromSeed(string seed, string? masterAddress, string? algorithm)
        {
            return rWallet.DeriveWallet(seed, masterAddress, algorithm);
        }
        /// <summary>
        /// Derives a wallet from an entropy (array of random numbers).
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="masterAddress"></param>
        /// <returns>A Wallet derived from an entropy.</returns>
        public static rWallet FromEntropy(byte[] entropy, string? masterAddress, string? algorithm)
         {
            string falgorithm = algorithm != null ? algorithm : rWallet.DEFAULT_ALGORITHM;
            string seed = Keypairs.GenerateSeed(null, falgorithm);
            return rWallet.DeriveWallet(seed, masterAddress, falgorithm);
        }

        /// <summary>
        /// Derive a Wallet from a seed.
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="masterAddress"></param>
        /// <returns>A Wallet derived from the seed.</returns>
        private static rWallet DeriveWallet(string seed, string? masterAddress, string? algorithm)
        {
            IKeyPair keypair = Keypairs.DeriveKeypair(seed, algorithm);
            return new rWallet(keypair.Id(), keypair.Id(), seed, masterAddress);
        }

        /// <summary>
        /// Signs a transaction offline.
        /// </summary>
        /// <param name="wallet"></param>
        /// <param name="transaction"></param>
        /// <param name="multisign"></param>
        /// <returns>A Wallet derived from the seed.</returns>
        public SignatureResult Sign(object transaction, bool multisign)
        {
            // OTHER
            //JToken txToSignAndEncode = { "test": ""};
            JToken t = JToken.FromObject(transaction);
            string serialized = BinaryCodec.Encode(t);
            //this.checkTxSerialization(serialized, tx);
            return new SignatureResult("serialized", "hashSignedTx(serialized)");
        }
    }
}
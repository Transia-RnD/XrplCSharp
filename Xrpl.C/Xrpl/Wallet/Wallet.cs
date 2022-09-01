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
using System.Collections.Generic;
using System.Diagnostics;
using Xrpl.Utils.Hashes;
using Xrpl.Client.Exceptions;
using Xrpl.Client.Models.Transactions;
using Org.BouncyCastle.Asn1;

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
            Debug.WriteLine("NEW WALLET");
            this.PublicKey = publicKey;
            this.PrivateKey = privateKey;
            this.ClassicAddress = masterAddress != null ? masterAddress : Keypairs.DeriveAddress(publicKey);
            this.Seed = seed;
            Debug.WriteLine(publicKey);
            Debug.WriteLine(privateKey);
            Debug.WriteLine(masterAddress);
            Debug.WriteLine(seed);
        }

        /// <summary>
        /// Generates a new Wallet using a generated seed.
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns>A new Wallet derived from a generated seed.</returns>
        public static rWallet Generate(string algorithm = "ed25519")
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
            return new rWallet(keypair.Id(), keypair.Pk(), masterAddress, seed);
        }

        /// <summary>
        /// Signs a transaction offline.
        /// </summary>
        /// <param name="wallet"></param>
        /// <param name="transaction"></param>
        /// <param name="multisign"></param>
        /// <returns>A Wallet derived from the seed.</returns>
        public SignatureResult Sign(Dictionary<string, dynamic> transaction, bool multisign, string? signingFor = null)
        {
            string multisignAddress = "";
            //if (signingFor != null && signingFor.starts(with: "X"))
            //{
            //    multisignAddress = signingFor;
            //}
            //else if (multisign)
            //{
            //    multisignAddress = this.ClassicAddress;
            //}

            Dictionary<string, dynamic> tx = transaction;

            if (tx.ContainsKey("TxnSignature") || tx.ContainsKey("Signers"))
            {
                new ValidationError("txJSON must not contain `TxnSignature` or `Signers` properties");
            }

            Debug.WriteLine(this.PublicKey);

            // OTHER
            //JToken txToSignAndEncode = { "test": ""};
            JObject txToSignAndEncode = JToken.FromObject(transaction).ToObject<JObject>();
            txToSignAndEncode.Add("SigningPubKey", multisignAddress != "" ? "" : this.PublicKey);

            string signature = ComputeSignature(txToSignAndEncode.ToObject<Dictionary<string, dynamic>>(), this.PrivateKey);
            txToSignAndEncode.Add("TxnSignature", signature);

            Debug.WriteLine(txToSignAndEncode);
            string serialized = BinaryCodec.Encode(txToSignAndEncode);
            //this.checkTxSerialization(serialized, tx);
            return new SignatureResult(serialized, HashLedger.HashSignedTx(serialized));
        }

        public string ComputeSignature(Dictionary<string, dynamic> transaction, string privateKey, string? signAs = null)
        {
            Debug.WriteLine("FUND: ComputeSignature");
            string encoded = BinaryCodec.EncodeForSigning(transaction);
            return Keypairs.Sign(Ripple.Address.Codec.Utils.FromHexToBytes(encoded), privateKey);
        }

    }
}
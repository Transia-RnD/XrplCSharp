using System;
using Ripple.Keypairs;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using System.Security.Cryptography.X509Certificates;
using Xrpl.XrplWallet;
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
using Org.BouncyCastle.Asn1.Ocsp;
using Ripple.Keypairs.Extensions;
using Ripple.Address.Codec;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/Wallet/index.ts

namespace Xrpl.XrplWallet
{
    public class Wallet
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
        public Wallet(string publicKey, string privateKey, string? masterAddress = null, string? seed = null)
        {
            this.PublicKey = publicKey;
            this.PrivateKey = privateKey;
            this.ClassicAddress = masterAddress != null ? masterAddress : Keypairs.DeriveAddress(publicKey);
            this.Seed = seed;
        }

        /// <summary>
        /// Generates a new Wallet using a generated seed.
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns>A new Wallet derived from a generated seed.</returns>
        public static Wallet Generate(string algorithm = "ed25519")
        {
            string seed = Keypairs.GenerateSeed(null, algorithm);
            return Wallet.FromSeed(seed, null, algorithm);
        }
        /// <summary>
        /// Derives a wallet from a seed.
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="masterAddress"></param>
        /// <returns>A Wallet derived from a seed.</returns>
        public static Wallet FromSeed(string seed, string? masterAddress = null, string? algorithm = null)
        {
            return Wallet.DeriveWallet(seed, masterAddress, algorithm);
        }
        /// <summary>
        /// Derives a wallet from an entropy (array of random numbers).
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="masterAddress"></param>
        /// <returns>A Wallet derived from an entropy.</returns>
        public static Wallet FromEntropy(byte[] entropy, string? masterAddress = null, string? algorithm = null)
        {
            string falgorithm = algorithm != null ? algorithm : Wallet.DEFAULT_ALGORITHM;
            string seed = Keypairs.GenerateSeed(entropy, falgorithm);
            return Wallet.DeriveWallet(seed, masterAddress, falgorithm);
        }

        /// <summary>
        /// Derive a Wallet from a seed.
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="masterAddress"></param>
        /// <returns>A Wallet derived from the seed.</returns>
        private static Wallet DeriveWallet(string seed, string? masterAddress = null, string? algorithm = null)
        {
            IKeyPair keypair = Keypairs.DeriveKeypair(seed, algorithm);
            return new Wallet(keypair.Id(), keypair.Pk(), masterAddress, seed);
        }

        /// <summary>
        /// Signs a transaction offline.
        /// </summary>
        /// <param name="wallet"></param>
        /// <param name="transaction"></param>
        /// <param name="multisign"></param>
        /// <returns>A Wallet derived from the seed.</returns>
        public SignatureResult Sign(Dictionary<string, dynamic> transaction, bool multisign = false, string? signingFor = null)
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

            // OTHER
            //JToken txToSignAndEncode = { "test": ""};
            JObject txToSignAndEncode = JToken.FromObject(transaction).ToObject<JObject>();
            txToSignAndEncode["SigningPubKey"] = multisignAddress != "" ? "" : this.PublicKey;

            string signature = ComputeSignature(txToSignAndEncode.ToObject<Dictionary<string, dynamic>>(), this.PrivateKey);
            txToSignAndEncode.Add("TxnSignature", signature);

            string serialized = BinaryCodec.Encode(txToSignAndEncode);
            //this.checkTxSerialization(serialized, tx);
            return new SignatureResult(serialized, HashLedger.HashSignedTx(serialized));
        }

        public bool VerifyTransaction(string signedTransaction)
        {
            JToken tx = BinaryCodec.Decode(signedTransaction);
            string messageHex = BinaryCodec.EncodeForSigning(tx.ToObject<Dictionary<string, dynamic>>());
            string signature = (string)tx["TxnSignature"];
            return Keypairs.Verify(messageHex.FromHex(), signature, this.PublicKey);
        }

        public string GetXAddress(int tag, bool isTestnet = false)
        {
            return AddressCodec.ClassicAddressToXAddress(this.ClassicAddress, tag, isTestnet);
        }

        public string ComputeSignature(Dictionary<string, dynamic> transaction, string privateKey, string? signAs = null)
        {
            string encoded = BinaryCodec.EncodeForSigning(transaction);
            return Keypairs.Sign(Ripple.Address.Codec.Utils.FromHexToBytes(encoded), privateKey);
        }

    }
}
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Xrpl.AddressCodec;
using Xrpl.BinaryCodec;
using Xrpl.Client.Exceptions;
using Xrpl.Keypairs;
using Xrpl.Utils.Hashes;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/Wallet/index.ts

namespace Xrpl.Wallet
{
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

    public class XrplWallet
    {

        public static string DEFAULT_ALGORITHM = "ed25519";

        public readonly string PublicKey;
        public readonly string PrivateKey;
        public readonly string ClassicAddress;
        public readonly string Seed;

        /// <summary>
        /// Creates a new Wallet.
        /// </summary>
        /// <param name="publicKey">The public key for the account.</param>
        /// <param name="privateKey">The private key used for signing transactions for the account.</param>
        /// <param name="masterAddress">Include if a Wallet uses a Regular Key Pair. It must be the master address of the account.</param>
        /// <param name="seed">The seed used to derive the account keys.</param>
        public XrplWallet(string publicKey, string privateKey, string? masterAddress = null, string? seed = null)
        {
            this.PublicKey = publicKey;
            this.PrivateKey = privateKey;
            this.ClassicAddress = masterAddress != null ? masterAddress : XrplKeypairs.DeriveAddress(publicKey);
            this.Seed = seed;
        }

        /// <summary>
        /// Generates a new Wallet using a generated seed.
        /// </summary>
        /// <param name="algorithm">The digital signature algorithm to generate an address for.</param>
        /// <returns>A new Wallet derived from a generated seed.</returns>
        public static XrplWallet Generate(string algorithm = "ed25519")
        {
            string seed = XrplKeypairs.GenerateSeed(null, algorithm);
            return XrplWallet.FromSeed(seed, null, algorithm);
        }
        /// <summary>
        /// Derives a wallet from a seed.
        /// </summary>
        /// <param name="seed">A string used to generate a keypair (publicKey/privateKey) to derive a wallet.</param>
        /// <param name="algorithm">The digital signature algorithm to generate an address for.</param>
        /// <param name="masterAddress">Include if a Wallet uses a Regular Key Pair. It must be the master address of the account.</param>
        /// <returns>A Wallet derived from a seed.</returns>
        public static XrplWallet FromSeed(string seed, string? masterAddress = null, string? algorithm = null)
        {
            return XrplWallet.DeriveWallet(seed, masterAddress, algorithm);
        }
        /// <summary>
        /// An array of random numbers to generate a seed used to derive a wallet.
        /// </summary>
        /// <param name="algorithm">The digital signature algorithm to generate an address for.</param>
        /// <param name="masterAddress">Include if a Wallet uses a Regular Key Pair. It must be the master address of the account.</param>
        /// <returns>A Wallet derived from an entropy.</returns>
        public static XrplWallet FromEntropy(byte[] entropy, string? masterAddress = null, string? algorithm = null)
        {
            string falgorithm = algorithm != null ? algorithm : XrplWallet.DEFAULT_ALGORITHM;
            string seed = XrplKeypairs.GenerateSeed(entropy, falgorithm);
            return XrplWallet.DeriveWallet(seed, masterAddress, falgorithm);
        }

        /// <summary>
        /// Derive a Wallet from a seed.
        /// </summary>
        /// <param name="seed">The seed used to derive the wallet.</param>
        /// <param name="algorithm">The digital signature algorithm to generate an address for.</param>
        /// <param name="masterAddress">Include if a Wallet uses a Regular Key Pair. It must be the master address of the account.</param>
        /// <returns>A Wallet derived from the seed.</returns>
        private static XrplWallet DeriveWallet(string seed, string? masterAddress = null, string? algorithm = null)
        {
            IXrplKeyPair keypair = XrplKeypairs.DeriveKeypair(seed, algorithm);
            return new XrplWallet(keypair.Id(), keypair.Pk(), masterAddress, seed);
        }

        /// <summary>
        /// Signs a transaction offline.
        /// </summary>
        /// <param name="transaction">A transaction to be signed offline.</param>
        /// <param name="multisign">Specify true/false to use multisign or actual address (classic/x-address) to make multisign tx request.</param>
        /// <param name="signingFor"></param>
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

            string serialized = XrplBinaryCodec.Encode(txToSignAndEncode);
            //this.checkTxSerialization(serialized, tx);
            return new SignatureResult(serialized, HashLedger.HashSignedTx(serialized));
        }

        /// <summary>
        /// Verifies a signed transaction offline.
        /// </summary>
        /// <param name="signedTransaction">A signed transaction (hex string of signTransaction result) to be verified offline.</param>
        /// <returns>Returns true if a signedTransaction is valid.</returns>
        public bool VerifyTransaction(string signedTransaction)
        {
            JToken tx = XrplBinaryCodec.Decode(signedTransaction);
            string messageHex = XrplBinaryCodec.EncodeForSigning(tx.ToObject<Dictionary<string, dynamic>>());
            string signature = (string)tx["TxnSignature"];
            return XrplKeypairs.Verify(messageHex.FromHex(), signature, this.PublicKey);
        }

        public string GetXAddress(int tag, bool isTestnet = false)
        {
            return XrplAddressCodec.ClassicAddressToXAddress(this.ClassicAddress, tag, isTestnet);
        }

        public string ComputeSignature(Dictionary<string, dynamic> transaction, string privateKey, string? signAs = null)
        {
            string encoded = XrplBinaryCodec.EncodeForSigning(transaction);
            return XrplKeypairs.Sign(AddressCodec.Utils.FromHexToBytes(encoded), privateKey);
        }

    }
}
using System.Security.Cryptography;
using Xrpl.AddressCodec;
using Xrpl.Keypairs.Ed25519;
using Xrpl.Keypairs.K256;
using static Xrpl.AddressCodec.XrplCodec;
using static Xrpl.AddressCodec.Utils;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-keypairs/src/index.ts

namespace Xrpl.Keypairs
{
    public static class XrplKeypairs
    {

        /// <summary> Generate random seed bytes for new account </summary>
        /// <returns>byte seed array</returns>
        public static byte[] FromRandom()
        {
            using var rng = RandomNumberGenerator.Create();
            var seed = new byte[16];
            rng.GetBytes(seed);
            return seed;
        }

        /// <summary>
        /// generate string seed from bytes array, if null - random
        /// </summary>
        /// <param name="entropy">seed byte array</param>
        /// <param name="algorithm">seed algorithm, base - secp256k1</param>
        /// <returns></returns>
        public static string GenerateSeed(byte[]? entropy = null, string algorithm = "secp256k1")
        {
            //assert.ok(
            //    !options.entropy || options.entropy.length >= 16,
            //    'entropy too short',
            //  )
            byte[] fentropy = entropy ?? FromRandom();
            return XrplCodec.EncodeSeed(fentropy, algorithm);
        }

        public static IXrplKeyPair DeriveKeypair(string seed, string? algorithm = null, bool validator = false, int index = 0)
        {
            DecodedSeed decoded = XrplCodec.DecodeSeed(seed);
            if (decoded.Type == "ed25519")
            {
                IXrplKeyPair edkp = EdKeyPair.From128Seed(decoded.Bytes);
                //byte[] messageToVerify = hash("This test message should verify.");
                //byte[] signature = method.sign(messageToVerify, keypair.privateKey);
                //if (method.verify(messageToVerify, signature, keypair.publicKey) != true)
                //{
                //        throw ValidationError("derived keypair did not generate verifiable signature");
                //}
                return edkp;
            }
            IXrplKeyPair scpk = K256KeyGenerator.From128Seed(decoded.Bytes, validator ? -1 : (int)index);
            //byte[] messageToVerify = hash("This test message should verify.");
            //byte[] signature = method.sign(messageToVerify, keypair.privateKey);
            //if (method.verify(messageToVerify, signature, keypair.publicKey) != true)
            //{
            //    throw new Error("derived keypair did not generate verifiable signature");
            //}
            return scpk;
        }

        /// <summary> get seed algorithm from seed key </summary>
        /// <param name="key">seed key</param>
        /// <returns></returns>
        public static string GetAlgorithmFromKey(string key)
        {
            byte[] data = FromHexToBytes(key);
            return data.Length == 33 && data[0] == 0xED ? "ed25519" : "secp256k1";
        }

        /// <summary> Sing message </summary>
        /// <param name="message">byte array</param>
        /// <param name="privateKey">private key</param>
        /// <returns></returns>
        public static string Sign(byte[] message, string privateKey)
        {
            string algorithm = GetAlgorithmFromKey(privateKey);
            if (algorithm != "ed25519") 
                return K256KeyPair.Sign(message, privateKey.FromHex()).ToHex();

            byte[] pk = Chaos.NaCl.Ed25519.ExpandedPrivateKeyFromSeed(privateKey[2..66].FromHex());
            return EdKeyPair.Sign(message, pk).ToHex();
        }

        public static bool Verify(byte[] message, string signature, string publicKey)
        {
            string algorithm = GetAlgorithmFromKey(publicKey);
            return algorithm == "ed25519"
                ? EdKeyPair.Verify(signature.FromHex(), message, publicKey[2..66].FromHex())
                : K256VerifyingKey.Verify(signature.FromHex(), message, publicKey.FromHex());
        }

        public static string DeriveAddressFromBytes(byte[] publicKeyBytes)
            => XrplCodec.EncodeAccountID(Utils.HashUtils.PublicKeyHash(publicKeyBytes));

        public static string DeriveAddress(string publicKey)
            => XrplKeypairs.DeriveAddressFromBytes(FromHexToBytes(publicKey));
    }
}
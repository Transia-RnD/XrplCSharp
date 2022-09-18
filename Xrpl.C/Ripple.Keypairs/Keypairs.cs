using System.Security.Cryptography;
using Ripple.Address.Codec;
using Ripple.Keypairs.Ed25519;
using Ripple.Keypairs.K256;
using static Ripple.Address.Codec.XrplCodec;
using Ripple.Keypairs.Extensions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-keypairs/src/index.ts

namespace Ripple.Keypairs
{
    public class Keypairs
    {

        public static byte[] FromRandom()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var seed = new byte[16];
                rng.GetBytes(seed);
                return seed;
            }
        }

        public static string GenerateSeed(byte[]? entropy = null, string algorithm = "secp256k1")
        {
            //assert.ok(
            //    !options.entropy || options.entropy.length >= 16,
            //    'entropy too short',
            //  )
            byte[] fentropy = entropy != null ? entropy : FromRandom();
            return XrplCodec.EncodeSeed(fentropy, algorithm);
        }

        public static IKeyPair DeriveKeypair(string seed, string? algorithm = null, bool validator = false, int index = 0)
        {
            DecodedSeed decoded = XrplCodec.DecodeSeed(seed);
            if (decoded.Type == "ed25519")
            {
                IKeyPair edkp = EdKeyPair.From128Seed(decoded.Bytes);
                //byte[] messageToVerify = hash("This test message should verify.");
                //byte[] signature = method.sign(messageToVerify, keypair.privateKey);
                //if (method.verify(messageToVerify, signature, keypair.publicKey) != true)
                //{
                //        throw ValidationError("derived keypair did not generate verifiable signature");
                //}
                return edkp;
            }
            IKeyPair scpk = K256KeyGenerator.From128Seed(decoded.Bytes, validator ? -1 : (int)index);
            //byte[] messageToVerify = hash("This test message should verify.");
            //byte[] signature = method.sign(messageToVerify, keypair.privateKey);
            //if (method.verify(messageToVerify, signature, keypair.publicKey) != true)
            //{
            //    throw new Error("derived keypair did not generate verifiable signature");
            //}
            return scpk;
        }

        public static string GetAlgorithmFromKey(string key)
        {
            byte[] data = Ripple.Address.Codec.Utils.FromHexToBytes(key);
            return data.Length == 33 && data[0] == 0xED ? "ed25519" : "secp256k1";
        }

        public static string Sign(byte[] message, string privateKey)
        {
            string algorithm = GetAlgorithmFromKey(privateKey);
            if (algorithm == "ed25519")
            {
                byte[] pk = Chaos.NaCl.Ed25519.ExpandedPrivateKeyFromSeed(privateKey[2..66].FromHex());
                return EdKeyPair.Sign(message, pk).ToHex();
            }
            return K256KeyPair.Sign(message, privateKey.FromHex()).ToHex();
        }

        public static bool Verify(byte[] message, string signature, string publicKey)
        {
            string algorithm = GetAlgorithmFromKey(publicKey);
            if (algorithm == "ed25519")
            {

                return EdKeyPair.Verify(signature.FromHex(), message, publicKey[2..66].FromHex());
            }
            return K256VerifyingKey.Verify(signature.FromHex(), message, publicKey.FromHex());
        }

        public static string DeriveAddressFromBytes(byte[] publicKeyBytes)
        {
            return XrplCodec.EncodeAccountID(Ripple.Keypairs.Utils.HashUtils.PublicKeyHash(publicKeyBytes));
        }

        public static string DeriveAddress(string publicKey)
        {
            return Keypairs.DeriveAddressFromBytes(Ripple.Address.Codec.Utils.FromHexToBytes(publicKey));
        }
    }
}
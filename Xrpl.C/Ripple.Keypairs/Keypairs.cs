using System;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.Ocsp;
using Ripple.Address.Codec;
using Ripple.Keypairs;
using Ripple.Keypairs.Ed25519;
using Ripple.Keypairs.K256;
using static Ripple.Address.Codec.B58;
using static Ripple.Address.Codec.XrplCodec;
using Ripple.Keypairs.Utils;

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

        public static string GenerateSeed(byte[]? entropy, string algorithm)
        {
            //assert.ok(
            //    !options.entropy || options.entropy.length >= 16,
            //    'entropy too short',
            //  )
            byte[] fentropy = entropy != null ? entropy : FromRandom();
            return XrplCodec.EncodeSeed(fentropy, algorithm);
        }

        public static IKeyPair DeriveKeypair(string seed, string algorithm)
        {
            DecodedSeed decoded = XrplCodec.DecodeSeed(seed);
            if (algorithm == "ed25519")
            {
                IKeyPair edkp = EdKeyPair.From128Seed(decoded.Bytes);
                //byte[] messageToVerify = hash("This test message should verify.");
                //byte[] signature = method.sign(messageToVerify, keypair.privateKey);
                //if (method.verify(messageToVerify, signature, keypair.publicKey) != true)
                //{
                //    throw new Error("derived keypair did not generate verifiable signature");
                //}
                return edkp;
            }
            IKeyPair scpk = K256KeyGenerator.From128Seed(decoded.Bytes, 0);
            //byte[] messageToVerify = hash("This test message should verify.");
            //byte[] signature = method.sign(messageToVerify, keypair.privateKey);
            //if (method.verify(messageToVerify, signature, keypair.publicKey) != true)
            //{
            //    throw new Error("derived keypair did not generate verifiable signature");
            //}
            return scpk;
        }

        public static string DeriveAddressFromBytes(byte[] publicKeyBytes)
        {
            return XrplCodec.EncodeAccountID(Ripple.Keypairs.Utils.HashUtils.PublicKeyHash(publicKeyBytes));
        }
        public static string DeriveAddress(IKeyPair keypair)
        {
            return Keypairs.DeriveAddressFromBytes(keypair.CanonicalPubBytes());
        }
    }
}
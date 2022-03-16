using Org.BouncyCastle.Utilities.Encoders;
using Ripple.Signing.Utils;

namespace Ripple.Signing
{
    public interface IKeyPair
    {
        byte[] CanonicalPubBytes();

        bool Verify(byte[] message, byte[] signature);
        byte[] Sign(byte[] message);

        string Id();
    }

    public static class KeyPairExtensions
    {
        public static byte[] PubKeyHash(this IKeyPair pair)
        {
            return HashUtils.PublicKeyHash(pair.CanonicalPubBytes());
        }
    }
}

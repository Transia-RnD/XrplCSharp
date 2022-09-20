using Org.BouncyCastle.Utilities.Encoders;
using Ripple.Keypairs.Utils;

namespace Ripple.Keypairs
{

    public class rKeypair
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

    public interface IKeyPair
    {
        byte[] CanonicalPubBytes();

        string Id();
        string Pk();
    }

    public static class KeyPairExtensions
    {
        public static byte[] PubKeyHash(this IKeyPair pair)
        {
            return HashUtils.PublicKeyHash(pair.CanonicalPubBytes());
        }
    }
}

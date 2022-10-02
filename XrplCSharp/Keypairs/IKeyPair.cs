using Xrpl.KeypairsLib.Utils;

namespace Xrpl.KeypairsLib
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

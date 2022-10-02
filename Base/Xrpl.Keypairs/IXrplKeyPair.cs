using Xrpl.Keypairs.Utils;

namespace Xrpl.Keypairs
{

    public class rKeypair
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

    public interface IXrplKeyPair
    {
        byte[] CanonicalPubBytes();

        string Id();
        string Pk();
    }

    public static class KeyPairExtensions
    {
        public static byte[] PubKeyHash(this IXrplKeyPair pair)
        {
            return HashUtils.PublicKeyHash(pair.CanonicalPubBytes());
        }
    }
}

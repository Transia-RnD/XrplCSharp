using Ripple.Binary.Codec.Hashing;
using Ripple.Binary.Codec.Util;

namespace Xrpl.Wallet
{
    internal static class Utils
    {
        internal static string TransactionId(byte[] txBlob)
        {
            var hash = TransactionIdBytes(txBlob);
            return B16.Encode(hash);
        }

        public static byte[] TransactionIdBytes(byte[] txBlob)
        {
            return Sha512.Half(input: txBlob,
                               prefix: (uint) HashPrefix.TransactionId);
        }
    }
}
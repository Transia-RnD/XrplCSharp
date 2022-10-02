using System.Security.Cryptography;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-keypairs/src/utils.ts

namespace Xrpl.Keypairs.Utils
{
    internal class HashUtils
    {
        public static byte[] PublicKeyHash(byte[] bytes)
        {
            var hash = SHA256.Create();
            var riper = RIPEMD160.Create();
            bytes = hash.ComputeHash(bytes, 0, bytes.Length);
            return riper.ComputeHash(bytes, 0, bytes.Length);
        }
    }
}
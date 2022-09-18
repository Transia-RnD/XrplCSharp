using System;
using Org.BouncyCastle.Utilities.Encoders;
using System.Linq;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-address-codec/src/utils.ts

namespace Ripple.Address.Codec
{
    public class Utils
    {
        public static string FromBytesToHex(byte[] bytes)
        {
            return Hex.ToHexString(bytes).ToUpper();
        }

        public static byte[] FromHexToBytes(string hex)
        {
            return Hex.Decode(hex);
        }

        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}

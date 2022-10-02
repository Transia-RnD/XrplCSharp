using Org.BouncyCastle.Utilities.Encoders;

using System.Linq;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-address-codec/src/utils.ts

namespace Xrpl.AddressCodecLib
{
    public class Utils
    {
        /// <summary>
        /// from bytes array to hex row
        /// </summary>
        /// <param name="bytes">bytes array</param>
        /// <returns></returns>
        public static string FromBytesToHex(byte[] bytes) => Hex.ToHexString(bytes).ToUpper();

        /// <summary>
        /// hex row to bytes array
        /// </summary>
        /// <param name="hex">hex row</param>
        /// <returns></returns>
        public static byte[] FromHexToBytes(string hex) => Hex.Decode(hex);

        /// <summary>
        /// combine bytes arrays to single array
        /// </summary>
        /// <param name="arrays">bytes arrays</param>
        /// <returns></returns>
        public static byte[] Combine(params byte[][] arrays)
        {
            var rv = new byte[arrays.Sum(a => a.Length)];
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

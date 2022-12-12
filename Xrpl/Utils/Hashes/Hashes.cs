using System.Diagnostics;
using Org.BouncyCastle.Utilities.Encoders;
using Xrpl.AddressCodec;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/index.ts

namespace Xrpl.Utils.Hashes
{
    public class Hashes
    {
        const int HEX = 16;
        const int BYTE_LENGTH = 4;

        public static string AddressToHex(string address)
        {
            return XrplCodec.DecodeAccountID(address).ToHex();
        }

        public static string LedgerSpaceHex(LedgerSpace name)
        {
            return ((int)name).ToString("X4");
        }

        public static string HashPaymentChannel(string address, string dstAddress, int sequence)
        {
            return Sha512HalfUtil.Sha512Half(
              LedgerSpaceHex(LedgerSpace.Paychan) +
                AddressToHex(address) +
                AddressToHex(dstAddress) +
                sequence.ToString("X").PadLeft(BYTE_LENGTH * 2, '0')
            );
        }
    }
}


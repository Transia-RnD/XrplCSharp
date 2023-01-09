
// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/parseNFTokenID.ts

using System.Diagnostics;
using Xrpl.AddressCodec;
using Xrpl.BinaryCodec.Util;
using Xrpl.BinaryCodec.Hashing;

namespace Xrpl.Utils
{
    public class ParseNFTID
    {
        public ParseNFTID()
        {
        }

        public static string ParseNFTOffer(string account, uint sequence)
        {

            byte[] PREFIX = { 0x00, 0x71 };
            byte[] accountBytes = XrplCodec.DecodeAccountID(account);
            byte[] sequenceBytes = Bits.GetBytes(sequence);

            byte[] rv = new byte[PREFIX.Length + accountBytes.Length + sequenceBytes.Length];
            System.Buffer.BlockCopy(PREFIX, 0, rv, 0, PREFIX.Length);
            System.Buffer.BlockCopy(accountBytes, 0, rv, PREFIX.Length, accountBytes.Length);
            System.Buffer.BlockCopy(sequenceBytes, 0, rv, PREFIX.Length + accountBytes.Length, sequenceBytes.Length);
            byte[] response = Sha512.Half(rv);
            return response.ToHex();
        }
    }
}

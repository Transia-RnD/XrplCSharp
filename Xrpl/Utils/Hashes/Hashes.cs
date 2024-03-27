using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

using Xrpl.AddressCodec;
using Xrpl.Client.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/index.ts

namespace Xrpl.Utils.Hashes
{
    //todo double need check
    public static class Hashes
    {
        const int HEX = 16;
        const int BYTE_LENGTH = 4;
        const byte MASK = 0xff;
        public static string AddressToHex(string address)
        {
            return XrplCodec.DecodeAccountID(address).ToHex();
        }

        public static string LedgerSpaceHex(LedgerSpace name)
        {
            return ((int)name).ToString("X4");
        }

        public static string LedgerSpaceHex(string name)
        {
            var enums = Enum.GetValues(typeof(LedgerSpace)).Cast<LedgerSpace>().ToList();

            var res = enums.FirstOrDefault(f => f.ToString() == name).ToString();
            var val = Convert.ToString(res.ToCharArray(0, 1)[0], HEX);
            while (val.Length < 4)
                val = "0" + val;
            return val;
        }
        /// <summary>
        /// check currency code for HEX 
        /// </summary>
        /// <param name="code">currency code</param>
        /// <returns></returns>
        public static bool IsHexCurrencyCode(this string code) => Regex.IsMatch(code, @"[0-9a-fA-F]{40}", RegexOptions.IgnoreCase);

        public static string CurrencyToHex(string currency)
        {
            var cur_code = currency.Trim();
            if (cur_code.Length <= 3)
                return cur_code;

            if (cur_code.IsHexCurrencyCode())
                return cur_code;

            cur_code = cur_code.ConvertStringToHex();

            if (cur_code.Length > 40)
                throw new XrplException("wrong currency code format");

            cur_code += new string('0', 40 - cur_code.Length);

            return cur_code;

        }
        /// <summary>
        ///  Hash the given binary transaction data with the single-signing prefix.<br/>
        /// See [Serialization Format](https://xrpl.org/serialization.html).
        /// </summary>
        /// <param name="txBlobHex">The binary transaction blob as a hexadecimal string.</param>
        /// <returns>The hash to sign.</returns>
        public static string HashTx(string txBlobHex)
        {

            var prefix = HashPrefix.TRANSACTION_SIGN.ToString("X").ToUpper();
            return (prefix + txBlobHex).Sha512Half();
        }


        public static string HashPaymentChannel(string address, string dstAddress, int sequence)
        {
            return (LedgerSpaceHex(LedgerSpace.Paychan) +
                    AddressToHex(address) +
                    AddressToHex(dstAddress) +
                    sequence.ToString("X").PadLeft(BYTE_LENGTH * 2, '0')).Sha512Half();
        }

        public static string HashTX(string txBlobHex)
        {
            string prefix = ((int)HashPrefix.TRANSACTION_SIGN).ToString("X").ToUpper();
            return (prefix + txBlobHex).Sha512Half();
        }

        public static string HashAccountRoot(string address)
        {
            return (LedgerSpaceHex(LedgerSpace.Account) + AddressToHex(address)).Sha512Half();
        }

        public static string HashSignerListId(string address)
        {
            return (LedgerSpaceHex(LedgerSpace.SignerList) + AddressToHex(address) + "00000000").Sha512Half();
        }

        public static string HashOfferId(string address, int sequence)
        {

            string hexPrefix = LedgerSpaceHex(LedgerSpace.Offer).PadLeft(2, '0');
            string hexSequence = sequence.ToString("X").PadLeft(8, '0');
            string prefix = "00" + hexPrefix;
            return (prefix + AddressToHex(address) + hexSequence).Sha512Half();
        }

        public static string HashTrustline(string address1, string address2, string currency)
        {
            string address1Hex = AddressToHex(address1);
            string address2Hex = AddressToHex(address2);

            bool swap = (BigInteger.Parse(address1Hex, NumberStyles.HexNumber)>(BigInteger.Parse(address2Hex, NumberStyles.HexNumber)));
            string lowAddressHex = swap ? address2Hex : address1Hex;
            string highAddressHex = swap ? address1Hex : address2Hex;

            string prefix = LedgerSpaceHex(LedgerSpace.RippleState);
            return (prefix + lowAddressHex + highAddressHex + CurrencyToHex(currency)).Sha512Half();
        }

        public static string HashEscrow(string address, int sequence)
        {
            return (LedgerSpaceHex(LedgerSpace.Escrow) + AddressToHex(address) + sequence.ToString("X").PadLeft(BYTE_LENGTH * 2, '0')).Sha512Half();
        }

    }
}


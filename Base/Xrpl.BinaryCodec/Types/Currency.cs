using System;
using System.Text.RegularExpressions;
using System.Linq;
using Xrpl.BinaryCodec.Enums;
using Xrpl.BinaryCodec.Types;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/currency.ts

namespace Xrpl.BinaryCodec.Types
{
    public class Currency : Hash160
    {
        public static readonly Currency XRP = new Currency(new byte[20]);

        private readonly string _iso;

        public Currency(byte[] bytes) : base(bytes)
        {
            var hex = Bytes.ToHex();
            if (Regex.IsMatch(hex, "^0{40}$"))
            {
                _iso = "XRP";
            }
            else if (Regex.IsMatch(hex, "^0{24}[\x00-\x7F]{6}0{10}$"))
            {
                _iso = IsoCodeFromHex(Bytes.Slice(12, 15));
            }
            else
            {
                _iso = null;
            }
        }

        public string Iso()
        {
            return _iso;
        }

        public static Currency From(Hash160 value)
        {
            if (value is Currency)
            {
                return (Currency)value;
            }

            if (value is string)
            {
                return new Currency(BytesFromRepresentation((string)value));
            }

            throw new Exception("Cannot construct Currency from value given");
        }

        public string ToJson()
        {
            var iso = Iso();
            if (iso != null)
            {
                return iso;
            }
            return Bytes.ToHex().ToUpper();
        }

        private static byte[] BytesFromRepresentation(string input)
        {
            if (!IsValidRepresentation(input))
            {
                throw new Exception($"Unsupported Currency representation: {input}");
            }
            return input.Length == 3 ? IsoToBytes(input) : input.HexToBytes();
        }

        private static bool IsValidRepresentation(string input)
        {
            return input.Length == 3 || IsHex(input);
        }

        private static bool IsHex(string hex)
        {
            return Regex.IsMatch(hex, "^[A-F0-9]{40}$");
        }

        private static byte[] IsoToBytes(string iso)
        {
            var bytes = new byte[20];
            if (iso != "XRP")
            {
                var isoBytes = iso.Split("").Map(c => c.CharCodeAt(0));
                bytes.Set(isoBytes, 12);
            }
            return bytes;
        }

        private static string IsoCodeFromHex(byte[] code)
        {
            var iso = code.ToString();
            if (iso == "XRP")
            {
                return null;
            }
            if (IsIsoCode(iso))
            {
                return iso;
            }
            return null;
        }

        private static bool IsIsoCode(string iso)
        {
            return Regex.IsMatch(iso, "^[A-Z0-9a-z?!@#$%^&*(){}[\\]|]{3}$");
        }
    }
}
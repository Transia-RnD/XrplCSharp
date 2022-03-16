using System;
using System.Text.RegularExpressions;
using Ripple.Core.Binary;
using Ripple.Core.Util;

namespace Ripple.Core.Types
{
    public abstract class AmountValue
    {
        public new abstract string ToString();
        public abstract byte[] ToBytes();

        public static AmountValue FromString(string value, bool native=false)
        {
            if (native)
            {
                return new NativeValue(value);
            }
            return IouValue.FromString(value);
        }

        public abstract bool IsIou { get;}

        public static AmountValue FromParser(BinaryParser parser)
        {
            AmountValue value;
            var mantissa = parser.Read(8);
            var b1 = mantissa[0];
            var b2 = mantissa[1];

            var isIou = (b1 & 0x80) != 0;
            var isPositive = (b1 & 0x40) != 0;
            var sign = isPositive ? 1 : -1;

            if (isIou)
            {
                mantissa[0] = 0;
                var exponent = ((b1 & 0x3F) << 2) + ((b2 & 0xff) >> 6) - 97;
                mantissa[1] &= 0x3F;
                value = new IouValue(mantissa, sign, exponent);
            }
            else
            {
                mantissa[0] &= 0x3F;
                value = new NativeValue(mantissa, sign);
            }
            return value;
        }

        internal static ulong ParseMantissa(byte[] mantissa)
        {
            if (mantissa.Length > 8)
            {
                throw new PrecisionException("Encoded mantissa must be only 8 bytes maximum");
            }
            return Bits.ToUInt64(mantissa, 0);
        }
    }

    public class IouValue : AmountValue
    {
        public readonly ulong Mantissa;
        public readonly bool IsNegative;

        public bool IsZero => Mantissa == 0;
        public readonly int Exponent;
        public readonly int Precision;

        public const int MinExponent = -96;
        public const int MaxExponent = 80;
        public const int MaxPrecision = 16;

        public const string ValueRegex = @"^([-+])?(\d+)?(\.(\d+))?([eE]([+-]?\d+))?$";
        public static readonly ulong MinMantissa = Ul("1000,0000,0000,0000");
        public static readonly ulong MaxMantissa = Ul("9999,9999,9999,9999");

        public const string IllegalOfferMantissaString = "1000000000000000100";
        public static readonly IouValue IllegalOffer = new IouValue(ulong.Parse(IllegalOfferMantissaString),
                0,
                false,
                17,
                false);

        public IouValue(ulong mantissa,
                           int exponent,
                           bool isNegative,
                           int? precision=null,
                           bool normalise=true)
        {
            Mantissa = mantissa;
            Exponent = exponent;
            if (normalise)
            {
                Normalize(ref Mantissa, ref Exponent);
            }
            IsNegative = isNegative;
            Precision = precision ?? (IsZero ? 1 :
                                      Mantissa.ToString()
                                              .Trim('0')
                                              .Length);

        }

        public IouValue(byte[] mantissa, int sign, int exponent=0) :
                this(ParseMantissa(mantissa), exponent, sign == -1) {}

        public static IouValue FromString(string value)
        {
            var match = Regex.Match(value, ValueRegex);

            if (!match.Success)
            {
                throw new InvalidAmountValueException($"Value {value} must match {ValueRegex}");
            }

            var signGroup = match.Groups[1];
            var numberGroup = match.Groups[2];
            var fractionGroup = match.Groups[4];
            var exponentGroup = match.Groups[6];

            var exponent = 0;
            var mantissaString = numberGroup.Success ? numberGroup.Value : "";
            if (fractionGroup.Success)
            {
                var fraction = fractionGroup.Value;
                mantissaString += fraction;
                exponent -= fraction.Length;
            }
            if (exponentGroup.Success)
            {
                exponent += int.Parse(exponentGroup.Value);
            }

            mantissaString = mantissaString.TrimStart('0');
            var trimmed = mantissaString.TrimEnd('0');
            if (trimmed.Length == 0)
            {
                trimmed = "0";
                mantissaString = "0";
            }
            exponent += mantissaString.Length - trimmed.Length;

            var mantissa = ulong.Parse(trimmed);
            var precision = trimmed.Length;
            var isNegative = signGroup.Success && signGroup.Value == "-";

            if (precision > MaxPrecision)
            {

                throw new PrecisionException();
            }

            return new IouValue(
                mantissa,
                exponent,
                isNegative,
                precision);
        }

        public override string ToString()
        {
            if (Mantissa == 0)
            {
                return "0";
            }
            var e = 16 - Precision + Exponent;
            var str = Mantissa.ToString().Substring(0, Precision);
            while (e > 0)
            {
                str += 0;
                e--;
            }
            var decimalPos = 0;
            while (e <= 0)
            {
                decimalPos++;
                e++;
            }
            while (decimalPos > str.Length)
            {
                str = "0" + str;
            }
            if (decimalPos != 0)
            {
                var startIndex = str.Length + 1 - decimalPos;
                if (startIndex != str.Length)
                {
                    str = str.Insert(startIndex, ".");
                }
            }
            if (IsNegative)
            {
                str = "-" + str;
            }
            return str;
        }

        private static void Normalize(ref ulong mantissa, ref int exponent)
        {
            if (mantissa == 0) return;
            while (mantissa < MinMantissa)
            {
                mantissa *= 10;
                exponent -= 1;
            }
            while (mantissa > MaxMantissa)
            {
                mantissa /= 10;
                exponent += 1;
            }
            if (exponent > MaxExponent || exponent < MinExponent)
            {
                throw new PrecisionException();
            }
        }

        private static ulong Ul(string s)
        {
            return ulong.Parse(s.Replace(",", ""));
        }

        public override byte[] ToBytes()
        {
            var notNegative = !IsNegative;
            var mantissa = MantissaBytes();

            // Set the top bit for IOU
            mantissa[0] |= 0x80;
            if (IsZero) return mantissa;

            if (notNegative)
            {
                mantissa[0] |= 0x40;
            }

            var exponent = Exponent;
            var exponentByte = 97 + exponent;
            mantissa[0] |= (byte) (exponentByte >> 2);
            mantissa[1] |= (byte) ((exponentByte & 0x03) << 6);
            return mantissa;
        }

        public override bool IsIou => true;

        public byte[] MantissaBytes()
        {
            return Bits.GetBytes(Mantissa);
        }
    }

    internal class NativeValue : AmountValue
    {
        public bool IsNegative;
        public ulong Mantissa;

        public override bool IsIou => false;

        public NativeValue(string value)
        {
            var parsed = long.Parse(value);
            IsNegative = parsed < 0;
            Mantissa = (ulong) Math.Abs(parsed);
        }
        public NativeValue(byte[] mantissa, int sign)
        {
            Mantissa = ParseMantissa(mantissa);
            IsNegative = sign == -1;
        }

        public override string ToString()
        {
            var mantissa = Mantissa.ToString();
            if (IsNegative)
            {
                return "-" + mantissa;
            }
            return mantissa;
        }

        public override byte[] ToBytes()
        {
            var notNegative = !IsNegative;
            var mantissa = Bits.GetBytes(Mantissa);
            mantissa[0] |= (byte) (notNegative ? 0x40 : 0x00);
            return mantissa;
        }
    }

    public class InvalidAmountValueException : Exception
    {
        public InvalidAmountValueException()
        {
        }

        public InvalidAmountValueException(string message) : base(message)
        {
        }
    }

    public class PrecisionException : Exception
    {
        public PrecisionException()
        {
        }

        public PrecisionException(string message) : base(message)
        {
        }
    }
}

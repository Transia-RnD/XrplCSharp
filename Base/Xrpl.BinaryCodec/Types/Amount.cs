using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Types;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/amount.ts

namespace Xrpl.BinaryCodec.Types
{
    public class Amount
    {
        public static Amount DefaultAmount = new Amount(new byte[] { 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

        public byte[] bytes { get; set; }

        public Amount(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public static Amount From(object value)
        {
            if (value is Amount)
            {
                return (Amount)value;
            }

            byte[] amount = new byte[8];

            if (value is string)
            {
                AssertXrpIsValid((string)value);

                BigInteger number = BigInteger.Parse((string)value);

                byte[] intBuf = new byte[8];
                byte[] msb = BitConverter.GetBytes(number >> 32);
                byte[] lsb = BitConverter.GetBytes(number & 0x00000000ffffffff);

                Array.Copy(msb, 0, intBuf, 0, 4);
                Array.Copy(lsb, 0, intBuf, 4, 4);

                amount = intBuf;

                amount[0] |= 0x40;

                return new Amount(amount);
            }

            if (value is AmountObject)
            {
                AmountObject amountObject = (AmountObject)value;

                BigInteger number = BigInteger.Parse(amountObject.value);
                AssertIouIsValid(number);

                if (number == 0)
                {
                    amount[0] |= 0x80;
                }
                else
                {
                    string integerNumberString = number.ToString();

                    BigInteger num = BigInteger.Parse(integerNumberString);
                    byte[] intBuf = new byte[8];
                    byte[] msb = BitConverter.GetBytes(num >> 32);
                    byte[] lsb = BitConverter.GetBytes(num & 0x00000000ffffffff);

                    Array.Copy(msb, 0, intBuf, 0, 4);
                    Array.Copy(lsb, 0, intBuf, 4, 4);

                    amount = intBuf;

                    amount[0] |= 0x80;

                    if (number > 0)
                    {
                        amount[0] |= 0x40;
                    }

                    int exponent = number.ToString().Length - 15;
                    int exponentByte = 97 + exponent;
                    amount[0] |= (byte)(exponentByte >> 2);
                    amount[1] |= (byte)((exponentByte & 0x03) << 6);
                }

                byte[] currency = Currency.From(amountObject.currency).bytes;
                byte[] issuer = AccountID.From(amountObject.issuer).bytes;
                return new Amount(amount.Concat(currency).Concat(issuer).ToArray());
            }

            throw new Exception("Invalid type to construct an Amount");
        }

        public static Amount FromParser(BinaryParser parser)
        {
            bool isXRP = (parser.Peek() & 0x80) == 0x80;
            int numBytes = isXRP ? 48 : 8;
            return new Amount(parser.Read(numBytes));
        }

        public object ToJSON()
        {
            if (IsNative())
            {
                byte[] bytes = this.bytes;
                bool isPositive = (bytes[0] & 0x40) == 0x40;
                string sign = isPositive ? "" : "-";
                bytes[0] &= 0x3f;

                BigInteger msb = BitConverter.ToUInt32(bytes, 0);
                BigInteger lsb = BitConverter.ToUInt32(bytes, 4);
                BigInteger num = (msb << 32) | lsb;

                return sign + num.ToString();
            }
            else
            {
                BinaryParser parser = new BinaryParser(this.ToString());
                byte[] mantissa = parser.Read(8);
                Currency currency = Currency.FromParser(parser);
                AccountID issuer = AccountID.FromParser(parser);

                byte b1 = mantissa[0];
                byte b2 = mantissa[1];

                bool isPositive = (b1 & 0x40) == 0x40;
                string sign = isPositive ? "" : "-";
                int exponent = ((b1 & 0x3f) << 2) + ((b2 & 0xff) >> 6) - 97;

                mantissa[0] = 0;
                mantissa[1] &= 0x3f;
                BigInteger value = BigInteger.Parse(sign + "0x" + BitConverter.ToString(mantissa).Replace("-", ""));
                AssertIouIsValid(value);

                return new AmountObject
                {
                    value = value.ToString(),
                    currency = currency.ToJSON(),
                    issuer = issuer.ToJSON()
                };
            }
        }

        private static void AssertXrpIsValid(string amount)
        {
            if (amount.IndexOf('.') != -1)
            {
                throw new Exception(amount + " is an illegal amount");
            };

            decimal value = decimal.Parse(amount);
            if (value != 0)
            {
                if (value < 0.000001m || value > 10000000000000000m)
                {
                    throw new Exception(amount + " is an illegal amount");
                }
            }
        }

        private static void AssertIouIsValid(BigInteger value)
        {
            if (value != 0)
            {
                int p = value.ToString().Length;
                int e = value.ToString().Length - 15;
                if (p > 16 || e > 80 || e < -96)
                {
                    throw new Exception("Decimal precision out of range");
                }
                VerifyNoDecimal(value);
            }
        }

        private static void VerifyNoDecimal(BigInteger value)
        {
            string integerNumberString = value.ToString();

            if (integerNumberString.IndexOf('.') != -1)
            {
                throw new Exception("Decimal place found in integerNumberString");
            }
        }

        private bool IsNative()
        {
            return (this.bytes[0] & 0x80) == 0;
        }
    }
}
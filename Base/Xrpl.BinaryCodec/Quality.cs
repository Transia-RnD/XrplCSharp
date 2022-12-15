using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Globalization;

namespace Xrpl.BinaryCodec
{
    public class Quality
    {
        public static byte[] Encode(string quality)
        {
            var decimalQuality = decimal.Parse(quality, CultureInfo.InvariantCulture);
            var exponent = decimalQuality.GetExponent();
            var qualityString = decimalQuality.ToString("G", CultureInfo.InvariantCulture);
            var bytes = UInt64.From(BigInteger.Parse(qualityString)).ToBytes();
            bytes[0] = (byte)(exponent + 100);
            return bytes;
        }

        public static decimal Decode(string quality)
        {
            var bytes = Encoding.UTF8.GetBytes(quality).Skip(quality.Length - 8).ToArray();
            var exponent = bytes[0] - 100;
            var mantissa = new decimal(new BigInteger(bytes.Skip(1).ToArray()));
            return mantissa * (decimal)Math.Pow(10, exponent);
        }
    }
}
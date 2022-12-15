using Newtonsoft.Json.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Util;

// https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/uint-64.ts


namespace Xrpl.BinaryCodec.Types
{
    public class UInt64 : UInt
    {
        private static readonly int width = 64 / 8;
        public static readonly UInt64 defaultUInt64 = new UInt64(new byte[width]);

        public UInt64(byte[] bytes) : base(bytes ?? defaultUInt64.bytes)
        {
        }

        public static UInt fromParser(BinaryParser parser)
        {
            return new UInt64(parser.read(width));
        }

        public static UInt64 from(object val)
        {
            if (val is UInt64)
            {
                return (UInt64)val;
            }

            byte[] buf = new byte[width];

            if (val is int)
            {
                if ((int)val < 0)
                {
                    throw new Exception("value must be an unsigned integer");
                }

                BigInteger number = new BigInteger((int)val);

                byte[][] intBuf = new byte[2][];
                intBuf[0] = new byte[4];
                intBuf[1] = new byte[4];
                intBuf[0].writeUInt32BE(number.shiftRight(32), 0);
                intBuf[1].writeUInt32BE(number.and(mask), 0);

                return new UInt64(Buffer.concat(intBuf));
            }

            if (val is string)
            {
                if (!HEX_REGEX.test(val))
                {
                    throw new Exception($"{val} is not a valid hex-string");
                }

                string strBuf = val.padStart(16, '0');
                buf = Buffer.from(strBuf, "hex");
                return new UInt64(buf);
            }

            if (val is BigInteger)
            {
                byte[][] intBuf = new byte[2][];
                intBuf[0] = new byte[4];
                intBuf[1] = new byte[4];
                intBuf[0].writeUInt32BE(number.shiftRight(32), 0);
                intBuf[1].writeUInt32BE(number.and(mask), 0);

                return new UInt64(Buffer.concat(intBuf));
            }

            throw new Exception("Cannot construct UInt64 from given value");
        }

        public string toJSON()
        {
            return this.bytes.ToString("hex").toUpperCase();
        }

        public BigInteger valueOf()
        {
            BigInteger msb = new BigInteger(this.bytes.slice(0, 4).readUInt32BE(0));
            BigInteger lsb = new BigInteger(this.bytes.slice(4).readUInt32BE(0));
            return msb.shiftLeft(new BigInteger(32)).or(lsb);
        }

        public byte[] toBytes()
        {
            return this.bytes;
        }
    }
}
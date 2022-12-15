using System;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Util;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/uint-32.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Derived UInt class for serializing/deserializing 32 bit UInt
    /// </summary>
    public class UInt32 : UInt
    {
        /// <summary>
        /// The width of the UInt32 in bytes
        /// </summary>
        protected static readonly int width = 32 / 8; // 4

        /// <summary>
        /// The default UInt32
        /// </summary>
        public static readonly UInt32 defaultUInt32 = new UInt32(new byte[width]);

        /// <summary>
        /// Construct a UInt32 object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to construct the UInt32 from</param>
        public UInt32(byte[] bytes) : base(bytes ?? defaultUInt32.bytes)
        {
        }

        /// <summary>
        /// Construct a UInt32 object from a BinaryParser
        /// </summary>
        /// <param name="parser">The BinaryParser to construct the UInt32 from</param>
        /// <returns>The UInt32 object</returns>
        public static UInt fromParser(BinaryParser parser)
        {
            return new UInt32(parser.read(width));
        }

        /// <summary>
        /// Construct a UInt32 object from a number
        /// </summary>
        /// <param name="val">The UInt32 object or number to construct the UInt32 from</param>
        /// <returns>The UInt32 object</returns>
        public static UInt32 from(UInt32 val)
        {
            if (val is UInt32)
            {
                return val;
            }

            byte[] buf = new byte[width];

            if (val is string)
            {
                int num = int.Parse(val);
                buf.writeUInt32BE(num, 0);
                return new UInt32(buf);
            }

            if (val is int)
            {
                buf.writeUInt32BE(val, 0);
                return new UInt32(buf);
            }

            throw new Exception("Cannot construct UInt32 from given value");
        }

        /// <summary>
        /// Get the value of a UInt32 object
        /// </summary>
        /// <returns>The number represented by this.bytes</returns>
        public int valueOf()
        {
            return this.bytes.readUInt32BE(0);
        }
    }
}
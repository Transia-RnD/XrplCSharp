using System;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Serdes;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/uint-8.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Derived UInt class for serializing/deserializing 8 bit UInt
    /// </summary>
    public class UInt8 : UInt
    {
        /// <summary>
        /// The width of the UInt8 in bytes
        /// </summary>
        protected static readonly int width = 8 / 8; // 1

        /// <summary>
        /// The default UInt8
        /// </summary>
        public static readonly UInt8 defaultUInt8 = new UInt8(new byte[UInt8.width]);

        /// <summary>
        /// Construct a UInt8 object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to construct the UInt8 from</param>
        public UInt8(byte[] bytes) : base(bytes ?? UInt8.defaultUInt8.bytes)
        {
        }

        /// <summary>
        /// Construct a UInt8 object from a BinaryParser
        /// </summary>
        /// <param name="parser">The BinaryParser to construct the UInt8 from</param>
        /// <returns>The UInt8 object</returns>
        public static UInt fromParser(BinaryParser parser)
        {
            return new UInt8(parser.read(UInt8.width));
        }

        /// <summary>
        /// Construct a UInt8 object from a number
        /// </summary>
        /// <param name="val">The UInt8 object or number to construct the UInt8 from</param>
        /// <returns>The UInt8 object</returns>
        public static UInt8 from(UInt8 val)
        {
            if (val is UInt8)
            {
                return val;
            }

            if (typeof(val) is int)
            {
                byte[] buf = new byte[UInt8.width];
                buf.writeUInt8(val, 0);
                return new UInt8(buf);
            }

            throw new Exception("Cannot construct UInt8 from given value");
        }

        /// <summary>
        /// Get the value of a UInt8 object
        /// </summary>
        /// <returns>The number represented by this.bytes</returns>
        public int valueOf()
        {
            return this.bytes.readUInt8(0);
        }
    }
}
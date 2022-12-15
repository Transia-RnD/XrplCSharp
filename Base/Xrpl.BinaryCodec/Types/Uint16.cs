using System;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Util;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/uint-16.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Derived UInt class for serializing/deserializing 16 bit UInt
    /// </summary>
    public class UInt16 : UInt
    {
        /// <summary>
        /// The width of the UInt16 in bytes
        /// </summary>
        protected static readonly int width = 16 / 8; // 2

        /// <summary>
        /// The default UInt16
        /// </summary>
        public static readonly UInt16 defaultUInt16 = new UInt16(new byte[UInt16.width]);

        /// <summary>
        /// Construct a UInt16 object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to construct the UInt16 from</param>
        public UInt16(byte[] bytes) : base(bytes ?? UInt16.defaultUInt16.bytes)
        {
        }

        /// <summary>
        /// Construct a UInt16 object from a BinaryParser
        /// </summary>
        /// <param name="parser">The BinaryParser to construct the UInt16 from</param>
        /// <returns>The UInt16 object</returns>
        public static UInt fromParser(BinaryParser parser)
        {
            return new UInt16(parser.read(UInt16.width));
        }

        /// <summary>
        /// Construct a UInt16 object from a number
        /// </summary>
        /// <param name="val">The UInt16 object or number to construct the UInt16 from</param>
        /// <returns>The UInt16 object</returns>
        public static UInt16 from(UInt16 val)
        {
            if (val is UInt16)
            {
                return val;
            }

            if (typeof(val) is int)
            {
                byte[] buf = new byte[UInt16.width];
                buf.writeUInt16BE(val, 0);
                return new UInt16(buf);
            }

            throw new Exception("Can not construct UInt16 with given value");
        }

        /// <summary>
        /// Get the value of a UInt16 object
        /// </summary>
        /// <returns>The number represented by this.bytes</returns>
        public int valueOf()
        {
            return this.bytes.readUInt16BE(0);
        }
    }
}
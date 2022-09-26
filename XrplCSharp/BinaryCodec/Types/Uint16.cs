using Newtonsoft.Json.Linq;

using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Util;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/uint-16.ts

namespace Xrpl.BinaryCodecLib.Types
{
    /// <summary>
    ///  Derived UInt class for serializing/deserializing 16 bit UInt
    /// </summary>
    public class Uint16 : Uint<ushort>
    {
        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="value">ushort value</param>
        public Uint16(ushort value) : base(value)
        {
        }

        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="value">byte value</param>
        public Uint16(byte value) : base(value)
        {
        }

        /// <summary> Deserialize Uint16 </summary>
        /// <param name="token">json token</param>
        /// <returns>Uint16 value</returns>
        public static Uint16 FromJson(JToken token) => (ushort) token;

        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="v">ushort value</param>
        public static implicit operator Uint16(ushort v) => new Uint16(v);

        /// <inheritdoc />
        public override byte[] ToBytes() => Bits.GetBytes(Value);

        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="v">int value</param>
        public static Uint16 FromValue(int v) => new Uint16((byte)v);

        /// <summary>
        /// Construct a Uint16 from a BinaryParser
        /// </summary>
        /// <param name="parser">A BinaryParser to read Uint16 from</param>
        /// <returns></returns>
        public static Uint16 FromParser(BinaryParser parser, int? hint=null) => Bits.ToUInt16(parser.Read(2), 0);
    }
}
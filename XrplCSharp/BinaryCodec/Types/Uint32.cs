using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Util;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/uint-32.ts

namespace Xrpl.BinaryCodecLib.Types
{
    /// <summary>
    /// Derived UInt class for serializing/deserializing 32 bit UInt
    /// </summary>
    public class Uint32 : Uint<uint>
    {
        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="value">uint value</param>
        public Uint32(uint value) : base(value)
        {
        }
        /// <summary> Deserialize Uint32 </summary>
        /// <param name="token">json token</param>
        /// <returns>Uint32 value</returns>
        public static Uint32 FromJson(JToken token) => (uint)token;

        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="v">uint value</param>
        public static implicit operator Uint32(uint v) => new Uint32(v);

        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="v">uint value</param>
        public static implicit operator uint(Uint32 v) => v.Value;

        /// <inheritdoc />
        public override byte[] ToBytes() => Bits.GetBytes(Value);

        /// <summary>
        /// Construct a Uint16 from a BinaryParser
        /// </summary>
        /// <param name="parser">A BinaryParser to read Uint16 from</param>
        /// <returns></returns>
        public static Uint32 FromParser(BinaryParser parser, int? hint=null) => Bits.ToUInt32(parser.Read(4), 0);
    }
}
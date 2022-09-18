using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/uint-8.ts

namespace Ripple.Binary.Codec.Types
{
    /// <summary>
    ///  Derived UInt class for serializing/deserializing 8 bit UInt
    /// </summary>
    public class Uint8 : Uint<byte>
    {
        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="value">byte value</param>
        public Uint8(byte value) : base(value)
        {
        }

        /// <inheritdoc />
        public override byte[] ToBytes() => new [] {Value};

        /// <summary> Deserialize Uint8 </summary>
        /// <param name="token">json token</param>
        /// <returns>Uint8 value</returns>
        public static Uint8 FromJson(JToken token) => (byte) token;

        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="v">byte value</param>
        public static implicit operator Uint8(byte v) => new Uint8(v);

        /// <summary>
        /// Construct a Uint8 from a BinaryParser
        /// </summary>
        /// <param name="parser">A BinaryParser to read Uint8 from</param>
        /// <returns></returns>
        public static Uint8 FromParser(BinaryParser parser, int? hint=null) => parser.ReadOne();
    }
}
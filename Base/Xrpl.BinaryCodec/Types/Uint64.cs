using Newtonsoft.Json.Linq;

using System.Text.RegularExpressions;

using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Util;

// https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/uint-64.ts


namespace Xrpl.BinaryCodecLib.Types
{
    /// <summary>
    /// Derived UInt class for serializing/deserializing 64 bit UInt
    /// </summary>
    public class Uint64 : Uint<ulong>
    {
        static string HEX_REGEX = @"^[a-fA-F0-9]{1,16}$";

        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="value">ulong value</param>
        public Uint64(ulong value) : base(value)
        {
        }

        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="value">byte value</param>
        public Uint64(byte value) : base(value)
        {
        }

        /// <inheritdoc />
        public override byte[] ToBytes() => Bits.GetBytes(Value);

        /// <inheritdoc />
        public override string ToString() => B16.Encode(ToBytes());

        /// <summary> Deserialize Uint64 </summary>
        /// <param name="token">json token</param>
        /// <returns>Uint64 value</returns>
        public static Uint64 FromJson(JToken token) => Bits.ToUInt64(B16.Decode(token.ToString()), 0);

        public static implicit operator Uint64(ulong v) => new Uint64(v);

        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="v">byte value</param>
        public static implicit operator Uint64(byte v) => new Uint64(v);

        /// <summary>
        /// create instance of this value from string
        /// </summary>
        public static Uint64 FromValue(int v)
        {
            byte[] valueBytes = Bits.GetBytes(v);
            return new Uint64(Bits.ToUInt32(valueBytes, 0));
        }

        /// <summary>
        /// create instance of this value from string
        /// </summary>
        public static Uint64 FromValue(string v)
        {
            Regex rg = new Regex(HEX_REGEX);
            if (rg.Matches(v).Count == 0)
            {
                throw new BinaryCodecException($"{v} is not a valid hex string");
            }

            string strBuf = v.PadRight(16, '0');
            return new Uint64(Bits.ToUInt64(strBuf.FromHex(), 0));
        }

        /// <inheritdoc />
        public override JToken ToJson()
        {
            return ToBytes().ToHex();
        }

        /// <summary>
        /// Construct a Uint64 from a BinaryParser
        /// </summary>
        /// <param name="parser">A BinaryParser to read Uint64 from</param>
        /// <returns></returns>
        public static Uint64 FromParser(BinaryParser parser, int? hint=null) => Bits.ToUInt64(parser.Read(8), 0);
    }
}
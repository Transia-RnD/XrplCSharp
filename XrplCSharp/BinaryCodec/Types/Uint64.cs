using Newtonsoft.Json.Linq;

using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Util;

using System;
using System.Numerics;

// https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/uint-64.ts


namespace Xrpl.BinaryCodecLib.Types
{
    /// <summary>
    /// Derived UInt class for serializing/deserializing 64 bit UInt
    /// </summary>
    public class Uint64 : Uint<ulong>
    {
        /// <summary>
        /// create instance of this value
        /// </summary>
        /// <param name="value">ulong value</param>
        public Uint64(ulong value) : base(value)
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
        /// create instance of this value from string
        /// </summary>
        public static Uint64 FromValue(string v)
        {
            BigInteger bignum = new BigInteger(Convert.ToByte(v));
            return new Uint64(((ulong)bignum));
        }

        /// <inheritdoc />
        public override JToken ToJson() => ToString();
        /// <summary>
        /// Construct a Uint64 from a BinaryParser
        /// </summary>
        /// <param name="parser">A BinaryParser to read Uint64 from</param>
        /// <returns></returns>

        public static Uint64 FromParser(BinaryParser parser, int? hint=null) => Bits.ToUInt64(parser.Read(8), 0);
    }
}
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Util;
using static Xrpl.BinaryCodec.Types.SerializedType;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/hash.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Base class defining how to encode and decode hashes
    /// </summary>
    public abstract class Hash : Comparable
    {
        /// <summary>
        /// The width of the hash
        /// </summary>
        public static readonly int Width;

        protected Hash(byte[] bytes) : base(bytes)
        {
            this.bytes = bytes;
        }

        protected Hash(object value)
        {
            Value = value;
        }

        public object Value { get; }

        /// <summary>
        /// Construct a Hash object from an existing Hash object or a hex-string
        /// </summary>
        /// <param name="value">A hash object or hex-string of a hash</param>
        /// <returns>The hash object</returns>
        public static Hash From<T>(T value) where T : Hash, IEquatable<string>
        {
            if (value is Hash)
            {
                return value;
            }

            if (value is string)
            {
                return new Hash(Encoding.UTF8.GetBytes(value));
            }

            throw new Exception("Cannot construct Hash from given value");
        }

        /// <summary>
        /// Read a Hash object from a BinaryParser
        /// </summary>
        /// <param name="parser">BinaryParser to read the hash from</param>
        /// <param name="hint">length of the bytes to read, optional</param>
        /// <returns>The hash object</returns>
        public static Hash FromParser(BinaryParser parser, int? hint = null)
        {
            return new Hash(parser.Read(hint ?? Width));
        }

        /// <summary>
        /// Overloaded operator for comparing two hash objects
        /// </summary>
        /// <param name="other">The Hash to compare this to</param>
        /// <returns>The comparison result</returns>
        public int CompareTo(Hash other)
        {
            return this.bytes.CompareTo(From(other).Bytes);
        }

        /// <summary>
        /// Returns four bits at the specified depth within a hash
        /// </summary>
        /// <param name="depth">The depth of the four bits</param>
        /// <returns>The number represented by the four bits</returns>
        public int Nibblet(int depth)
        {
            int byteIx = depth > 0 ? (depth / 2) | 0 : 0;
            int b = this.bytes[byteIx];
            if (depth % 2 == 0)
            {
                b = (b & 0xf0) >> 4;
            }
            else
            {
                b = b & 0x0f;
            }
            return b;
        }
    }
}
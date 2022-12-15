

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/blob.ts

using System;
using System.Text;
using Xrpl.BinaryCodec.Serdes;

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Variable length encoded type
    /// </summary>
    public class Blob : SerializedType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bytes">The bytes to be used as the blob</param>
        public Blob(byte[] bytes) : base(bytes)
        {
        }

        /// <summary>
        /// Defines how to read a Blob from a BinaryParser
        /// </summary>
        /// <param name="parser">The binary parser to read the Blob from</param>
        /// <param name="hint">The length of the blob, computed by readVariableLengthLength() and passed in</param>
        /// <returns>A Blob object</returns>
        public static Blob FromParser(BinaryParser parser, int hint)
        {
            return new Blob(parser.Read(hint));
        }

        /// <summary>
        /// Create a Blob object from a hex-string
        /// </summary>
        /// <param name="value">existing Blob object or a hex-string</param>
        /// <returns>A Blob object</returns>
        public static Blob From(Blob value)
        {
            if (value is Blob)
            {
                return value;
            }

            if (value is string)
            {
                return new Blob(Encoding.UTF8.GetBytes(value));
            }

            throw new Exception("Cannot construct Blob from value given");
        }
    }
}
using System;
using System.Diagnostics;

using Newtonsoft.Json.Linq;

using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Util;

//https://xrpl.org/serialization.html#blob-fields
//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/blob.ts

namespace Xrpl.BinaryCodecLib.Types
{
    /// <summary>
    /// The Blob type is a length-prefixed field with arbitrary data.<br/>
    /// Two common fields that use this type are SigningPubKey and TxnSignature, which contain (respectively)
    /// the public key and signature that authorize a transaction to be executed.<br/>
    /// Blob fields have no further structure to their contents, so they consist of
    /// exactly the amount of bytes indicated in the variable-length encoding, after the Field ID and length prefixes.
    ///<br/><br/>
    /// Variable length encoded type
    /// </summary>
    public class Blob : ISerializedType
    {
        public readonly byte[] Buffer;
        private Blob(byte[] decode)
        {
            this.Buffer = decode;
        }
        public static Blob FromHex(string value)
        {
            return B16.Decode(value);
        }
        public static implicit operator Blob(byte[] value)
        {
            return new Blob(value);
        }
        public static implicit operator Blob(JToken token)
        {
            return FromJson(token);
        }
        /// Defines how to read a Blob from json
        /// <param name="token">json token</param>
        /// <returns>A Blob object</returns>
        public static Blob FromJson(JToken token) => FromHex(token.ToString());

        /// <inheritdoc />
        public void ToBytes(IBytesSink sink)
        {
            sink.Put(Buffer);
        }

        /// <inheritdoc />
        public JToken ToJson() => ToString();

        /// <inheritdoc />
        public override string ToString()
        {
            return B16.Encode(Buffer);
        }
        /// <summary>
        /// Defines how to read a Blob from a BinaryParser
        /// </summary>
        /// <param name="parser">The binary parser to read the Blob from</param>
        /// <param name="hint">The length of the blob, computed by readVariableLengthLength() and passed in</param>
        /// <returns>A Blob object</returns>
        public static Blob FromParser(BinaryParser parser, int? hint = null)
        {
            Debug.Assert(hint != null, "hint != null");
            return parser.Read((int)hint);
        }
        /// <summary>
        /// Create a Blob object from a hex-string
        /// </summary>
        /// <param name="blob">existing hex-string</param>
        /// <param name="encoding">string encoding</param>
        /// <returns>A Blob object</returns>
        public static Blob FromString(string blob, System.Text.Encoding encoding) => new Blob(encoding.GetBytes(blob));

        /// <summary>
        /// Create a Blob object from a hex-string
        /// </summary>
        /// <param name="blob">existing hex-string in ASCII</param>
        /// <returns>A Blob object</returns>
        public static Blob FromAscii(string blob) => FromString(blob, System.Text.Encoding.ASCII);
    }
}
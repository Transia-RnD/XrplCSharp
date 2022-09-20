using Ripple.Binary.Codec.Util;

using System;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/serdes/binary-parser.ts#L9

namespace Ripple.Binary.Codec.Binary
{
    /// <inheritdoc />
    public class BufferParser : BinaryParser
    {
        /// <summary> buffer bytes </summary>
        protected internal byte[] Bytes;

        /// <summary> Initialize parser from bytes  </summary>
        /// <param name="bytes">bytes</param>
        public BufferParser (byte[] bytes)
        {
            Size = bytes.Length;
            Bytes = bytes;
        }

        /// <summary>
        /// Initialize bytes to a hex string
        /// </summary>
        /// <param name="hex">hexBytes a hex string</param>
        public BufferParser(string hex) : this(B16.Decode(hex))
        {
            
        }

        /// <inheritdoc />
        public override void Skip(int n) => Cursor += n;

        /// <inheritdoc />
        public override byte ReadOne() => Bytes[Cursor++];

        /// <inheritdoc />
        public override byte[] Read(int n)
        {
            byte[] ret = new byte[n];
            Array.Copy(Bytes, Cursor, ret, 0, n);
            Cursor += n;
            return ret;
        }
    }
}
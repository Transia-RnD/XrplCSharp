using System;
using Xrpl.BinaryCodec.Util;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/serdes/binary-parser.ts

namespace Xrpl.BinaryCodec.Binary
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
        public byte[] ToBytes() => Bytes;

        /// <inheritdoc />
        public override byte Peek() => Bytes[0];

        /// <inheritdoc />
        public override void Skip(int n) => Cursor += n;

        /// <inheritdoc />
        public override byte[] Read(int n)
        {
            byte[] ret = new byte[n];
            Array.Copy(Bytes, Cursor, ret, 0, n);
            Cursor += n;
            return ret;
        }

        public override byte ReadOne() => Bytes[Cursor++];

        public byte ReadUIntN(int n)
        {
            if (n < 0 || n > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "n must be between 0 and 4.");
            }

            var bytes = Read(n);
            var result = 0;
            for (var i = 0; i < n; i++)
            {
                result = (result << 8) | bytes[i];
            }

            return (byte)result;
        }

        public override byte ReadUInt8()
        {
            return this.ReadUIntN(1);
        }
    }
}
using System;
using System.Linq;
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

        /// <inheritdoc />
        public int ReadUIntN(int n)
        {
            if (n < 0 && 4 <= n)
            {
                throw new BinaryCodecError("Invalid n");
            }
            return this.Read(n).Aggregate((v, _byte) => (byte)(v << 8 | (int)_byte));
        }

        public int ReadUInt8()
        {
            return ReadUIntN(n: 1);
        }

        public int ReadUInt16()
        {
            return ReadUIntN(n: 2);
        }

        public int ReadUInt32()
        {
            return ReadUIntN(n: 4);
        }

        /// <inheritdoc />
        public override byte ReadOne() => Bytes[Cursor++];
    }
}
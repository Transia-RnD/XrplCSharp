using System;
using Ripple.Binary.Codec.Util;

namespace Ripple.Binary.Codec.Binary
{
    public class BufferParser : BinaryParser
    {
        protected internal byte[] Bytes;

        public BufferParser (byte[] bytes)
        {
            Size = bytes.Length;
            Bytes = bytes;
        }

        public BufferParser(string hex) : this(B16.Decode(hex))
        {
            
        }

        public override void Skip(int n) => Cursor += n;
        public override byte ReadOne() => Bytes[Cursor++];

        public override byte[] Read(int n)
        {
            byte[] ret = new byte[n];
            Array.Copy(Bytes, Cursor, ret, 0, n);
            Cursor += n;
            return ret;
        }
    }
}
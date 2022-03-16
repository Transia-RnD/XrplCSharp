using System;
using System.IO;

namespace Ripple.Core.Binary
{
    public class StreamParser : BinaryParser
    {
        private readonly Stream _stream;

        public StreamParser(Stream stream)
        {
            _stream = stream;
            Size = (int) _stream.Length;
        }

        public override byte[] Read(int n)
        {
            byte[] ret = new byte[n];
            var read = _stream.Read(ret, 0, n);
            if (read != n)
            {
                throw new InvalidOperationException();
            }
            Cursor += n;
            return ret;
        }

        public override byte ReadOne()
        {
            return Read(1)[0];
        }

        public override void Skip(int n)
        {
            Read(n);
        }
    }
}
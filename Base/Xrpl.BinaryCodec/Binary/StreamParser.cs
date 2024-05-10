using System;
using System.IO;

namespace Xrpl.BinaryCodec.Binary
{
    /// <inheritdoc />
    public class StreamParser : BinaryParser
    {
        private readonly Stream _stream;

        /// <summary> Initialize parser from stream  </summary>
        /// <param name="stream">stream</param>
        public StreamParser(Stream stream)
        {
            _stream = stream;
            Size = (int) _stream.Length;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override byte ReadOne()
        {
            return Read(1)[0];
        }

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

        /// <inheritdoc />
        public override byte Peek()
        {
            return Read(1)[0];
        }

        /// <inheritdoc />
        public override void Skip(int n)
        {
            Read(n);
        }
    }
}
using System;
using System.IO;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/serdes/binary-parser.ts#L9

namespace Ripple.Binary.Codec.Binary
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

        /// <inheritdoc />
        public override void Skip(int n)
        {
            Read(n);
        }
    }
}
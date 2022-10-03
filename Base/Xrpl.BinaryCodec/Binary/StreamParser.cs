//using System;
//using System.IO;

//namespace Xrpl.BinaryCodec.Binary
//{
//    /// <inheritdoc />
//    public class StreamParser : BinaryParser
//    {
//        private readonly Stream _stream;

//        /// <summary> Initialize parser from stream  </summary>
//        /// <param name="stream">stream</param>
//        public StreamParser(Stream stream)
//        {
//            _stream = stream;
//            Size = (int) _stream.Length;
//        }

//        /// <inheritdoc />
//        public override byte[] Read(int n)
//        {
//            byte[] ret = new byte[n];
//            var read = _stream.Read(ret, 0, n);
//            if (read != n)
//            {
//                throw new InvalidOperationException();
//            }
//            Cursor += n;
//            return ret;
//        }

//        /// <inheritdoc />
//        public override byte ReadOne()
//        {
//            return Read(1)[0];
//        }

//        /// <inheritdoc />
//        public override byte Peek()
//        {
//            return Read(1)[0];
//        }

//        /// <inheritdoc />
//        public override void Skip(int n)
//        {
//            Read(n);
//        }
//    }
//}
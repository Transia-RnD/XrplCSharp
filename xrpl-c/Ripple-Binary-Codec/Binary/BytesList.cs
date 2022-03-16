using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ripple.Core.Binary
{
    public class BytesList : IBytesSink
    {
        private readonly List<byte[]> _buffer = new List<byte[]>();
        private int _len;

        public void Add(BytesList bl)
        {
            foreach (byte[] bytes in bl.RawList())
            {
                Put(bytes);
            }
        }

        public void Put(byte aByte)
        {
            Put(new[] { aByte });
        }

        public void Put(byte[] bytes)
        {
            _len += bytes.Length;
            _buffer.Add(bytes);
        }

        public byte[] Bytes()
        {
            var n = BytesLength();
            var bytes = new byte[n];
            AddBytes(bytes, 0);
            return bytes;
        }

        public static string[] HexLookup = new string[256];
        static BytesList()
        {
            for (var i = 0; i < 256; i++)
            {
                var s = i.ToString("x").ToUpper();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }
                HexLookup[i] = s;
            }
        }

        public string BytesHex()
        {
            var builder = new StringBuilder(_len * 2);
            foreach (var aByte in _buffer.SelectMany(buf => buf))
            {
                builder.Append(HexLookup[aByte & 0xFF]);
            }
            return builder.ToString();
        }

        public int BytesLength()
        {
            return _len;
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private int AddBytes(byte[] bytes, int destPos)
        {

            foreach (byte[] buf in _buffer)
            {
                Array.Copy(buf, 0, bytes, destPos, buf.Length);
                destPos += buf.Length;
            }
            return destPos;
        }

        public List<byte[]> RawList()
        {
            return _buffer;
        }
    }
}
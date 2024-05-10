using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/serdes/binary-serializer.ts#L9

namespace Xrpl.BinaryCodec.Binary
{
    /// <summary> Bytes list is a collection of buffer objects </summary>
    public class BytesList : IBytesSink
    {
        private readonly List<byte[]> _buffer = new List<byte[]>();
        private int _len;
        /// <summary>
        /// add Bytes list to this Bytes list
        /// </summary>
        /// <param name="bl">Bytes list</param>
        public void Add(BytesList bl)
        {
            foreach (byte[] bytes in bl.RawList())
            {
                Put(bytes);
            }
        }
        /// <summary>
        /// Put bytes in the BytesList
        /// </summary>
        /// <param name="aByte">byte</param>
        public void Put(byte aByte)
        {
            Put(new[] { aByte });
        }
        /// <summary>
        /// Put bytes in the BytesList
        /// </summary>
        /// <param name="bytes">bytesArg A Buffer</param>
        public void Put(byte[] bytes)
        {
            _len += bytes.Length;
            _buffer.Add(bytes);
        }
        /// <summary> Get all bytes </summary>
        /// <returns>Bytes</returns>
        public byte[] ToBytes()
        {
            var n = BytesLength();
            var bytes = new byte[n];
            AddBytes(bytes, 0);
            return bytes;
        }
        /// <summary> Hex Lookup </summary>
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
        /// <summary> convert bytes to hex string </summary>
        /// <returns>string</returns>
        public string BytesHex()
        {
            var builder = new StringBuilder(_len * 2);
            foreach (var aByte in _buffer.SelectMany(buf => buf))
            {
                builder.Append(HexLookup[aByte & 0xFF]);
            }
            return builder.ToString();
        }
        /// <summary>
        /// Get the total number of bytes in the BytesList
        /// </summary>
        /// <returns> the number of bytes</returns>
        public int BytesLength()
        {
            return _len;
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        /// <summary> add bytes to selected position </summary>
        /// <param name="bytes">bytes</param>
        /// <param name="destPos">position</param>
        /// <returns></returns>
        private int AddBytes(byte[] bytes, int destPos)
        {

            foreach (byte[] buf in _buffer)
            {
                Array.Copy(buf, 0, bytes, destPos, buf.Length);
                destPos += buf.Length;
            }
            return destPos;
        }
        /// <summary>
        /// view bytes list
        /// </summary>
        /// <returns>Raw List</returns>
        public List<byte[]> RawList()
        {
            return _buffer;
        }
    }
}
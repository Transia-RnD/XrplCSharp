using Ripple.Binary.Codec.Enums;

using System;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/serdes/binary-serializer.ts#L52

namespace Ripple.Binary.Codec.Binary
{
    /// <summary>
    /// BinarySerializer is used to write fields and values to buffers
    /// </summary>
    public class BinarySerializer : IBytesSink
    {
        private readonly IBytesSink _sink;
        /// <summary>
        /// create a value to this BinarySerializer
        /// </summary>
        /// <param name="sink">Bytes Sink</param>
        public BinarySerializer(IBytesSink sink)
        {
            _sink = sink;
        }
        /// <inheritdoc />
        public void Put(byte[] n)
        {
            _sink.Put(n);
        }
        /// <summary>
        ///  Calculate the header of Variable Length encoded bytes
        /// </summary>
        /// <param name="n">length the length of the bytes</param>
        public void AddLengthEncoded(byte[] n)
        {
            Put(EncodeVl(n.Length));
            Put(n);
        }

        public static byte[] EncodeVl(int length)
        {
            // TODO: bytes
            var lenBytes = new byte[4];

            if (length <= 192)
            {
                lenBytes[0] = (byte)length;
                return TakeSome(lenBytes, 1);
            }
            if (length <= 12480)
            {
                length -= 193;
                lenBytes[0] = (byte)(193u + (length >> 8));
                lenBytes[1] = (byte)(length & 0xff);
                return TakeSome(lenBytes, 2);
            }
            if (length <= 918745)
            {
                length -= 12481;
                lenBytes[0] = (byte)(241u + (length >> 16));
                lenBytes[1] = (byte)((length >> 8) & 0xff);
                lenBytes[2] = (byte)(length & 0xff);
                return TakeSome(lenBytes, 3);
            }
            throw new InvalidOperationException($"length must <= 918745, was {length}");
        }

        private static byte[] TakeSome(byte[] buffer, int n)
        {
            var ret = new byte[n];
            Array.Copy(buffer, 0, ret, 0, n);
            return ret;
        }
        /// <summary>
        /// Write a value to this BinarySerializer
        /// </summary>
        /// <param name="bl">value a SerializedType value</param>
        public void Add(BytesList bl)
        {
            foreach (byte[] bytes in bl.RawList())
            {
                _sink.Put(bytes);
            }
        }
        /// <summary>
        /// Write field header to this BinarySerializer
        /// </summary>
        /// <param name="f">field</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public int AddFieldHeader(Field f)
        {
            if (!f.IsSerialised)
            {
                throw new InvalidOperationException(
                    $"Field {f} is a discardable field");
            }
            var n = f.Header;
            Put(n);
            return n.Length;
        }

        /// <inheritdoc />
        public void Put(byte type)
        {
            _sink.Put(type);
        }
        /// <summary>
        /// Write a variable length encoded value to the BinarySerializer
        /// </summary>
        /// <param name="bytes">value a SerializedType value</param>
        public void AddLengthEncoded(BytesList bytes)
        {
            Put(EncodeVl(bytes.BytesLength()));
            Add(bytes);
        }
        /// <summary>
        /// Write field and value to BinarySerializer
        /// </summary>
        /// <param name="field">field field to write to BinarySerializer</param>
        /// <param name="value">value value to write to BinarySerializer</param>
        public void Add(Field field, ISerializedType value)
        {
            AddFieldHeader(field);
            if (field.IsVlEncoded)
            {
                AddLengthEncoded(value);
            }
            else
            {
                value.ToBytes(_sink);
                if (field.Type == FieldType.StObject)
                {
                    AddFieldHeader(Field.ObjectEndMarker);
                }
                else if (field.Type == FieldType.StArray)
                {
                    AddFieldHeader(Field.ArrayEndMarker);
                }
            }
        }
        /// <summary>
        /// Write a variable length encoded value to the BinarySerializer
        /// </summary>
        /// <param name="value">value length encoded value to write to BytesList</param>
        public void AddLengthEncoded(ISerializedType value)
        {
            var bytes = new BytesList();
            value.ToBytes(bytes);
            AddLengthEncoded(bytes);
        }
    }
}
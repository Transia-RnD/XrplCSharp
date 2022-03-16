using System;
using System.Diagnostics.Contracts;
using Ripple.Core.Enums;

namespace Ripple.Core.Binary
{
    public class BinarySerializer : IBytesSink
    {
        private readonly IBytesSink _sink;

        public BinarySerializer(IBytesSink sink)
        {
            _sink = sink;
        }

        public void Put(byte[] n)
        {
            _sink.Put(n);
        }

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

        public void Add(BytesList bl)
        {
            foreach (byte[] bytes in bl.RawList())
            {
                _sink.Put(bytes);
            }
        }

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

        public void Put(byte type)
        {
            _sink.Put(type);
        }

        public void AddLengthEncoded(BytesList bytes)
        {
            Put(EncodeVl(bytes.BytesLength()));
            Add(bytes);
        }

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

        public void AddLengthEncoded(ISerializedType value)
        {
            var bytes = new BytesList();
            value.ToBytes(bytes);
            AddLengthEncoded(bytes);
        }
    }
}
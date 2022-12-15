using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xrpl.BinaryCodec.Enums;
using Xrpl.BinaryCodec.Types;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/serdes/binary-serializer.ts

namespace Xrpl.BinaryCodec.Serdes
{
    public class BytesList : IEnumerable<byte>
    {
        private List<byte[]> bytesArray = new List<byte[]>();

        public int GetLength()
        {
            return bytesArray.Sum(x => x.Length);
        }

        public BytesList Put(byte[] bytesArg)
        {
            byte[] bytes = bytesArg; // Temporary, to catch instances of Uint8Array being passed in
            bytesArray.Add(bytes);
            return this;
        }

        public void ToBytesSink(BytesList list)
        {
            list.Put(this.ToBytes());
        }

        public byte[] ToBytes()
        {
            return bytesArray.SelectMany(x => x).ToArray();
        }

        public string ToHex()
        {
            return BitConverter.ToString(this.ToBytes()).Replace("-", "").ToUpper();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return this.ToBytes().AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.ToBytes().AsEnumerable().GetEnumerator();
        }
    }

    public class BinarySerializer
    {
        private BytesList sink = new BytesList();

        public BinarySerializer(BytesList sink)
        {
            this.sink = sink;
        }

        public void Write(SerializedType value)
        {
            value.ToBytesSink(this.sink);
        }

        public void Put(byte[] bytes)
        {
            this.sink.Put(bytes);
        }

        public void WriteType(SerializedType type, SerializedType value)
        {
            this.Write(type.From(value));
        }

        public void WriteBytesList(BytesList bl)
        {
            bl.ToBytesSink(this.sink);
        }

        private byte[] EncodeVariableLength(int length)
        {
            byte[] lenBytes = new byte[3];
            if (length <= 192)
            {
                lenBytes[0] = (byte)length;
                return lenBytes.Take(1).ToArray();
            }
            else if (length <= 12480)
            {
                length -= 193;
                lenBytes[0] = (byte)(193 + (length >> 8));
                lenBytes[1] = (byte)(length & 0xff);
                return lenBytes.Take(2).ToArray();
            }
            else if (length <= 918744)
            {
                length -= 12481;
                lenBytes[0] = (byte)(241 + (length >> 16));
                lenBytes[1] = (byte)((length >> 8) & 0xff);
                lenBytes[2] = (byte)(length & 0xff);
                return lenBytes.Take(3).ToArray();
            }
            throw new Exception("Overflow error");
        }

        public void WriteFieldAndValue(FieldInstance field, SerializedType value, bool isUnlModifyWorkaround = false)
        {
            SerializedType associatedValue = field.AssociatedType.From(value);
            if (associatedValue.ToBytesSink == null)
            {
                throw new Exception("associatedValue.ToBytesSink is null");
            }
            if (field.Name == null)
            {
                throw new Exception("field.Name is null");
            }

            this.sink.Put(field.Header);

            if (field.IsVariableLengthEncoded)
            {
                this.WriteLengthEncoded(associatedValue, isUnlModifyWorkaround);
            }
            else
            {
                associatedValue.ToBytesSink(this.sink);
            }
        }

        public void WriteLengthEncoded(SerializedType value, bool isUnlModifyWorkaround = false)
        {
            BytesList bytes = new BytesList();
            if (!isUnlModifyWorkaround)
            {
                // this part doesn't happen for the Account field in a UNLModify transaction
                value.ToBytesSink(bytes);
            }
            this.Put(this.EncodeVariableLength(bytes.GetLength()));
            this.WriteBytesList(bytes);
        }
    }
}
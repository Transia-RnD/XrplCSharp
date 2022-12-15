using System;
using System.Diagnostics;
using System.Linq;
using Xrpl.BinaryCodec.Enums;
using Xrpl.BinaryCodec.Types;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/serdes/binary-parser.ts

namespace Xrpl.BinaryCodec.Serdes
{
    public class BinaryParser
    {
        private byte[] bytes;

        public BinaryParser(string hexBytes)
        {
            this.bytes = Enumerable.Range(0, hexBytes.Length)
                                   .Where(x => x % 2 == 0)
                                   .Select(x => Convert.ToByte(hexBytes.Substring(x, 2), 16))
                                   .ToArray();
        }

        public byte Peek()
        {
            Debug.Assert(this.bytes.Length != 0);
            return this.bytes[0];
        }

        public void Skip(int n)
        {
            Debug.Assert(n <= this.bytes.Length);
            this.bytes = this.bytes.Skip(n).ToArray();
        }

        public byte[] Read(int n)
        {
            Debug.Assert(n <= this.bytes.Length);
            var slice = this.bytes.Take(n).ToArray();
            this.Skip(n);
            return slice;
        }

        public uint ReadUIntN(int n)
        {
            Debug.Assert(0 < n && n <= 4);
            return this.Read(n).Aggregate((a, b) => (byte)((a << 8) | b));
        }

        public uint ReadUInt8()
        {
            return this.ReadUIntN(1);
        }

        public uint ReadUInt16()
        {
            return this.ReadUIntN(2);
        }

        public uint ReadUInt32()
        {
            return this.ReadUIntN(4);
        }

        public int Size()
        {
            return this.bytes.Length;
        }

        public bool End(int? customEnd = null)
        {
            var length = this.bytes.Length;
            return length == 0 || (customEnd != null && length <= customEnd);
        }

        public byte[] ReadVariableLength()
        {
            return this.Read(this.ReadVariableLengthLength());
        }

        public int ReadVariableLengthLength()
        {
            var b1 = this.ReadUInt8();
            if (b1 <= 192)
            {
                return (int)b1;
            }
            else if (b1 <= 240)
            {
                var b2 = this.ReadUInt8();
                return (int)(193 + (b1 - 193) * 256 + b2);
            }
            else if (b1 <= 254)
            {
                var b2 = this.ReadUInt8();
                var b3 = this.ReadUInt8();
                return (int)(12481 + (b1 - 241) * 65536 + b2 * 256 + b3);
            }
            throw new Exception("Invalid variable length indicator");
        }

        public uint ReadFieldOrdinal()
        {
            var type = this.ReadUInt8();
            var nth = type & 15;
            type >>= 4;

            if (type == 0)
            {
                type = this.ReadUInt8();
                if (type == 0 || type < 16)
                {
                    throw new Exception("Cannot read FieldOrdinal, type_code out of range");
                }
            }

            if (nth == 0)
            {
                nth = this.ReadUInt8();
                if (nth == 0 || nth < 16)
                {
                    throw new Exception("Cannot read FieldOrdinal, field_code out of range");
                }
            }

            return (type << 16) | nth;
        }

        public FieldInstance ReadField()
        {
            return Field.FromString(this.ReadFieldOrdinal().ToString());
        }

        public SerializedType ReadType(Type type)
        {
            return (SerializedType)Activator.CreateInstance(type, this);
        }

        public Type TypeForField(FieldInstance field)
        {
            return field.AssociatedType;
        }

        public SerializedType ReadFieldValue(FieldInstance field)
        {
            var type = this.TypeForField(field);
            if (type == null)
            {
                throw new Exception($"unsupported: ({field.Name}, {field.Type.Name})");
            }
            var sizeHint = field.IsVariableLengthEncoded ? this.ReadVariableLengthLength() : (int?)null;
            var value = (SerializedType)Activator.CreateInstance(type, this, sizeHint);
            if (value == null)
            {
                throw new Exception($"fromParser for ({field.Name}, {field.Type.Name}) -> undefined ");
            }
            return value;
        }

        public Tuple<FieldInstance, SerializedType> ReadFieldAndValue()
        {
            var field = this.ReadField();
            return new Tuple<FieldInstance, SerializedType>(field, this.ReadFieldValue(field));
        }
    }
}
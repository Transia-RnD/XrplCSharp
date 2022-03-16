using System;
using System.Diagnostics.Contracts;
using Ripple.Core.Enums;

namespace Ripple.Core.Binary
{
    public abstract class BinaryParser
    {
        protected internal int Size;
        protected internal int Cursor;
        public bool End() => Cursor >= Size;
        public int Pos() => Cursor;
        public int ReadOneInt() => ReadOne() & 0xFF;

        public abstract void Skip(int n);
        public abstract byte ReadOne();
        public abstract byte[] Read(int n);

        public Field ReadField()
        {
            var fieldCode = ReadFieldCode();
            var field = Field.Values[fieldCode];
            if (field == null)
            {
                throw new InvalidOperationException(
                    "Couldn't parse field from " +
                    $"{fieldCode.ToString("x")}");
            }

            return field;
        }

        public int ReadFieldCode()
        {
            var tagByte = ReadOne();

            var typeBits = (tagByte & 0xFF) >> 4;
            if (typeBits == 0)
            {
                typeBits = ReadOne();
            }

            var fieldBits = tagByte & 0x0F;
            if (fieldBits == 0)
            {
                fieldBits = ReadOne();
            }

            return typeBits << 16 | fieldBits;
        }

        public int ReadVlLength()
        {
            var b1 = ReadOneInt();
            int result;

            if (b1 <= 192)
            {
                result = b1;
            }
            else if (b1 <= 240)
            {
                var b2 = ReadOneInt();
                result = 193 + (b1 - 193) * 256 + b2;
            }
            else if (b1 <= 254)
            {
                var b2 = ReadOneInt();
                var b3 = ReadOneInt();
                result = 12481 + (b1 - 241) * 65536 + b2 * 256 + b3;
            }
            else
            {
                throw new InvalidOperationException(
                    "Invalid variable length indicator");
            }

            return result;
        }

        public bool End(int? customEnd)
        {
            return Cursor >= Size ||
                   (customEnd != null && Cursor >= customEnd);
        }
    }
}
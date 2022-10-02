using Xrpl.BinaryCodecLib.Enums;

using System;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/serdes/binary-parser.ts#L9

namespace Xrpl.BinaryCodecLib.Binary
{
    /// <summary>
    /// BinaryParser is used to compute fields and values from a HexString
    /// </summary>
    public abstract class BinaryParser
    {
        /// <summary> Parser size </summary>
        protected internal int Size;
        /// <summary> cursor position </summary>
        protected internal int Cursor;
        /// <summary> END of parser </summary>
        /// <returns>bool result</returns>
        public bool End() => Cursor >= Size;
        /// <summary> Cursor position</summary>
        /// <returns>Cursor position</returns>
        public int Pos() => Cursor;
        /// <summary>
        /// Read an integer of given size
        /// </summary>
        /// <returns></returns>
        public int ReadOneInt() => ReadOne() & 0xFF;
        /// <summary> Consume the first n bytes of the BinaryParser </summary>
        /// <param name="n">n the number of bytes to skip</param>
        public abstract byte Peek();
        /// <summary> todo </summary>
        /// <param name="n">n the number of bytes to skip</param>
        public abstract void Skip(int n);
        /// <summary>  read the byte from the BinaryParser by current cursor position </summary>
        public abstract byte ReadOne();
        /// <summary>  read the first n bytes from the BinaryParser </summary>
        /// <param name="n">The number of bytes to read</param>
        /// <returns>The bytes</returns>
        public abstract byte[] Read(int n);

        /// <summary>
        ///  Read the field from the BinaryParser
        /// </summary>
        /// <returns>The field represented by the bytes at the head of the BinaryParser</returns>
        /// <exception cref="InvalidOperationException">Couldn't parse field </exception>
        public Field ReadField()
        {
            var fieldCode = ReadFieldCode();
            var field = Field.Values[fieldCode];
            if (field == null)
            {
                throw new InvalidOperationException(
                    "Couldn't parse field from " +
                    $"{fieldCode:x}");
            }

            return field;
        }
        /// <summary>
        /// Reads the field ordinal from the BinaryParser
        /// </summary>
        /// <returns>Field ordinal</returns>
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
        /// <summary>
        /// Reads variable length encoded bytes
        /// </summary>
        /// <returns>The variable length bytes</returns>
        /// <exception cref="InvalidOperationException">Invalid variable length indicator</exception>
        public int ReadVlLength()
        {
            var b1 = ReadOneInt();
            int result;

            switch (b1)
            {
                case <= 192: result = b1;
                    break;
                case <= 240:
                {
                    var b2 = ReadOneInt();
                    result = 193 + (b1 - 193) * 256 + b2;
                    break;
                }
                case <= 254:
                {
                    var b2 = ReadOneInt();
                    var b3 = ReadOneInt();
                    result = 12481 + (b1 - 241) * 65536 + b2 * 256 + b3;
                    break;
                }
                default:
                    throw new InvalidOperationException(
                        "Invalid variable length indicator");
            }

            return result;
        }
        /// <summary> END of parser </summary>
        /// <param name="customEnd">current cursor position</param>
        /// <returns>bool result</returns>
        public bool End(int? customEnd) => Cursor >= Size || Cursor >= customEnd;
    }
}
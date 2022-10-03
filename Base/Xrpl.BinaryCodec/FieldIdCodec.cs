using System;
using Xrpl.BinaryCodec.Definitions;

namespace Xrpl.BinaryCodec
{
    public class FieldIdCodec
    {
        private byte[] EncodeFieldId(FieldHeader field)
        {
            var fieldCode = field.fieldCode;
            var typeCode = field.typeCode;
            //var nth = NthOfType;
            //var type = Type.Ordinal;

            if (!(0 < fieldCode || fieldCode <= 255) || !(0 < typeCode || typeCode <= 255))
            {
                throw new BinaryCodecError("Codes must be nonzero and fit in 1 byte.");
            };

            if (typeCode < 16 && fieldCode < 16)
            {
                return new[] { (byte)((typeCode << 4) | fieldCode) };
            }

            if (typeCode >= 16 && fieldCode < 16)
            {
                return new[] { (byte)fieldCode, (byte)typeCode };
            }

            if (typeCode < 16 && fieldCode >= 16)
            {
                return new[] { (byte)(typeCode << 4), (byte)fieldCode };
            }
            else
            {
                return new byte[] { 0, (byte)typeCode, (byte)fieldCode };
            }
        }

        private byte[] DecodeFieldId(FieldHeader field)
        {
            var fieldCode = field.fieldCode;
            var typeCode = field.typeCode;
            //var nth = NthOfType;
            //var type = Type.Ordinal;

            if (!(0 < fieldCode || fieldCode <= 255) || !(0 < typeCode || typeCode <= 255))
            {
                throw new BinaryCodecError("Codes must be nonzero and fit in 1 byte.");
            };

            if (typeCode < 16 && fieldCode < 16)
            {
                return new[] { (byte)((typeCode << 4) | fieldCode) };
            }

            if (typeCode >= 16 && fieldCode < 16)
            {
                return new[] { (byte)fieldCode, (byte)typeCode };
            }

            if (typeCode < 16 && fieldCode >= 16)
            {
                return new[] { (byte)(typeCode << 4), (byte)fieldCode };
            }
            else
            {
                return new byte[] { 0, (byte)typeCode, (byte)fieldCode };
            }
        }
    }
}
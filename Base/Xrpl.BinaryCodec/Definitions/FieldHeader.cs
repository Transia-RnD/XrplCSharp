using Newtonsoft.Json.Linq;
using System;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Hashing;
using Xrpl.BinaryCodec.Types;

// https://github.com/XRPLF/xrpl-py/blob/master/xrpl/core/binarycodec/definitions/field_header.py

namespace Xrpl.BinaryCodec.Definitions
{
    public class FieldHeader
    {
        public int typeCode;
        public int fieldCode;

        public FieldHeader(int type, int field)
        {
            typeCode = type;
            fieldCode = field;
        }

        public bool IsEqual(FieldHeader other)
        {
            return this.typeCode == other.typeCode && this.fieldCode == other.fieldCode;
        }

        public int ToHash()
        {
            // TODO: fix this.
            //return hash((self.typeCode, self.fieldCode))
            return 0;
        }

            public byte[] ToBytes()
        {
            if (this.typeCode < 16 && this.fieldCode < 16)
            {
                return new[] { (byte)((this.typeCode << 4) | this.fieldCode) };
            }

            if (this.typeCode >= 16 && this.fieldCode < 16)
            {
                return new[] { (byte)this.fieldCode, (byte)this.typeCode };
            }

            if (this.typeCode < 16 && this.fieldCode >= 16)
            {
                return new[] { (byte)(this.typeCode << 4), (byte)this.fieldCode };
            }
            else
            {
                return new byte[] { 0, (byte)this.typeCode, (byte)this.fieldCode };
            }
        }
    }
}

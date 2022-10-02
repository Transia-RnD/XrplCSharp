using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Util;

namespace Xrpl.BinaryCodecLib.Enums
{
    public abstract class SerializedEnumItem<TOrd> : EnumItem, ISerializedType
        where TOrd : struct, IConvertible
    {
        protected readonly byte[] Bytes; 
        public void ToBytes(BytesList sink)
        {
            sink.Put(Bytes);
        }

        public JToken ToJson()
        {
            return ToString();
        }

        protected SerializedEnumItem(string name, int ordinal) : base(name, ordinal)
        {
            var width = Marshal.SizeOf(default(TOrd));
            switch (width)
            {
                case 1:
                    Bytes = new[] { (byte)ordinal };
                    break;
                case 2:
                    Bytes = Bits.GetBytes((ushort) ordinal);
                    break;
            }
        }
    }
}
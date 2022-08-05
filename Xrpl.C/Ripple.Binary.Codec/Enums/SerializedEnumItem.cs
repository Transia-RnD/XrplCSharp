using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Util;

namespace Ripple.Binary.Codec.Enums
{
    public abstract class SerializedEnumItem<TOrd> : EnumItem, ISerializedType
        where TOrd : struct, IConvertible
    {
        protected readonly byte[] Bytes; 
        public void ToBytes(IBytesSink sink) => sink.Put(Bytes);

        public JToken ToJson() => ToString();

        protected SerializedEnumItem(string name, int ordinal) : base(name, ordinal) 
            => Bytes = Marshal.SizeOf(default(TOrd)) switch
            {
                1 => new[] { (byte)ordinal },
                2 => Bits.GetBytes((ushort)ordinal),
                _ => Bytes
            };
    }
}
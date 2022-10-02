using System;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Binary;

namespace Xrpl.BinaryCodec.Enums
{
    public abstract class SerializedEnumeration<TEnum, TOrd> : Enumeration<TEnum> 
        where TEnum : SerializedEnumItem<TOrd>
        where TOrd : struct, IConvertible
    {
        protected SerializedEnumeration()
        {
            Width = Marshal.SizeOf(default(TOrd));
        }

        public int Width { get;}

        public TEnum FromParser(BinaryParser parser, int? hint = null)
        {
            return this[ReadOrdinal(parser)];
        }

        public TEnum FromJson(JToken value)
        {
            return value.Type == JTokenType.String ? 
                this[value.ToString()] : this[(int) value];
        }

        public int ReadOrdinal(BinaryParser parser)
        {
            return parser.Read(Width).Aggregate(0, (a, b) => (a >> 8) + b);
        }
    }
}
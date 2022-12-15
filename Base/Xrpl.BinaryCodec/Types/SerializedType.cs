using System;
using System.Collections.Generic;
using Xrpl.BinaryCodec.Serdes;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/serialized-type.ts

namespace Xrpl.BinaryCodec.Types
{
	public class SerializedType
	{
        public interface IJson
        {
        }

        public class JsonString : IJson
        {
            public string Value { get; set; }
        }

        public class JsonNumber : IJson
        {
            public double Value { get; set; }
        }

        public class JsonBoolean : IJson
        {
            public bool Value { get; set; }
        }

        public class JsonNull : IJson
        {
        }

        public class JsonArray : IJson
        {
            public IJson[] Value { get; set; }
        }

        public class JsonObject : IJson
        {
            public Dictionary<string, IJson> Value { get; set; }
        }

        protected byte[] bytes = new byte[0];

        public SerializedType(byte[] bytes)
        {
            this.bytes = bytes ?? new byte[0];
        }

        public static SerializedType FromParser(BinaryParser parser, int? hint = null)
        {
            throw new Exception("fromParser not implemented");
            return FromParser(parser, hint);
        }

        public static SerializedType From(SerializedType value)
        {
            throw new Exception("from not implemented");
            return From(value);
        }

        public void ToBytesSink(BytesList list)
        {
            list.Put(this.bytes);
        }

        public string ToHex()
        {
            return ToBytes().ToString("hex").ToUpper();
        }

        public byte[] ToBytes()
        {
            if (this.bytes != null)
            {
                return this.bytes;
            }
            BytesList bytes = new BytesList();
            ToBytesSink(bytes);
            return bytes.ToBytes();
        }

        public object ToJSON()
        {
            return ToHex();
        }

        public override string ToString()
        {
            return ToHex();
        }

        public class Comparable : SerializedType
        {
            public Comparable(byte[] bytes) : base(bytes)
            {
                this.bytes = bytes;
            }

            public bool Lt(Comparable other)
            {
                return CompareTo(other) < 0;
            }

            public bool Eq(Comparable other)
            {
                return CompareTo(other) == 0;
            }

            public bool Gt(Comparable other)
            {
                return CompareTo(other) > 0;
            }

            public bool Gte(Comparable other)
            {
                return CompareTo(other) > -1;
            }

            public bool Lte(Comparable other)
            {
                return CompareTo(other) < 1;
            }

            public int CompareTo(Comparable other)
            {
                throw new Exception($"cannot compare {ToString()} and {other.ToString()}");
            }
        }
    }
}


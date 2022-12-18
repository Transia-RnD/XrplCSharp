using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Util;

namespace Xrpl.BinaryCodec.Types
{
    public class SerializedType
    {
        protected byte[] Buffer;

        //public SerializedType(byte[] buffer)
        //{
        //    this.Buffer = buffer ?? new byte[0];
        //}

        //public static SerializedType FromParser(BinaryParser parser, int? hint = null)
        //{
        //    throw new Exception("fromParser not implemented");
        //    //return FromParser(parser, hint);
        //}

        //public static SerializedType From(SerializedType value)
        //{
        //    throw new Exception("from not implemented");
        //    //return From(value);
        //}

        public void ToBytesSink(BytesList list)
        {
            list.Put(Buffer);
        }

        public string ToHex()
        {
            return ToBytes().ToHex();
        }

        public byte[] ToBytes()
        {
            if (Buffer != null)
            {
                return Buffer;
            }
            var bl = new BytesList();
            ToBytesSink(bl);
            return bl.ToBytes();
        }

        public object ToJson()
        {
            return ToHex();
        }

        public override string ToString()
        {
            return ToHex();
        }
    }

    public class Comparable : SerializedType
    {
        //public Comparable(byte[] bytes) : base(bytes) {}

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

    public interface ISerializedType
    {
        /// <summary> to bytes Sink </summary>
        /// <param name="sink"> bytes Sink container</param>
        void ToBytes(IBytesSink sink);
        /// <summary> Get the JSON representation of this type </summary>
        JToken ToJson();
    }
    /// <summary> extension for ISerializedType </summary>
    public static class StExtensions
    {
        /// <summary> object to hex string </summary>
        /// <param name="st">Serialized type</param>
        /// <returns></returns>
        public static string ToHex(this ISerializedType st)
        {
            BytesList list = new BytesList();
            st.ToBytes(list);
            return list.BytesHex();
        }
        public static string ToDebuggedHex(this ISerializedType st)
        {
            BytesList list = new BytesList();
            st.ToBytes(list);
            return list.RawList().Aggregate("", (a, b) => a + ',' + B16.Encode(b));
        }
    }
}
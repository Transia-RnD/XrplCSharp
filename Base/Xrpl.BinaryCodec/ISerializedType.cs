using System.Linq;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Util;

namespace Xrpl.BinaryCodec
{

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
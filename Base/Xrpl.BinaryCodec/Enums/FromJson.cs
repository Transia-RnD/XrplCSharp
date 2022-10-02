using Newtonsoft.Json.Linq;

namespace Xrpl.BinaryCodec.Enums
{
    public delegate ISerializedType FromJson(JToken token);
}

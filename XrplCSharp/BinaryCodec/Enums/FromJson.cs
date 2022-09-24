using Newtonsoft.Json.Linq;

namespace Xrpl.BinaryCodecLib.Enums
{
    public delegate ISerializedType FromJson(JToken token);
}

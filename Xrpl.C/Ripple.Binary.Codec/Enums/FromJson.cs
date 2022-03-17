using Newtonsoft.Json.Linq;

namespace Ripple.Binary.Codec.Enums
{
    public delegate ISerializedType FromJson(JToken token);
}

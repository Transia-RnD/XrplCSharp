using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Types;

namespace Xrpl.BinaryCodec.Enums
{
    public delegate ISerializedType FromJson(JToken token);
}

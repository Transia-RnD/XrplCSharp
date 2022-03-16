using Newtonsoft.Json.Linq;

namespace Ripple.Core.Enums
{
    public delegate ISerializedType FromJson(JToken token);
}

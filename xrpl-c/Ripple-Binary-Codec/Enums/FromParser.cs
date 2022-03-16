using Ripple.Core.Binary;

namespace Ripple.Core.Enums
{
    public delegate ISerializedType FromParser(BinaryParser parser, int? hint = null);
}

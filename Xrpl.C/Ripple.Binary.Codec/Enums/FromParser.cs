using Ripple.Binary.Codec.Binary;

namespace Ripple.Binary.Codec.Enums
{
    public delegate ISerializedType FromParser(BinaryParser parser, int? hint = null);
}

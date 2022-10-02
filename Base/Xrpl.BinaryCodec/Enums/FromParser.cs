using Xrpl.BinaryCodec.Binary;

namespace Xrpl.BinaryCodec.Enums
{
    public delegate ISerializedType FromParser(BinaryParser parser, int? hint = null);
}

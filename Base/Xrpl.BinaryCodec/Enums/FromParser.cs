using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Types;

namespace Xrpl.BinaryCodec.Enums
{
    public delegate ISerializedType FromParser(BinaryParser parser, int? hint = null);
}

using Xrpl.BinaryCodecLib.Binary;

namespace Xrpl.BinaryCodecLib.Enums
{
    public delegate ISerializedType FromParser(BinaryParser parser, int? hint = null);
}

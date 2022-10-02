using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Hashing;

namespace Xrpl.BinaryCodecLib.ShaMapTree
{
    public interface IShaMapItem<out T>
    {
        void ToBytes(BytesList sink);
        IShaMapItem<T> Copy();
        T Value();
        HashPrefix Prefix();
    }
}

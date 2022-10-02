using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Hashing;

namespace Xrpl.BinaryCodec.ShaMapTree
{
    public interface IShaMapItem<out T>
    {
        void ToBytes(IBytesSink sink);
        IShaMapItem<T> Copy();
        T Value();
        HashPrefix Prefix();
    }
}

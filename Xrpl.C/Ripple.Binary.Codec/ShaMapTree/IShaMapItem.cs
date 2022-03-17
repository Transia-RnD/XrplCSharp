using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Hashing;

namespace Ripple.Binary.Codec.ShaMapTree
{
    public interface IShaMapItem<out T>
    {
        void ToBytes(IBytesSink sink);
        IShaMapItem<T> Copy();
        T Value();
        HashPrefix Prefix();
    }
}

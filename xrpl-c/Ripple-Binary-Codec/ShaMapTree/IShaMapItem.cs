using Ripple.Core.Binary;
using Ripple.Core.Hashing;

namespace Ripple.Core.ShaMapTree
{
    public interface IShaMapItem<out T>
    {
        void ToBytes(IBytesSink sink);
        IShaMapItem<T> Copy();
        T Value();
        HashPrefix Prefix();
    }
}

using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Hashing;
using Ripple.Binary.Codec.Types;

namespace Ripple.Binary.Codec.ShaMapTree
{
    public class ShaMapLeaf : ShaMapNode
    {
        public Hash256 Index;
        public IShaMapItem<object> Item;
        public long Version = -1;

        protected internal ShaMapLeaf(Hash256 index, IShaMapItem<object> item)
        {
            Index = index;
            Item = item;
        }

        public override bool IsLeaf => true;
        public override bool IsInner => false;

        internal override HashPrefix Prefix()
        {
            return Item.Prefix();
        }

        public override void ToBytesSink(IBytesSink sink)
        {
            Item.ToBytes(sink);
            Index.ToBytes(sink);
        }

        public ShaMapLeaf Copy()
        {
            return new ShaMapLeaf(Index, Item.Copy());
        }
    }
}
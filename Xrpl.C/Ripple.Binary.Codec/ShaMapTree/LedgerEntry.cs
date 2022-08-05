using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Enums;
using Ripple.Binary.Codec.Hashing;
using Ripple.Binary.Codec.Types;

namespace Ripple.Binary.Codec.ShaMapTree
{
    public class LedgerEntry : IShaMapItem<LedgerEntry>
    {
        public readonly StObject Entry;

        public LedgerEntry(StObject entry) => Entry = entry;

        public void ToBytes(IBytesSink sink) => Entry.ToBytes(sink);

        public IShaMapItem<LedgerEntry> Copy() => this;

        public LedgerEntry Value() => this;

        public HashPrefix Prefix() => HashPrefix.LeafNode;

        public Hash256 Index() => Entry[Field.index];
    }
    public static class LedgerEntryReader
    {
        public static LedgerEntry ReadLedgerEntry(this StReader reader)
        {
            var index = reader.ReadHash256();
            var obj = reader.ReadVlStObject();
            obj[Field.index] = index;
            return new LedgerEntry(obj);
        }
    }
}

using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Enums;
using Xrpl.BinaryCodecLib.Hashing;
using Xrpl.BinaryCodecLib.Types;

namespace Xrpl.BinaryCodecLib.ShaMapTree
{
    public class TransactionResult : IShaMapItem<TransactionResult>
    {
        public readonly StObject Tx;
        public readonly StObject Meta;
        public readonly uint LedgerIndex;

        public TransactionResult(StObject tx, StObject meta, uint ledgerIndex=0)
        {
            Tx = tx;
            Meta = meta;
            LedgerIndex = ledgerIndex;
        }

        public void ToBytes(BytesList sink)
        {
            var ser = new BinarySerializer(sink);
            ser.AddLengthEncoded(Tx);
            ser.AddLengthEncoded(Meta);
        }

        public IShaMapItem<TransactionResult> Copy()
        {
            return this;
        }

        public TransactionResult Value()
        {
            return this;
        }

        public HashPrefix Prefix()
        {
            return HashPrefix.TxNode;
        }

        public static TransactionResult FromJson(JToken obj)
        {
            return new TransactionResult(obj, obj["meta"]);
        }

        public Hash256 Hash()
        {
            return (Hash256)Tx[Field.hash];
        }
    }

    public static class TransactionResultReader
    {
        public static TransactionResult ReadTransactionResult(this StReader reader, uint ledgerIndex=0)
        {
            var hash = reader.ReadHash256();
            var txn = reader.ReadVlStObject();
            var meta = reader.ReadVlStObject();
            txn[Field.hash] = hash;
            return new TransactionResult(txn, meta, ledgerIndex);
        }
    }

}

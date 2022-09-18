using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Hashing;
using Ripple.Binary.Codec.Types;

namespace Ripple.Binary.Codec.Ledger
{
    public class LedgerHeader
    {
        public Uint32 LedgerIndex { get; private set; } // Ledger Sequence (0 for genesis ledger)
        public Uint64 TotalDrops { get; private set; }  //
        public Hash256 ParentHash { get; private set; }  // The hash of the previous ledger (0 for genesis ledger)
        public Hash256 TransactionHash { get; private set; } // The hash of the transaction tree's root node.
        public Hash256 StateHash { get; private set; }  // The hash of the state tree's root node.
        public Uint32 ParentCloseTime { get; private set; } // The time the previous ledger closed
        public Uint32 CloseTime { get; private set; } // UTC minute ledger closed encoded as seconds since 1/1/2000 (or 0 for genesis ledger)
        public Uint8 CloseResolution { get; private set; } // The resolution (in seconds) of the close time
        public Uint8 CloseFlags { get; private set; } // Flags

        public static LedgerHeader FromReader(StReader reader)
        {
            var pos = reader.Parser().Pos();
            // ReSharper disable once UseObjectOrCollectionInitializer
            var header = new LedgerHeader();
            header.LedgerIndex = reader.ReadUint32();
            header.TotalDrops = reader.ReadUint64();
            header.ParentHash = reader.ReadHash256();
            header.TransactionHash = reader.ReadHash256();
            header.StateHash = reader.ReadHash256();
            header.ParentCloseTime = reader.ReadUint32();
            header.CloseTime = reader.ReadUint32();
            header.CloseResolution = reader.ReadUint8();
            header.CloseFlags = reader.ReadUint8();

            if (reader.Parser().Pos() != pos + 118)
            {
                throw new InvalidOperationException();
            }
            return header;
        }

        public Hash256 Hash()
        {
            var hasher = new Sha512((uint) HashPrefix.LedgerMaster);
            ToBytes(hasher);
            return new Hash256(hasher.Finish256());
        }

        private void ToBytes(IBytesSink sink)
        {
            LedgerIndex.ToBytes(sink);
            TotalDrops.ToBytes(sink);
            ParentHash.ToBytes(sink);
            TransactionHash.ToBytes(sink);
            StateHash.ToBytes(sink);
            ParentCloseTime.ToBytes(sink);
            CloseTime.ToBytes(sink);
            CloseResolution.ToBytes(sink);
            CloseFlags.ToBytes(sink);
        }

        public JObject ToJson()
        {
            dynamic jsonObject = new JObject();
            jsonObject.ledger_index = LedgerIndex.ToJson();
            jsonObject.total_drops = TotalDrops.ToJson();
            jsonObject.parent_hash = ParentHash.ToJson();
            jsonObject.transaction_hash = TransactionHash.ToJson();
            jsonObject.state_hash = StateHash.ToJson();
            jsonObject.parent_close_time = ParentCloseTime.ToJson();
            jsonObject.close_time = CloseTime.ToJson();
            jsonObject.close_resolution = CloseResolution.ToJson();
            jsonObject.close_flags = CloseFlags.ToJson();
            return jsonObject;
        }
    }
}

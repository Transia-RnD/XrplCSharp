using Newtonsoft.Json.Linq;
using System;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Hashing;
using Xrpl.BinaryCodec.Types;

//https://xrpl.org/ledger-header.html#ledger-header

namespace Xrpl.BinaryCodec.Ledger
{
    /// <summary>
    /// Every ledger version has a unique header that describes the contents. You can look up a ledger's header information with the ledger method.
    /// </summary>
    public class LedgerHeader
    {
        /// <summary>
        ///  Ledger Sequence (0 for genesis ledger)
        /// </summary>
        public Uint32 LedgerIndex { get; private set; }
        /// <summary>
        /// The total number of drops of XRP owned by accounts in the ledger. This omits XRP that has been destroyed by transaction fees.<br/>
        /// The actual amount of XRP in circulation is lower because some accounts are "black holes" whose keys are not known by anyone.
        /// </summary>
        public Uint64 TotalDrops { get; private set; } 
        /// <summary> The hash of the previous ledger (0 for genesis ledger) </summary>
        public Hash256 ParentHash { get; private set; }
        /// <summary> The hash of the transaction tree's root node.
        /// The SHA-512Half of the transactions included in this ledger.
        /// </summary>
        public Hash256 TransactionHash { get; private set; }
        /// <summary> The hash of the state tree's root node. </summary>
        public Hash256 StateHash { get; private set; } 
        /// <summary> The time the previous ledger closed </summary>
        public Uint32 ParentCloseTime { get; private set; }
        /// <summary> UTC minute ledger closed encoded as seconds since 1/1/2000 (or 0 for genesis ledger) </summary>
        public Uint32 CloseTime { get; private set; }
        /// <summary> The resolution (in seconds) of the close time
        /// An integer in the range [2,120] indicating the maximum number of seconds by which the close_time could be rounded.
        /// </summary>
        public Uint8 CloseResolution { get; private set; }
        /// <summary> A bit-map of flags relating to the closing of this ledger. </summary>
        public Uint8 CloseFlags { get; private set; } 
        /// <summary>
        /// construct headers from reader
        /// </summary>
        /// <param name="reader">reader</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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
        /// <summary> get headers hash </summary>
        /// <returns></returns>
        public Hash256 Hash()
        {
            var hasher = new Sha512((uint) HashPrefix.LedgerMaster);
            ToBytes(hasher);
            return new Hash256(hasher.Finish256());
        }

        private void ToBytes(BytesList sink)
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
        /// <summary> headers to json </summary>
        /// <returns></returns>
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

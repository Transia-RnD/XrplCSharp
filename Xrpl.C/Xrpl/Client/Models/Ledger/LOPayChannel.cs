using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/PayChannel.ts

namespace Xrpl.Client.Models.Ledger
{
    public class LOPayChannel : BaseLedgerEntry
    {
        public LOPayChannel()
        {
            LedgerEntryType = LedgerEntryType.PayChannel;
        }

        public uint Flags { get; set; }

        public string Account { get; set; }

        public string Destination { get; set; }

        public string Amount { get; set; }

        public string Balance { get; set; }

        public string PublicKey { get; set; }

        public uint SettleDelay { get; set; }

        public string OwnerNode { get; set; }

        public string PreviousTxnID { get; set; }

        public uint PreviousTxnLgrSeq { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }

        public uint SourceTag { get; set; }

        public uint DestinationTag { get; set; }
    }
}

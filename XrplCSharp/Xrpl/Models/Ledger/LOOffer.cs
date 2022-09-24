using System;
using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models;
using Xrpl.Models.Common;
using Xrpl.Models.Transactions;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/Offer.ts

namespace Xrpl.Models.Ledger
{
    public class LOOffer : BaseLedgerEntry
    {
        
        public LOOffer()
        {
            LedgerEntryType = LedgerEntryType.Offer;
        }

        public string Account { get; set; }

        public OfferFlags Flags { get; set; }
        
        public uint Sequence { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerPays { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerGets { get; set; }

        public string BookDirectory { get; set; }

        public string BookNode { get; set; }

        public string OwnerNode { get; set; }

        [JsonProperty("PreviousTxnID")]
        public string PreviousTxnID { get; set; }

        [JsonProperty("PreviousTxnLgrSeq")]
        public uint PreviousTxnLgrSeq { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
    }
}

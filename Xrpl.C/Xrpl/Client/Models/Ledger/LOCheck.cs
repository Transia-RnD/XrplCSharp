using System;
using Newtonsoft.Json;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/Check.ts

namespace Xrpl.Client.Models.Ledger
{
    public class LOCheck : BaseLedgerEntry
    {
        public LOCheck()
        {
            LedgerEntryType = LedgerEntryType.Check;
        }

        /** The sender of the Check. Cashing the Check debits this address's balance. */
        public string Account { get; set; }
        /**
        * The intended recipient of the Check. Only this address can cash the Check,
        * using a CheckCash transaction.
        */
        public string Destination { get; set; }
        /**
        * A bit-map of boolean flags. No flags are defined for Checks, so this value
        * is always 0.
        */
        public string Flags { get; set; }
        /**
        * A hint indicating which page of the sender's owner directory links to this
        * object, in case the directory consists of multiple pages.
        */
        public string OwnerNode { get; set; }
        /**
        * The identifying hash of the transaction that most recently modified this
        * object.
        */
        public string PreviousTxnID { get; set; }
        /**
        * The index of the ledger that contains the transaction that most recently
        * modified this object.
        */
        public uint PreviousTxnLgrSeq { get; set; }
        /**
        * The maximum amount of currency this Check can debit the sender. If the
        * Check is successfully cashed, the destination is credited in the same
        * currency for up to this amount.
        */
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency SendMax { get; set; }
        /** The sequence number of the CheckCreate transaction that created this check. */
        public uint Sequence { get; set; }
        /**
        * A hint indicating which page of the destination's owner directory links to
        * this object, in case the directory consists of multiple pages.
        */
        public string DestinationNode { get; set; }
        /**
        * An arbitrary tag to further specify the destination for this Check, such
        * as a hosted recipient at the destination address.
        */
        public uint? DestinationTag { get; set; }
        /** Indicates the time after which this Check is considered expired. */
        public uint? Expiration { get; set; }
        /**
        * Arbitrary 256-bit hash provided by the sender as a specific reason or
        * identifier for this Check.
        */
        public string InvoiceID { get; set; }
        /**
        * An arbitrary tag to further specify the source for this Check, such as a
        * hosted recipient at the sender's address.
        */
        public string SourceTag { get; set; }
    }
}

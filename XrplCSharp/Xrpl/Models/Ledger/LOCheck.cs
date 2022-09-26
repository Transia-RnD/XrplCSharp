using Newtonsoft.Json;

using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/Check.ts

namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// A Check object describes a check, similar to a paper personal check,
    /// which can be cashed by its destination to get money from its sender.
    /// </summary>
    public class LOCheck : BaseLedgerEntry
    {
        public LOCheck()
        {
            LedgerEntryType = LedgerEntryType.Check;
        }

        /// <summary>
        /// The sender of the Check. Cashing the Check debits this address's balance. 
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// The intended recipient of the Check.Only this address can cash the Check,
        /// using a CheckCash transaction.
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// A bit-map of boolean flags.<br/>
        /// No flags are defined for Checks, so this value is always 0.
        /// </summary>
        public string Flags { get; set; }
        /// <summary>
        /// A hint indicating which page of the sender's owner directory links to this object,
        /// in case the directory consists of multiple pages.
        /// </summary>
        public string OwnerNode { get; set; }
        /// <summary>
        /// The identifying hash of the transaction that most recently modified this object.
        /// </summary>
        public string PreviousTxnID { get; set; }
        /// <summary>
        /// The index of the ledger that contains the transaction that most recently modified this object.
        /// </summary>
        public uint PreviousTxnLgrSeq { get; set; }
        /// <summary>
        /// The maximum amount of currency this Check can debit the sender.<br/>
        /// If the Check is successfully cashed,
        /// the destination is credited in the same currency for up to this amount.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency SendMax { get; set; }
        /// <summary>
        /// The sequence number of the CheckCreate transaction that created this check.
        /// </summary>
        public uint Sequence { get; set; }
        /// <summary>
        /// A hint indicating which page of the destination's owner directory links to this object,
        /// in case the directory consists of multiple pages.
        /// </summary>
        public string DestinationNode { get; set; }
        /// <summary>
        /// An arbitrary tag to further specify the destination for this Check,
        /// such as a hosted recipient at the destination address.
        /// </summary>
        public uint? DestinationTag { get; set; }
        /// <summary>
        /// Indicates the time after which this Check is considered expired.
        /// </summary>
        public uint? Expiration { get; set; }
        /// <summary>
        /// Arbitrary 256-bit hash provided by the sender as a specific reason or identifier for this Check.
        /// </summary>
        public string InvoiceID { get; set; }
        /// <summary>
        /// An arbitrary tag to further specify the source for this Check,
        /// such as a hosted recipient at the sender's address.
        /// </summary>
        public string SourceTag { get; set; }
    }
}

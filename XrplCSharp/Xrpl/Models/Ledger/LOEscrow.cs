using Newtonsoft.Json;

using System;

using Xrpl.ClientLib.Json.Converters;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/Escrow.ts

namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// The Escrow object type represents a held payment of XRP waiting to be executed or canceled.
    /// </summary>
    public class LOEscrow : BaseLedgerEntry
    {
        public LOEscrow()
        {
            LedgerEntryType = LedgerEntryType.Escrow;
        }


        /// <summary>
        /// The address of the owner (sender) of this held payment.<br/>
        /// This is the account that provided the XRP, and gets it back if the held payment is
        /// canceled.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// The destination address where the XRP is paid if the held payment is successful.
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// The amount of XRP, in drops, to be delivered by the held payment.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// A PREIMAGE-SHA-256 crypto-condition, as hexadecimal.<br/>
        /// If present, the EscrowFinish transaction must contain a fulfillment that satisfies this condition.<br/>
        /// https://tools.ietf.org/html/draft-thomas-crypto-conditions-02#section-8.1
        /// </summary>
        public string Condition { get; set; }
        
        /// <summary>
        /// The time after which this Escrow is considered expired.
        /// </summary>
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }

        /// <summary>
        /// The time, in seconds, since the Ripple Epoch, after which this held payment can be finished.<br/>
        /// Any EscrowFinish transaction before this time fails.
        /// </summary>
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? FinishAfter { get; set; }

        /// <summary>
        /// An arbitrary tag to further specify the source for this held payment,
        /// such as a hosted recipient at the owner's address.
        /// </summary>
        public uint? SourceTag { get; set; }

        /// <summary>
        /// An arbitrary tag to further specify the destination for this held payment,
        /// such as a hosted recipient at the destination address.
        /// </summary>
        public uint? DestinationTag { get; set; }

        /// <summary>
        /// A hint indicating which page of the owner directory links to this object,
        /// in case the directory consists of multiple pages.
        /// </summary>
        public string OwnerNode { get; set; }

        /// <summary>
        /// A hint indicating which page of the destination's owner directory links to this object,
        /// in case the directory consists of multiple pages.
        /// </summary>
        public string DestinationNode { get; set; }

        /// <summary>
        /// The identifying hash of the transaction that most recently modified this object.
        /// </summary>
        public string PreviousTxnID { get; set; }

        /// <summary>
        /// The index of the ledger that contains the transaction that most recently modified this object.
        /// </summary>
        public uint PreviousTxnLgrSeq { get; set; }

        //todo not found field Flags: number
        //A bit-map of boolean flags. No flags are defined for the Escrow type, so
        //this value is always 0.

    }
}

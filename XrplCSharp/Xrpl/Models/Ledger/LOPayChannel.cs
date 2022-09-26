using Newtonsoft.Json;

using System;

using Xrpl.ClientLib.Json.Converters;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/PayChannel.ts

namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// The PayChannel object type represents a payment channel.<br/>
    /// Payment channels enable small, rapid off-ledger payments of XRP that can be later reconciled with the consensus ledger.<br/>
    /// A payment channel holds a balance of XRP that can only be paid out to a specific destination address until the channel is closed.
    /// </summary>
    public class LOPayChannel : BaseLedgerEntry
    {
        public LOPayChannel()
        {
            LedgerEntryType = LedgerEntryType.PayChannel;
        }
        /// <summary>
        /// A bit-map of boolean flags enabled for this payment channel.<br/>
        /// Currently, the protocol defines no flags for PayChannel objects.
        /// </summary>
        public uint Flags { get; set; }
        /// <summary>
        /// The source address that owns this payment channel.<br/>
        /// This comes from the sending address of the transaction that created the channel.
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// The destination address for this payment channel.<br/>
        /// While the payment channel is open, this address is the only one that can receive XRP from the channel.<br/>
        /// This comes from the Destination field of the transaction that created the channel.
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// Total XRP, in drops, that has been allocated to this channel.<br/>
        /// This includes XRP that has been paid to the destination address.<br/>
        /// This is initially set by the transaction that created the channel and
        /// can be increased if the source address sends a PaymentChannelFund transaction.
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// Total XRP, in drops, already paid out by the channel.<br/>
        /// The difference between this value and the Amount field
        /// is how much XRP can still be paid to the destination address with PaymentChannelClaim transactions.<br/>
        /// If the channel closes, the remaining difference is returned to the source address.
        /// </summary>
        public string Balance { get; set; }
        /// <summary>
        /// Public key, in hexadecimal, of the key pair that can be used to sign claims against this channel.<br/>
        /// This can be any valid secp256k1 or Ed25519 public key.<br/>
        /// This is set by the transaction that created the channel and must match the public key used in claims against the channel.<br/>
        /// The channel source address can also send XRP from this channel to the destination without signed claims.
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// Number of seconds the source address must wait to close the channel if it still has any XRP in it.<br/>
        /// Smaller values mean that the destination address has less time to redeem any outstanding claims
        /// after the source address requests to close the channel.<br/>
        /// Can be any value that fits in a 32-bit unsigned integer (0 to 2^32-1). This is set by the transaction that creates the channel.
        /// </summary>
        public uint SettleDelay { get; set; }
        /// <summary>
        /// A hint indicating which page of the source address's owner directory links to this object,
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
        /// The mutable expiration time for this payment channel, in seconds since the Ripple Epoch.<br/>
        /// The channel is expired if this value is present and smaller than the previous ledger's close_time field.
        /// See Setting Channel Expiration for more details.
        /// </summary>
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
        /// <summary>
        /// The immutable expiration time for this payment channel, in seconds since the Ripple Epoch.<br/>
        /// This channel is expired if this value is present and smaller than the previous ledger's close_time field.<br/>
        /// This is optionally set by the transaction that created the channel, and cannot be changed.
        /// </summary>
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }
        /// <summary>
        /// An arbitrary tag to further specify the source for this payment channel useful for specifying a hosted recipient at the owner's address.
        /// </summary>
        public uint SourceTag { get; set; }
        /// <summary>
        /// An arbitrary tag to further specify the destination for this payment channel, such as a hosted recipient at the destination address.
        /// </summary>
        public uint DestinationTag { get; set; }

        //todo not found field DestinationNode?: string
        //A hint indicating which page of the destination's owner directory links to
        //this object, in case the directory consists of multiple pages.
    }
}

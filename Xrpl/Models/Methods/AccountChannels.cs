using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using Xrpl.Client.Json.Converters;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/accountChannels.ts

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The expected response from an <see cref="AccountChannelsRequest"/> .
    /// </summary>
    public class AccountChannels //todo rename to AccountChannelsResponse
    {
        /// <summary>
        /// The address of the source/owner of the payment channels.<br/>
        /// This corresponds to the account field of the request.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// Payment channels owned by this account.
        /// </summary>
        [JsonProperty("channels")]
        public List<Channel> Channels { get; set; }
        /// <summary>
        /// The limit to how many channel objects were actually returned by this request.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
        /// <summary>
        /// Server-defined value for pagination.<br/>
        /// Pass this to the next call to resume getting results where this call left off.<br/>
        /// Omitted when there are no additional pages after this one.
        /// </summary>
        [JsonProperty("marker")]
        public object Marker { get; set; }

        //todo not found BaseResponse fiends - ledger_hash: string, ledger_index: number,  validated?: boolean
        //https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/accountChannels.ts#L80
    }

    /// <summary>
    /// Payment channel owned by account.
    /// https://xrpl.org/account_channels.html
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// The owner of the channel, as an Address.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// The total amount of XRP, in drops allocated to this channel.
        /// </summary>
        [JsonProperty("amount")]
        public string Amount { get; set; }
        /// <summary>
        /// The total amount of XRP, in drops, paid out from this channel, as of the ledger version used.<br/>
        /// (You can calculate the amount of XRP left in the channel by subtracting balance from amount.)
        /// </summary>
        [JsonProperty("balance")]
        public string Balance { get; set; }
        /// <summary>
        /// A unique ID for this channel, as a 64-character hexadecimal string.<br/>
        /// This is also the ID of the channel object in the ledger's state data.
        /// </summary>
        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }

        /// <summary>
        /// The destination account of the channel, as an Address.<br/>
        /// Only this account can receive the XRP in the channel while it is open.
        /// </summary>
        [JsonProperty("destination_account")]
        public string DestinationAccount { get; set; }
        /// <summary>
        /// (May be omitted) The public key for the payment channel in the XRP Ledger's base58 format.<br/>
        /// Signed claims against this channel must be redeemed with the matching key pair.
        /// </summary>
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
        /// <summary>
        /// (May be omitted) The public key for the payment channel in hexadecimal format, if one was specified at channel creation.<br/>
        /// Signed claims against this channel must be redeemed with the matching key pair.
        /// </summary>
        [JsonProperty("public_key_hex")]
        public string PublicKeyHex { get; set; }
        /// <summary>
        /// The number of seconds the payment channel must stay open after the owner of the channel requests to close it.
        /// </summary>
        [JsonProperty("settle_delay")]
        public uint SettleDelay { get; set; }
        /// <summary>
        /// (May be omitted) Time, in seconds since the Ripple Epoch, when this channel is set to expire.<br/>
        /// This expiration date is mutable.<br/>
        /// If this is before the close time of the most recent validated ledger, the channel is expired.
        /// </summary>
        [JsonProperty("expiration")]
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
        /// <summary>
        /// (May be omitted) Time, in seconds since the Ripple Epoch, of this channel's immutable expiration, if one was specified at channel creation.<br/>
        /// If this is before the close time of the most recent validated ledger, the channel is expired.
        /// </summary>
        [JsonProperty("cancel_after")]
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }
        /// <summary>
        /// (May be omitted) A 32-bit unsigned integer to use as a source tag for payments through this payment channel,
        /// if one was specified at channel creation.<br/>
        /// This indicates the payment channel's originator or other purpose at the source account.<br/>
        /// Conventionally, if you bounce payments from this channel, you should specify this value in the DestinationTag of the return payment.
        /// </summary>
        [JsonProperty("source_tag")]
        public uint? SourceTag { get; set; }
        /// <summary>
        /// (May be omitted) A 32-bit unsigned integer to use as a destination tag for payments through this channel, if one was specified at channel creation.<br/>
        /// This indicates the payment channel's beneficiary or other purpose at the destination account.
        /// </summary>
        [JsonProperty("destination_tag")]
        public uint? DestinationTag { get; set; }        
    }

    /// <summary>
    /// The account_channels method returns information about an account's Payment Channels.<br/>
    /// This includes only channels where the specified account is the channel's source, not the destination.<br/>
    /// (A channel's "source" and "owner" are the same.)<br/>
    /// All information retrieved is relative to a particular version of the ledger.<br/>
    /// Returns an <see cref="AccountChannels"/>.
    /// </summary>
    /// <code>
    /// {
    /// 	"id": 1,
    /// 	"command": "account_channels",
    /// 	"account": "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn",
    /// 	"destination_account": "ra5nK24KXen9AHvsdFTKHSANinZseWnPcX",
    /// 	"ledger_index": "validated"
    /// }
    /// </code>
    public class AccountChannelsRequest : BaseLedgerRequest
    {
        public AccountChannelsRequest(string account)
        {
            Account = account;
            Command = "account_channels";
        }
        /// <summary>
        /// The unique identifier of an account, typically the account's address.<br/>
        /// The request returns channels where this account is the channel's owner/source.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// The unique identifier of an account, typically the account's address.<br/>
        /// If provided, filter results to payment channels whose destination is this account.
        /// </summary>
        [JsonProperty("destination_account")]
        public string DestinationAccount { get; set; }
        /// <summary>
        /// Limit the number of transactions to retrieve.<br/>
        /// Cannot be less than 10 or more than 400. The default is 200.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
        /// <summary>
        /// Value from a previous paginated response.<br/>
        /// Resume retrieving data where that response left off.
        /// </summary>
        [JsonProperty("marker")]
        public object Marker { get; set; }
    }
}

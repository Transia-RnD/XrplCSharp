using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;

namespace Xrpl.Models.Ledger
{
    public class LOAmm : BaseLedgerEntry
    {
        public LOAmm()
        {
            LedgerEntryType = LedgerEntryType.AccountRoot;
        }
        /// <summary>
        /// The account that tracks the balance of LPTokens between the AMM instance via Trustline.
        /// </summary>
        public string AMMAccount { get; set; }
        /// <summary>
        /// Specifies one of the pool assets (XRP or token) of the AMM instance.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Asset { get; set; }
        /// <summary>
        /// Specifies the other pool asset of the AMM instance.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Asset2 { get; set; }
        /// <summary>
        /// Details of the current owner of the auction slot.
        /// </summary>
        public AuctionSlot AuctionSlot { get; set; }
        /// <summary>
        /// The total outstanding balance of liquidity provider tokens from this AMM instance.<br/>
        /// The holders of these tokens can vote on the AMM's trading fee in proportion to their holdings,
        /// or redeem the tokens for a share of the AMM's assets which grows with the trading fees collected.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency LPTokenBalance { get; set; }
        /// <summary>
        /// Specifies the fee, in basis point, to be charged to the traders for the trades
        /// executed against the AMM instance.<br/>
        /// Trading fee is a percentage of the trading volume.<br/>
        /// Valid values for this field are between 0 and 1000 inclusive.<br/>
        /// A value of 1 is equivalent to 1/10 bps or 0.001%, allowing trading fee
        /// between 0% and 1%.<br/>
        /// This field is required.
        /// </summary>
        public uint TradingFee { get; set; }
        /// <summary>
        /// A list of vote objects, representing votes on the pool's trading fee..
        /// </summary>
        public List<VoteEntry> VoteSlots { get; set; }
        /// <summary>
        /// The ledger index of the current in-progress ledger, which was used when
        /// retrieving this information.
        /// </summary>
        public int? LedgerCurrentIndex { get; set; }
        /// <summary>
        /// True if this data is from a validated ledger version;<br/>
        /// if omitted or set to false, this data is not final.
        /// </summary>
        public bool? Validated { get; set; }

    }

    public interface IAuthAccount
    {
        public string Account { get; set; }
    }
    public class AuthAccount : IAuthAccount
    {
        [JsonProperty("account")]
        public string Account { get; set; }
    }

    public interface IVoteEntry
    {
        public string Account { get; set; }
        public uint TradingFee { get; set; }
        public uint VoteWeight { get; set; }
    }
    public class VoteEntry : IVoteEntry
    {
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("trading_fee")]
        public uint TradingFee { get; set; }
        [JsonProperty("vote_weight")]
        public uint VoteWeight { get; set; }
    }
    /// <summary>
    /// Details of the current owner of the auction slot.
    /// </summary>
    public class AuctionSlot
    {
        /// <summary>
        /// The current owner of this auction slot.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// A list of at most 4 additional accounts that are authorized to trade at the discounted fee for this AMM instance.
        /// </summary>
        [JsonProperty("auth_accounts")]
        public List<AuthAccount> AuthAccounts { get; set; }
        /// <summary>
        /// The trading fee to be charged to the auction owner, in the same format as TradingFee.<br/>
        /// By default this is 0, meaning that the auction owner can trade at no fee instead of the standard fee for this AMM.
        /// </summary>
        [JsonProperty("discounted_fee")]
        public uint DiscountedFee { get; set; }
        /// <summary>
        /// The time when this slot expires, in seconds since the Ripple Epoch.
        /// </summary>
        [JsonProperty("expiration")]
        public DateTime? Expiration { get; set; }
        /// <summary>
        /// The amount the auction owner paid to win this slot, in LPTokens.
        /// </summary>
        [JsonProperty("price")]
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Price { get; set; }
        /// <summary>
        /// The current 72-minute time interval this auction slot is in, from 0 to 19.
        /// The auction slot expires after 24 hours (20 intervals of 72 minutes)
        /// and affects the cost to outbid the current holder and how much the current holder is refunded if someone outbids them.
        /// </summary>
        [JsonProperty("time_interval")]
        public uint TimeInterval { get; set; }
    }

}
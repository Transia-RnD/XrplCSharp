using System.Collections.Generic;

using Xrpl.BinaryCodec.Types;
using Xrpl.Models.Subscriptions;

using static Xrpl.Models.Common.Common;

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The `amm_info` command retrieves information about an AMM instance.
    /// </summary>
    /// Returns an <see cref="AMMInfoResponse"/>.
    public class AMMInfoRequest : BaseLedgerRequest
    {
        public AMMInfoRequest()
        {
            Command = "amm_info";

        }
        /// <summary>
        /// Specifies one of the pool assets (XRP or token) of the AMM instance.<br/>
        /// Both asset and asset2 must be defined to specify an AMM instance.
        /// </summary>
        public Issue Asset { get; set; }
        /// <summary>
        /// Specifies the other pool asset of the AMM instance.<br/>
        /// Both asset and asset2 must be defined to specify an AMM instance.
        /// </summary>
        public Issue Asset2 { get; set; }
    }

    public interface IAuthAccount
    {
        public string Account { get; set; }
    }
    public class AuthAccount : IAuthAccount
    {
        public string Account { get; set; }
    }

    public interface IVoteEntry
    {
        public string Account { get; set; }
        public double TradingFee { get; set; }
        public double VoteWeight { get; set; }
    }
    public class VoteEntry : IVoteEntry
    {
        public string Account { get; set; }
        public double TradingFee { get; set; }
        public double VoteWeight { get; set; }
    }
    /// <summary>
    /// Details of the current owner of the auction slot.
    /// </summary>
    public class AuctionSlot
    {
        /// <summary>
        /// The current owner of this auction slot.
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// A list of at most 4 additional accounts that are authorized to trade at the discounted fee for this AMM instance.
        /// </summary>
        public List<IAuthAccount> AuthAccounts { get; set; }
        /// <summary>
        /// The trading fee to be charged to the auction owner, in the same format as TradingFee.<br/>
        /// By default this is 0, meaning that the auction owner can trade at no fee instead of the standard fee for this AMM.
        /// </summary>
        public double DiscountedFee { get; set; }
        /// <summary>
        /// The time when this slot expires, in seconds since the Ripple Epoch.
        /// </summary>
        public string Expiration { get; set; }
        /// <summary>
        /// The amount the auction owner paid to win this slot, in LPTokens.
        /// </summary>
        public Amount Price { get; set; }
    }
    /// <summary>
    /// Response expected from an <see cref="AMMInfoRequest"/>.
    /// </summary>
    public class AMMInfoResponse : BaseResponse
    {
        /// <summary>
        /// The account that tracks the balance of LPTokens between the AMM instance via Trustline.
        /// </summary>
        public string AMMAccount { get; set; }
        /// <summary>
        /// Specifies one of the pool assets (XRP or token) of the AMM instance.
        /// </summary>
        public Issue Asset { get; set; }
        /// <summary>
        /// Specifies the other pool asset of the AMM instance.
        /// </summary>
        public Issue Asset2 { get; set; }
        /// <summary>
        /// Details of the current owner of the auction slot.
        /// </summary>
        public AuctionSlot AuctionSlot { get; set; }
        /// <summary>
        /// The total outstanding balance of liquidity provider tokens from this AMM instance.<br/>
        /// The holders of these tokens can vote on the AMM's trading fee in proportion to their holdings,
        /// or redeem the tokens for a share of the AMM's assets which grows with the trading fees collected.
        /// </summary>
        public IssuedCurrencyAmount LPTokenBalance { get; set; }
        /// <summary>
        /// Specifies the fee, in basis point, to be charged to the traders for the trades
        /// executed against the AMM instance.<br/>
        /// Trading fee is a percentage of the trading volume.<br/>
        /// Valid values for this field are between 0 and 1000 inclusive.<br/>
        /// A value of 1 is equivalent to 1/10 bps or 0.001%, allowing trading fee
        /// between 0% and 1%.<br/>
        /// This field is required.
        /// </summary>
        public double TradingFee { get; set; }
        /// <summary>
        /// Keeps a track of up to eight active votes for the instance.
        /// </summary>
        public List<IVoteEntry> VoteSlots { get; set; }
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
}

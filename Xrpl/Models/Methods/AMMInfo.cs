using System.Collections.Generic;
using Newtonsoft.Json;

using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Subscriptions;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/AMM.ts
namespace Xrpl.Models.Methods;

/// <summary>
/// The `amm_info` command retrieves information about an AMM instance.
/// </summary>
/// Returns an <see cref="AMMInfoResponse"/>.
public class AMMInfoRequest : BaseLedgerRequest
{
    public AMMInfoRequest() { Command = "amm_info"; }


    /// <summary>
    /// Show only LP Tokens held by this liquidity provider.
    /// </summary>
    [JsonProperty("account")]
    public string? Account { get; set; }

    /// <summary>
    /// The address of the AMM's special AccountRoot. (This is the issuer of the AMM's LP Tokens.)
    /// </summary>
    [JsonProperty("amm_account")]
    public string? AmmAccount { get; set; }

    /// <summary>
    /// Specifies one of the pool assets (XRP or token) of the AMM instance.<br/>
    /// Both asset and asset2 must be defined to specify an AMM instance.
    /// </summary>
    [JsonProperty("asset"), JsonConverter(typeof(IssuedCurrencyConverter)),]
    public Common.Common.IssuedCurrency Asset { get; set; }

    /// <summary>
    /// Specifies the other pool asset of the AMM instance.<br/>
    /// Both asset and asset2 must be defined to specify an AMM instance.
    /// </summary>
    [JsonProperty("asset2"), JsonConverter(typeof(IssuedCurrencyConverter)),]
    public Common.Common.IssuedCurrency Asset2 { get; set; }
}

/// <summary>
/// Response expected from an <see cref="AMMInfoRequest"/>.
/// </summary>
public class AMMInfoResponse
{
    [JsonProperty("amm")]
    public AMMInfo Amm { get; set; }
    /// <summary>
    /// The ledger index of the ledger version that was used to generate this response.
    /// </summary>
    [JsonProperty("ledger_index")]
    public int? LedgerIndex { get; set; }

    /// <summary>
    ///The identifying hash of the ledger that was used to generate this response.
    /// </summary>
    [JsonProperty("ledger_hash")]
    public string? LedgerHash { get; set; }

    /// <summary>
    /// True if this data is from a validated ledger version;<br/>
    /// if omitted or set to false, this data is not final.
    /// </summary>
    [JsonProperty("validated")]
    public bool? Validated { get; set; }
}

public class AMMInfo
{
    /// <summary>
    /// The account that tracks the balance of LPTokens between the AMM instance via Trustline.
    /// </summary>
    [JsonProperty("account")]
    public string Account { get; set; }

    /// <summary>
    /// Specifies one of the pool assets (XRP or token) of the AMM instance.
    /// </summary>
    [JsonProperty("amount"), JsonConverter(typeof(CurrencyConverter)),]
    public Currency Amount { get; set; }

    /// <summary>
    /// Specifies the other pool asset of the AMM instance.
    /// </summary>
    [JsonProperty("amount2"), JsonConverter(typeof(CurrencyConverter))]
    public Currency Amount2 { get; set; }

    [JsonProperty("asset_frozen"),]
    public bool? AssetFrozen { get; set; }

    [JsonProperty("asset2_frozen")]
    public bool? Asset2Frozen { get; set; }

    /// <summary>
    /// Details of the current owner of the auction slot.
    /// </summary>
    [JsonProperty("auction_slot")]
    public AuctionSlot AuctionSlot { get; set; }

    /// <summary>
    /// The total outstanding balance of liquidity provider tokens from this AMM instance.<br/>
    /// The holders of these tokens can vote on the AMM's trading fee in proportion to their holdings,
    /// or redeem the tokens for a share of the AMM's assets which grows with the trading fees collected.
    /// </summary>
    [JsonProperty("lp_token"), JsonConverter(typeof(CurrencyConverter)),]
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
    [JsonProperty("trading_fee")]
    public uint TradingFee { get; set; }

    /// <summary>
    /// Keeps a track of up to eight active votes for the instance.
    /// </summary>
    [JsonProperty("vote_slots")]
    public List<VoteEntry> VoteSlots { get; set; }
}

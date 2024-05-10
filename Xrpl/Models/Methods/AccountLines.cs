using System.Collections.Generic;
using System.Globalization;

using Newtonsoft.Json;

using Xrpl.Client.Extensions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/accountLines.ts

namespace Xrpl.Models.Methods;

/// <summary>
/// Response expected from an <see cref="AccountLinesRequest"/>.
/// </summary>
public class AccountLines //todo rename to AccountLinesResponse
{
    /// <summary>
    /// Unique Address of the account this request corresponds to.<br/>
    /// This is the  "perspective account" for purpose of the trust lines.
    /// </summary>
    [JsonProperty(propertyName: "account")]
    public string Account { get; set; }

    /// <summary>
    /// Array of trust line objects.<br/>
    /// If the number of trust lines is large, only  returns up to the limit at a time.
    /// </summary>
    [JsonProperty(propertyName: "lines")]
    public List<TrustLine> TrustLines { get; set; }

    /// <summary>
    /// The ledger index of the current open ledger, which was used when  retrieving this information.
    /// </summary>
    [JsonProperty(propertyName: "ledger_current_index")]
    public uint? LedgerCurrentIndex { get; set; }

    /// <summary>
    /// The ledger index of the ledger version that was used when retrieving  this data.
    /// </summary>
    [JsonProperty(propertyName: "ledger_index")]
    public uint? LedgerIndex { get; set; }

    /// <summary>
    /// The identifying hash the ledger version that was used when retrieving  this data.
    /// </summary>
    [JsonProperty(propertyName: "ledger_hash")]
    public string LedgerHash { get; set; }

    /// <summary>
    /// Server-defined value indicating the response is paginated.<br/>
    /// Pass this to  the next call to resume where this call left off.<br/>
    /// Omitted when there are  No additional pages after this one.
    /// </summary>
    [JsonProperty(propertyName: "marker")]
    public object Marker { get; set; }
}

/// <summary>
/// Trust line objects.
/// </summary>
public class TrustLine
{
    /// <summary>
    /// The unique Address of the counterparty to this trust line.
    /// </summary>
    [JsonProperty(propertyName: "account")]
    public string Account { get; set; }

    /// <summary>
    /// Representation of the numeric balance currently held against this line.<br/>
    /// A positive balance means that the perspective account holds value;<br/>
    /// a negative Balance means that the perspective account owes value.
    /// </summary>
    [JsonProperty(propertyName: "balance")]
    public string Balance { get; set; }

    /// <summary>
    /// Representation of the numeric balance currently held against this line.<br/>
    /// A positive balance means that the perspective account holds value;<br/>
    /// a negative Balance means that the perspective account owes value.
    /// </summary>
    [JsonIgnore]
    public decimal BalanceAsNumber => decimal.Parse(
        Balance,
        NumberStyles.AllowLeadingSign
        | (NumberStyles.AllowLeadingSign & NumberStyles.AllowDecimalPoint)
        | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent)
        | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
        | (NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
        | NumberStyles.AllowExponent
        | NumberStyles.AllowDecimalPoint,
        CultureInfo.InvariantCulture);

    /// <summary>
    /// A Currency Code identifying what currency this trust line can hold.
    /// </summary>
    [JsonProperty(propertyName: "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Readable currency name 
    /// </summary>
    [JsonIgnore]
    public string CurrencyValidName => Currency is { Length: > 0, } row
        ? row.Length > 3 ? row.StartsWith(value: "03") ? $"LP {row[2..6]}.." : row.FromHexString().Trim(trimChar: '\0') : row
        : string.Empty;

    /// <summary>
    /// The maximum amount of currency that the issuer account is willing to owe the perspective account. 
    /// </summary>
    [JsonProperty(propertyName: "limit")]
    public string Limit { get; set; }

    /// <summary>
    /// The maximum amount of currency that the issuer account is willing to owe the perspective account. 
    /// </summary>
    [JsonIgnore]
    public double LimitAsNumber => double.Parse(
        Limit,
        NumberStyles.AllowLeadingSign
        | (NumberStyles.AllowLeadingSign & NumberStyles.AllowDecimalPoint)
        | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent)
        | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
        | (NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
        | NumberStyles.AllowExponent
        | NumberStyles.AllowDecimalPoint,
        CultureInfo.InvariantCulture);

    /// <summary>
    /// The maximum amount of currency that the issuer account is willing to owe the perspective account.
    /// </summary>
    [JsonProperty(propertyName: "limit_peer")]
    public string LimitPeer { get; set; }

    [JsonIgnore]
    public double LimitPeerAsNumber => double.Parse(
        LimitPeer,
        NumberStyles.AllowLeadingSign
        | (NumberStyles.AllowLeadingSign & NumberStyles.AllowDecimalPoint)
        | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent)
        | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
        | (NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
        | NumberStyles.AllowExponent
        | NumberStyles.AllowDecimalPoint,
        CultureInfo.InvariantCulture);

    /// <summary>
    /// Rate at which the account values incoming balances on this trust line, as a ratio of this value per 1 billion units.<br/>
    /// (For example, a value of 500 million represents a 0.5:1 ratio.)<br/>
    /// As a special case, 0 is treated as a 1:1 ratio.
    /// </summary>
    [JsonProperty(propertyName: "quality_in")]
    public uint QualityIn { get; set; }

    /// <summary>
    /// Rate at which the account values outgoing balances on this trust line, as a ratio of this value per 1 billion units.<br/>
    /// (For example, a value of 500 million represents a 0.5:1 ratio.)<br/>
    /// As a special case, 0 is treated as a 1:1 ratio.
    /// </summary>
    [JsonProperty(propertyName: "quality_out")]
    public uint QualityOut { get; set; }

    /// <summary>
    /// If true, this account has enabled the No Ripple flag for this trust line.<br/>
    /// If present and false, this account has disabled the No Ripple flag, but,
    /// because the account also has the Default Ripple flag enabled, that is not considered the default state.<br/>
    /// If omitted, the account has the No Ripple flag disabled for this trust line and Default Ripple disabled.
    /// </summary>
    [JsonProperty(propertyName: "no_ripple")]
    public bool? NoRipple { get; set; }

    /// <summary>
    /// If true, the peer account has enabled the No Ripple flag for this trust line.<br/>
    /// If present and false, this account has disabled the No Ripple flag, but,
    /// because the account also has the Default Ripple flag enabled, that is not considered the default state.<br/>
    /// If omitted, the account has the No Ripple flag disabled for this trust line and Default Ripple disabled.
    /// </summary>
    [JsonProperty(propertyName: "no_ripple_peer")]
    public bool? NoRipplePeer { get; set; }

    /// <summary>
    /// If true, this account has frozen this trust line. The default is false. 
    /// </summary>
    [JsonProperty(propertyName: "freeze")]
    public bool? Freeze { get; set; }

    /// <summary>
    /// If true, the peer account has frozen this trust line.<br/>
    /// The default is false.
    /// </summary>
    [JsonProperty(propertyName: "freeze_peer")]
    public bool? FreezePeer { get; set; }

    //todo not found fields - authorized?: boolean, peer_authorized?: boolean
}

/// <summary>
/// The account_lines method returns information about an account's trust lines,
/// including balances in all non-XRP currencies and assets.<br/>
/// All information  retrieved is relative to a particular version of the ledger.<br/>
/// Expects an <see cref="AccountLines"/>.
/// </summary>
/// <code>
/// {
/// 	"id": 1,
/// 	"command": "account_lines",
/// 	"account": "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59"
/// }
/// </code>
public class AccountLinesRequest : BaseLedgerRequest
{
    public AccountLinesRequest(string account)
    {
        Account = account;
        Command = "account_lines";
    }

    /// <summary>
    /// A unique identifier for the account, most commonly the account's Address.
    /// </summary>
    [JsonProperty(propertyName: "account")]
    public string Account { get; set; }

    /// <summary>
    /// The Address of a second account.
    /// If provided, show only lines of trust connecting the two accounts.
    /// </summary>
    [JsonProperty(propertyName: "peer")]
    public string Peer { get; set; }

    /// <summary>
    /// Limit the number of trust lines to retrieve.<br/>
    /// The server is not required to honor this value.<br/>
    /// Must be within the inclusive range 10 to 400.
    /// </summary>
    [JsonProperty(propertyName: "limit")]
    public int? Limit { get; set; } = 10;

    /// <summary>
    /// Value from a previous paginated response.<br/>
    /// Resume retrieving data where that response left off.
    /// </summary>
    [JsonProperty(propertyName: "marker")]
    public object Marker { get; set; }
}
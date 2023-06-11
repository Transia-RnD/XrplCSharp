using Newtonsoft.Json;

using System.Collections.Generic;
using Xrpl.Client.Extensions;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/accountNFTs.ts

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// Response expected from an <see cref="AccountNFTsRequest"/>.
    /// </summary>
    public class AccountNFTs //todo rename to response
    {
        /// <summary>
        /// The account requested.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// A list of NFTs owned by the specified account.
        /// </summary>
        [JsonProperty("account_nfts")]
        public List<NFT> NFTs { get; set; }
        /// <summary>
        /// The ledger index of the current open ledger, which was used when  retrieving this information.
        /// </summary>
        [JsonProperty("ledger_current_index")]
        public uint? LedgerCurrentIndex { get; set; }
        /// <summary>
        /// If true, this data comes from a validated ledger.
        /// </summary>
        [JsonProperty("validated")]
        public bool Validated { get; set; }
        /// <summary>
        /// The limit that was used to fulfill this request.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
        /// <summary>
        /// Server-defined value indicating the response is paginated.<br/>
        /// Pass this to  the next call to resume where this call left off.<br/>
        /// Omitted when there are  No additional pages after this one.
        /// </summary>
        [JsonProperty("marker")]
        public object Marker { get; set; }
    }
    /// <summary>
    /// One NFToken that might be returned from an <see cref="AccountNFTsRequest"/>.
    /// https://xrpl.org/account_nfts.html#account_nfts
    /// </summary>
    public class NFT
    {
        /// <summary>
        /// A bit-map of boolean flags enabled for this NFToken.<br/>
        /// See NFToken Flags for possible values.
        /// </summary>
        [JsonProperty("Flags")]
        public string Flags { get; set; }
        /// <summary>
        /// The TransferFee value specifies the percentage fee, in units of 1/100,000, charged by the issuer for secondary sales of the token.
        /// Valid values for this field are between 0 and 50,000, inclusive.
        /// A value of 1 is equivalent to 0.001% or 1/10 of a basis point (bps), allowing transfer rates between 0% and 50%.
        /// </summary>
        [JsonProperty("TransferFee")]
        public string TransferFee { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; } //todo unknown field
        /// <summary>
        /// The account that issued this NFToken.
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
        /// <summary>
        /// The unique identifier of this NFToken, in hexadecimal.
        /// </summary>
        [JsonProperty("NFTokenID")]
        public string NFTokenID { get; set; }
        /// <summary>
        /// The unscrambled version of this token's taxon. Several tokens with the same taxon might represent instances of a limited series.
        /// </summary>
        [JsonProperty("token_taxon")]
        public uint NFTokenTaxon { get; set; }
        /// <summary>
        /// The URI data associated with this NFToken, in hexadecimal.
        /// </summary>
        [JsonProperty("URI")]
        public string URI { get; set; }

        [JsonIgnore]
        public string URIAsString => URI.FromHexString();

        [JsonProperty("transaction_fee")]
        public uint TransactionFee { get; set; }
        /// <summary>
        /// The token sequence number of this NFToken, which is unique for its issuer
        /// </summary>
        [JsonProperty("nft_serial")]
        public string NFTSerial { get; set; }
    }
    /// <summary>
    /// The `account_nfts` method retrieves all of the NFTs currently owned by the  specified account.
    /// </summary>
    /// <code>
    /// {
    /// 	"command": "account_nfts",
    /// 	"account": "rsuHaTvJh1bDmDoxX9QcKP7HEBSBt4XsHx",
    /// 	"ledger_index": "validated"
    /// }
    /// </code>
    public class AccountNFTsRequest : BaseLedgerRequest
    {
        public AccountNFTsRequest(string account)
        {
            Account = account;
            Command = "account_nfts";
        }
        /// <summary>
        /// The unique identifier of an account, typically the account's address.<br/>
        /// The request returns NFTs owned by this account.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// Limit the number of NFTokens to retrieve.
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

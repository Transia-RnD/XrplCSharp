using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Extensions;
using XrplCSharp.Xrpl.Utils;

namespace Xrpl.Client.Models.Methods
{
    public class AccountNFTs
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("account_nfts")]
        public List<NFT> NFTs { get; set; }

        [JsonProperty("ledger_current_index")]
        public uint? LedgerCurrentIndex { get; set; }

        [JsonProperty("validated")]
        public bool Validated { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("marker")]
        public object Marker { get; set; }
    }

    public class NFT
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("nft_id")]
        public string NFTokenID { get; set; }

        [JsonProperty("token_taxon")]
        public uint NFTokenTaxon { get; set; }

        [JsonProperty("uri")]
        public string URI { get; set; }

        [JsonIgnore]
        public string URIAsString => URI.FromHexString();

        [JsonProperty("transaction_fee")]
        public uint TransactionFee { get; set; }

        [JsonProperty("nft_serial")]
        public string NFTSerial { get; set; }
    }

    public class AccountNFTsRequest : BaseLedgerRequest
    {
        public AccountNFTsRequest(string account)
        {
            Account = account;
            Command = "account_nfts";
        }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("marker")]
        public object Marker { get; set; }
    }
}

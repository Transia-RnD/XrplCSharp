using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Xrpl.Client.Requests.Account
{
    public class AccountChannelsRequest : BaseLedgerRequest
    {
        public AccountChannelsRequest(string account)
        {
            Account = account;
            Command = "account_channels";
        }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("destination_account")]
        public string DestinationAccount { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("marker")]
        public object Marker { get; set; }
    }
}

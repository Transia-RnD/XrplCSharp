using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RippleDotNet.Model;

namespace RippleDotNet.Requests.Transaction
{
    public class BookOffersRequest : BaseLedgerRequest
    {
        public BookOffersRequest()
        {
            Command = "book_offers";
        }

        [JsonProperty("limit")]
        public uint? Limit { get; set; }

        [JsonProperty("taker")]
        public string Taker { get; set; }

        [JsonProperty("taker_gets")]
        public Currency TakerGets { get; set; }

        [JsonProperty("taker_pays")]
        public Currency TakerPays { get; set; }
    }
}

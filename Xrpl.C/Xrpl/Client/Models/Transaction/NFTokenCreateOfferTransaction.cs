using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Models.Common;

namespace Xrpl.Client.Models.Transactions
{
    [Flags]
    public enum NFTokenCreateOfferFlags : uint
    {
        tfSellToken = 1
    }
    public class NFTokenCreateOfferTransaction : TransactionCommon, INFTokenCreateOfferTransaction
    {
        public NFTokenCreateOfferTransaction()
        {
            TransactionType = TransactionType.NFTokenCreateOffer;
        }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }

        public new NFTokenCreateOfferFlags? Flags { get; set; }

        public string NFTokenID { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        public string Owner { get; set; }

        public string Destination { get; set; }
    }

    public interface INFTokenCreateOfferTransaction : ITransactionCommon
    {
        DateTime? Expiration { get; set; }
        new NFTokenCreateOfferFlags? Flags { get; set; }
        string NFTokenID { get; set; }
        Currency Amount { get; set; }
        string Owner { get; set; }
        string Destination { get; set; }
    }

    public class NFTokenCreateOfferTransactionResponse : TransactionResponseCommon, INFTokenCreateOfferTransaction
    {
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }

        public new NFTokenCreateOfferFlags? Flags { get; set; }

        public string NFTokenID { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        public string Owner { get; set; }

        public string Destination { get; set; }

    }
}

using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;

namespace Xrpl.Models.Transactions
{

    /// <summary>
    /// This information is added to Transactions in request responses, but is not part  of the canonical Transaction information on ledger.<br/>
    /// These fields are denoted with  lowercase letters to indicate this in the rippled responses.
    /// </summary>
    public interface IBaseTransactionResponse
    {
        /// <summary>
        /// The date/time when this transaction was included in a validated ledger.
        /// </summary>
        [JsonConverter(typeof(RippleDateTimeConverter))]
        [JsonProperty("date")]
        DateTime? Date { get; set; }

        /// <summary>
        /// An identifying hash value unique to this transaction, as a hex string.
        /// </summary>
        [JsonProperty("hash")]
        string Hash { get; set; }

        [JsonProperty("inLedger")]
        uint? InLedger { get; set; }

        /// <summary>
        /// The sequence number of the ledger that included this transaction.
        /// </summary>
        [JsonProperty("ledger_index")]
        uint? LedgerIndex { get; set; }

        [JsonProperty("validated")]
        bool? Validated { get; set; }
    }

    /// <inheritdoc />
    public class BaseTransactionResponse : IBaseTransactionResponse
    {
        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        /// <inheritdoc />
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("inLedger")]
        public uint? InLedger { get; set; }

        /// <inheritdoc />
        [JsonProperty("ledger_index")]
        public uint? LedgerIndex { get; set; }

        [JsonProperty("validated")]
        public bool? Validated { get; set; }
    }
}

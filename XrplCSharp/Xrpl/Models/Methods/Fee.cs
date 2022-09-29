using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/fee.ts

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The `fee` command reports the current state of the open-ledger requirements  for the transaction cost.<br/>
    /// This requires the FeeEscalation amendment to be  enabled.<br/>
    /// Expects a response in the form of a <see cref="Fee"/>.
    /// </summary>
    public class FeeRequest : RippleRequest
    {
        public FeeRequest()
        {
            Command = "fee";
        }
    }
    /// <summary>
    /// Response expected from a  <see cref="FeeRequest"/>.
    /// </summary>
    public class Fee //todo rename to FeeResponse : BaseResponse
    {
        /// <summary>
        /// Number of transactions provisionally included in the in-progress ledger.
        /// </summary>
        [JsonProperty("current_ledger_size")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint CurrentLedgerSize { get; set; }
        /// <summary>
        /// Number of transactions currently queued for the next ledger.
        /// </summary>
        [JsonProperty("current_queue_size")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint CurrentQueueSize { get; set; }
        /// <summary>
        /// The transaction cost 
        /// </summary>
        [JsonProperty("drops")]
        public Drops Drops { get; set; }
        /// <summary>
        /// The approximate number of transactions expected to be included in the  current ledger.<br/>
        /// This is based on the number of transactions in the  previous ledger.
        /// </summary>
        [JsonProperty("expected_ledger_size")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint ExpectedLedgerSize { get; set; }
        /// <summary>
        /// The Ledger Index of the current open ledger these stats describe.
        /// </summary>
        [JsonProperty("ledger_current_index")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint LedgerCurrentIndex { get; set; }
        /// <summary>
        /// required transaction cost level
        /// </summary>
        [JsonProperty("levels")]
        public Levels Levels { get; set; }
        /// <summary>
        /// The maximum number of transactions that the transaction queue can  currently hold.
        /// </summary>
        [JsonProperty("max_queue_size")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint MaxQueueSize { get; set; }
    }
    /// <summary>
    /// The transaction cost 
    /// </summary>
    public class Drops
    {
        /// <summary>
        /// The transaction cost required for a reference transaction to be included in a ledger under minimum load, represented in drops of XRP.
        /// </summary>
        [JsonProperty("base_fee")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint BaseFee { get; set; }
        /// <summary>
        /// An approximation of the median transaction cost among transactions.<br/>
        /// Included in the previous validated ledger, represented in drops of XRP.
        /// </summary>
        [JsonProperty("median_fee")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint MedianFee { get; set; }
        /// <summary>
        /// The minimum transaction cost for a reference transaction to be queued for a later ledger, represented in drops of XRP.<br/>
        /// If greater than base_fee, the transaction queue is full.
        /// </summary>
        [JsonProperty("minimum_fee")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint MinimumFee { get; set; }
        /// <summary>
        /// The minimum transaction cost that a reference transaction must pay to be included in the current open ledger, represented in drops of XRP.
        /// </summary>
        [JsonProperty("open_ledger_fee")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint OpenLedgerFee { get; set; }
    }
    /// <summary>
    /// required transaction cost level
    /// </summary>
    public class Levels
    {
        /// <summary>
        /// The median transaction cost among transactions in the previous validated ledger, represented in fee levels.
        /// </summary>
        [JsonProperty("median_level")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint MedianLevel { get; set; }
        /// <summary>
        /// The minimum transaction cost required to be queued for a future ledger, represented in fee levels.
        /// </summary>
        [JsonProperty("minimum_level")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint MinimumLevel { get; set; }
        /// <summary>
        /// The minimum transaction cost required to be included in the current open ledger, represented in fee levels.
        /// </summary>
        [JsonProperty("open_ledger_level")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint OpenLedgerLevel { get; set; }
        /// <summary>
        /// The equivalent of the minimum transaction cost, represented in fee levels.
        /// </summary>
        [JsonProperty("reference_level")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint ReferenceLevel { get; set; }
    }

}

using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;

namespace Xrpl.Models.Methods
{

    public class FeeRequest : RippleRequest
    {
        public FeeRequest()
        {
            Command = "fee";
        }
    }
        public class Fee
    {
        [JsonProperty("current_ledger_size")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint CurrentLedgerSize { get; set; }

        [JsonProperty("current_queue_size")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint CurrentQueueSize { get; set; }

        [JsonProperty("drops")]
        public Drops Drops { get; set; }

        [JsonProperty("expected_ledger_size")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint ExpectedLedgerSize { get; set; }

        [JsonProperty("ledger_current_index")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint LedgerCurrentIndex { get; set; }

        [JsonProperty("levels")]
        public Levels Levels { get; set; }

        [JsonProperty("max_queue_size")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint MaxQueueSize { get; set; }
    }

    public class Drops
    {
        [JsonProperty("base_fee")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint BaseFee { get; set; }

        [JsonProperty("median_fee")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint MedianFee { get; set; }

        [JsonProperty("minimum_fee")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint MinimumFee { get; set; }

        [JsonProperty("open_ledger_fee")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint OpenLedgerFee { get; set; }
    }

    public class Levels
    {
        [JsonProperty("median_level")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint MedianLevel { get; set; }

        [JsonProperty("minimum_level")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint MinimumLevel { get; set; }

        [JsonProperty("open_ledger_level")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint OpenLedgerLevel { get; set; }

        [JsonProperty("reference_level")]
        [JsonConverter(typeof(GenericStringConverter<uint>))]
        public uint ReferenceLevel { get; set; }
    }

}

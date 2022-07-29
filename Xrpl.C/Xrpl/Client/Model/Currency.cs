using System.Dynamic;
using System.Globalization;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;

namespace Xrpl.Client.Model
{
    public class Currency
    {
        public Currency()
        {
            CurrencyCode = "XRP";
        }

        [JsonProperty("currency")]
        public string CurrencyCode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonIgnore]
        public decimal ValueAsNumber
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Value))
                    return 0;
                var can_parse = decimal.TryParse(Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result);
                return !can_parse ? 0 : result;
            }
            set => Value = value.ToString(CurrencyCode == "XRP" ? "G0" : "G15", CultureInfo.InvariantCulture);
        }

        [JsonIgnore]
        public decimal? ValueAsXrp
        {
            get
            {
                if (CurrencyCode != "XRP" || string.IsNullOrEmpty(Value))
                    return null;
                if (string.IsNullOrWhiteSpace(Value))
                    return 0;
                var can_parse = decimal.TryParse(Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var val);

                return !can_parse ? 0 : val / 1000000;
            }
            set
            {
                if (value.HasValue)
                {
                    CurrencyCode = "XRP";
                    decimal val = value.Value * 1000000;
                    Value = val.ToString("G0", CultureInfo.InvariantCulture);
                }
                else
                {
                    Value = "0";
                }
            }
        }
    }
}

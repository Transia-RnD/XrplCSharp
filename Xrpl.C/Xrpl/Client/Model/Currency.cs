using Newtonsoft.Json;

using System.Globalization;
using Xrpl.Client.Extensions;

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
        [JsonIgnore]
        public string CurrencyHexName => CurrencyCode is { Length: > 0 } row ? row.Length > 3 ? row.FromHexString().Trim('\0') : row : string.Empty;


        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonIgnore]
        public decimal ValueAsNumber
        {
            get => string.IsNullOrWhiteSpace(Value) ? 0 : decimal.Parse(Value, NumberStyles.Float, CultureInfo.InvariantCulture);
            set => Value = value.ToString(CurrencyCode == "XRP" ? "G0" : "G15", CultureInfo.InvariantCulture);
        }

        [JsonIgnore]
        public decimal? ValueAsXrp
        {
            get
            {
                if (CurrencyCode != "XRP" || string.IsNullOrEmpty(Value))
                    return null;
                var val = string.IsNullOrWhiteSpace(Value) ? 0 : decimal.Parse(Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                return val / 1000000;
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
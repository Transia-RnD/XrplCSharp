using Newtonsoft.Json;

using Xrpl.Client.Extensions;

namespace Xrpl.Client.Models.Common
{
    public partial class Currency
    {
        [JsonIgnore]
        public string CurrencyValidName => CurrencyCode is { Length: > 0 } row ? row.Length > 3 ? row.FromHexString().Trim('\0') : row : string.Empty;

    }
}


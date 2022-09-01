using Newtonsoft.Json;
using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
{
    public class Submit
    {
        [JsonProperty("engine_result")]
        public string EngineResult { get; set; }

        [JsonProperty("engine_result_code")]
        public int EngineResultCode { get; set; }

        [JsonProperty("engine_result_message")]
        public string EngineResultMessage { get; set; }

        [JsonProperty("tx_blob")]
        public string TxBlob { get; set; }

        [JsonProperty("tx_json")]
        public dynamic TxJson { get; set; }

        //[JsonIgnore]
        public ITransactionResponseCommon Transaction => JsonConvert.DeserializeObject<TransactionResponseCommon>(TxJson.ToString());
    }
}

using Newtonsoft.Json;
//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/tx.ts
namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The tx method retrieves information on a single transaction, by its  identifying hash.<br/>
    /// Expects a response in the form of a TxResponse.
    /// </summary>
    public class TxRequest : RippleRequest
    {
        public TxRequest(string hash)
        {
            Command = "tx";
            Transaction = hash;
        }

        [JsonProperty("transaction")]
        internal string Transaction { get; set; }

        /// <summary>
        /// If true, return transaction data and metadata as binary serialized to hexadecimal strings.<br/>
        /// If false, return transaction data and metadata as JSON.<br/>
        /// The default is false.
        /// </summary>
        [JsonProperty("binary")]
        internal bool? Binary { get; set; }

        //todo not found fields - min_ledger?: number, max_ledger?: number
    }
    // todo not found class TxResponse extends BaseResponse
    //https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/tx.ts#L41
}

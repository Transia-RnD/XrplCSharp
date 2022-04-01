using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model;
using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Model.Transaction.TransactionTypes;
using Xrpl.Client.Responses.Transaction.Interfaces;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
    [JsonConverter(typeof(TransactionConverter))]
    public abstract class   TransactionResponseCommon : BaseTransactionResponse, ITransactionCommon, ITransactionResponseCommon
    {
        public string Account { get; set; }

        public string AccountTxnID { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Fee { get; set; }

        public uint? Flags { get; set; }

        /// <summary>
        /// Although optional, the LastLedgerSequence is strongly recommended on every transaction to ensure it's validated or rejected promptly.
        /// </summary>
        public uint? LastLedgerSequence { get; set; }

        public List<Memo> Memos { get; set; }

        public uint? Sequence { get; set; }

        [JsonProperty("SigningPubKey")]
        public string SigningPublicKey { get; set; }

        public List<Signer> Signers { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionType TransactionType { get; set; }

        [JsonProperty("TxnSignature")]
        public string TransactionSignature { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("date")]
        public uint? date { get; set; }

        [JsonProperty("inLedger")]
        public uint? inLedger { get; set; }

        [JsonProperty("ledger_index")]
        public uint? ledger_index { get; set; }

        public string ToJson()
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            return JsonConvert.SerializeObject(this, serializerSettings);
        }
    }
}

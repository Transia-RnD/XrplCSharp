using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model;
using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.Interfaces;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
    public class NFTokenMintTransactionResponse : TransactionResponseCommon, INFTokenMintTransaction
    {
        public new NFTokenMintFlags? Flags { get; set; }

        public uint? TokenTaxon { get; set; }

        public string Issuer { get; set; }

        public uint? TransferFee { get; set; }

        public string URI { get; set; }
    }
}

using System;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model;
using RippleDotNet.Model.Transaction.Interfaces;
using RippleDotNet.Responses.Transaction.Interfaces;

namespace RippleDotNet.Responses.Transaction.TransactionTypes
{
    public class NFTokenBurnTransactionResponse : TransactionResponseCommon, INFTokenBurnTransaction
    {
        public string TokenID { get; set; }
    }
}

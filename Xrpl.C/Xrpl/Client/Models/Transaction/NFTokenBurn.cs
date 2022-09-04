using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelClaim.ts

namespace Xrpl.Client.Models.Transactions
{
    public class NFTokenBurn : TransactionCommon, INFTokenBurn
    {
        public NFTokenBurn()
        {
            TransactionType = TransactionType.NFTokenBurn;
        }

        //public string Account { get; set; } // INHEIRTED FROM COMMON

        public string NFTokenID { get; set; }
    }

    public interface INFTokenBurn : ITransactionCommon
    {
        //string Issuer { get; set; } // INHEIRTED FROM COMMON
        string NFTokenID { get; set; }
    }

    public class NFTokenBurnResponse : TransactionResponseCommon, INFTokenBurn
    {
        public string NFTokenID { get; set; }
    }
}

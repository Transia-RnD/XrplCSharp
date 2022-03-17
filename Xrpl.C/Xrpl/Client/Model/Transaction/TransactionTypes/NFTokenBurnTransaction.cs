using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model.Transaction.Interfaces;

namespace Xrpl.Client.Model.Transaction.TransactionTypes
{
    public class NFTokenBurnTransaction : TransactionCommon, INFTokenBurnTransaction
    {
        public NFTokenBurnTransaction()
        {
            TransactionType = TransactionType.NFTokenBurn;
        }

        //public string Account { get; set; } // INHEIRTED FROM COMMON

        public string TokenID { get; set; }
    }
}

using System;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model.Transaction.Interfaces;

namespace RippleDotNet.Model.Transaction.TransactionTypes
{
    public class NFTokenMintTransaction : TransactionCommon, INFTokenMintTransaction
    {
        public NFTokenMintTransaction()
        {
            TransactionType = TransactionType.NFTokenMint;
        }

        public new NFTokenMintFlags? Flags { get; set; }

        public uint? TokenTaxon { get; set; }

        public string Issuer { get; set; }

        public uint? TransferFee { get; set; }

        public string URI { get; set; }
    }
}

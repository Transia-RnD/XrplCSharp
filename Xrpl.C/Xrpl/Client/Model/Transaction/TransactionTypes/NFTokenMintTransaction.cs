using System;

using Newtonsoft.Json;

using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model.Transaction.Interfaces;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Transaction.TransactionTypes
{
    public class NFTokenMintTransaction : TransactionCommon, INFTokenMintTransaction
    {
        public NFTokenMintTransaction()
        {
            TransactionType = TransactionType.NFTokenMint;
        }

        public new NFTokenMintFlags? Flags { get; set; }

        public uint NFTokenTaxon { get; set; }

        public string Issuer { get; set; }

        public uint? TransferFee { get; set; }

        public string URI { get; set; }
    }
}

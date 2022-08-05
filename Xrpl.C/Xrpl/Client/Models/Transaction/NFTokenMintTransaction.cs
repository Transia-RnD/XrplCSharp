using System;
using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
{
    [Flags]
    public enum NFTokenMintFlags : uint
    {
        tfBurnable = 1,
        tfOnlyXRP = 2,
        tfTrustLine = 3,
        tfTransferable = 4
    }
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

    public interface INFTokenMintTransaction : ITransactionCommon
    {
        new NFTokenMintFlags? Flags { get; set; }
        uint NFTokenTaxon { get; set; }
        string Issuer { get; set; }
        uint? TransferFee { get; set; }
        string URI { get; set; }
    }

    public class NFTokenMintTransactionResponse : TransactionResponseCommon, INFTokenMintTransaction
    {
        public new NFTokenMintFlags? Flags { get; set; }

        public uint NFTokenTaxon { get; set; }

        public string Issuer { get; set; }

        public uint? TransferFee { get; set; }

        public string URI { get; set; }
    }
}

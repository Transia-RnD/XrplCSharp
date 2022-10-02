using System;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelClaim.ts

namespace Xrpl.Models.Transactions
{
    [Flags]
    public enum NFTokenMintFlags : uint
    {
        tfBurnable = 1,
        tfOnlyXRP = 2,
        tfTrustLine = 3,
        tfTransferable = 4
    }
    public class NFTokenMint : TransactionCommon, INFTokenMint
    {
        public NFTokenMint()
        {
            TransactionType = TransactionType.NFTokenMint;
        }

        public new NFTokenMintFlags? Flags { get; set; }

        public uint NFTokenTaxon { get; set; }

        public string Issuer { get; set; }

        public uint? TransferFee { get; set; }

        public string URI { get; set; }
    }

    public interface INFTokenMint : ITransactionCommon
    {
        new NFTokenMintFlags? Flags { get; set; }
        uint NFTokenTaxon { get; set; }
        string Issuer { get; set; }
        uint? TransferFee { get; set; }
        string URI { get; set; }
    }

    public class NFTokenMintResponse : TransactionResponseCommon, INFTokenMint
    {
        public new NFTokenMintFlags? Flags { get; set; }

        public uint NFTokenTaxon { get; set; }

        public string Issuer { get; set; }

        public uint? TransferFee { get; set; }

        public string URI { get; set; }
    }
}

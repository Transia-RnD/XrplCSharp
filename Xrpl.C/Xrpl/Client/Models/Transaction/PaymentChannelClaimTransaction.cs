using System;
using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
{
    [Flags]
    public enum PaymentChannelClaimFlags : uint
    {
        tfRenew = 65536,
        tfClose = 131072
    }
    public class PaymentChannelClaimTransaction : TransactionCommon, IPaymentChannelClaimTransaction
    {
        public PaymentChannelClaimTransaction()
        {
            TransactionType = TransactionType.PaymentChannelClaim;
        }

        public string Channel { get; set; }

        public string Balance { get; set; }

        public string Amount { get; set; }

        public new PaymentChannelClaimFlags? Flags { get; set; }

        public string Signature { get; set; }

        public string PublicKey { get; set; }
    }

    public interface IPaymentChannelClaimTransaction : ITransactionCommon
    {
        string Amount { get; set; }
        string Balance { get; set; }
        string Channel { get; set; }
        new PaymentChannelClaimFlags? Flags { get; set; }
        string PublicKey { get; set; }
        string Signature { get; set; }
    }

    public class PaymentChannelClaimTransactionResponse : TransactionResponseCommon, IPaymentChannelClaimTransaction
    {
        public string Amount { get; set; }
        public string Balance { get; set; }
        public string Channel { get; set; }
        public new PaymentChannelClaimFlags? Flags { get; set; }
        public string PublicKey { get; set; }
        public string Signature { get; set; }
    }
}

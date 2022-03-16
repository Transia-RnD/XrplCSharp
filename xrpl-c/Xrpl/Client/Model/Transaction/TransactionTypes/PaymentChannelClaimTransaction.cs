using RippleDotNet.Model.Transaction.Interfaces;

namespace RippleDotNet.Model.Transaction.TransactionTypes
{
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
}

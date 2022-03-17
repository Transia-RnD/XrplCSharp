using Xrpl.Client.Model;
using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.Interfaces;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
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

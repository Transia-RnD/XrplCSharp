namespace RippleDotNet.Model.Transaction.Interfaces
{
    public interface IPaymentChannelClaimTransaction : ITransactionCommon
    {
        string Amount { get; set; }
        string Balance { get; set; }
        string Channel { get; set; }
        new PaymentChannelClaimFlags? Flags { get; set; }
        string PublicKey { get; set; }
        string Signature { get; set; }
    }
}
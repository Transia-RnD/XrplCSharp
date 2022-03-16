namespace RippleDotNet.Model.Transaction.Interfaces
{
    public interface IAccountSetTransaction : ITransactionCommon
    {
        uint? ClearFlag { get; set; }

        string Domain { get; set; }

        string EmailHash { get; set; }

        string MessageKey { get; set; }

        uint? SetFlag { get; set; }

        uint? TransferRate { get; set; }

        uint? TickSize { get; set; }
    }
}

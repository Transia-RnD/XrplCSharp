using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
{
    public class NFTokenBurnTransaction : TransactionCommon, INFTokenBurnTransaction
    {
        public NFTokenBurnTransaction()
        {
            TransactionType = TransactionType.NFTokenBurn;
        }

        //public string Account { get; set; } // INHEIRTED FROM COMMON

        public string NFTokenID { get; set; }
    }

    public interface INFTokenBurnTransaction : ITransactionCommon
    {
        //string Issuer { get; set; } // INHEIRTED FROM COMMON
        string NFTokenID { get; set; }
    }

    public class NFTokenBurnTransactionResponse : TransactionResponseCommon, INFTokenBurnTransaction
    {
        public string NFTokenID { get; set; }
    }
}

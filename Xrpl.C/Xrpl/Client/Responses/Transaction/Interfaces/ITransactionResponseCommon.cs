using System.Collections.Generic;
using Xrpl.Client.Model;
using Xrpl.Client.Model.Transaction.TransactionTypes;

namespace Xrpl.Client.Responses.Transaction.Interfaces
{
    public interface ITransactionResponseCommon : IBaseTransactionResponse
    {
        string Account { get; set; }
        string AccountTxnID { get; set; }
        Currency Fee { get; set; }
        uint? Flags { get; set; }
        uint? LastLedgerSequence { get; set; }
        List<Memo> Memos { get; set; }
        //Meta Meta { get; set; }
        uint? Sequence { get; set; }
        List<Signer> Signers { get; set; }
        string SigningPublicKey { get; set; }
        string TransactionSignature { get; set; }
        TransactionType TransactionType { get; set; }

        string ToJson();
    }
}
using System.Collections.Generic;

using Xrpl.Client.Model.Transaction.TransactionTypes;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface ITransactionCommon
    {
        string Account { get; set; }

        string AccountTxnID { get; set; }

        Currency Fee { get; set; }

        uint? Flags { get; set; }

        uint? LastLedgerSequence { get; set; }

        List<Memo> Memos { get; set; }

        Meta Meta { get; set; }

        uint? Sequence { get; set; }

        List<Signer> Signers { get; set; }

        string SigningPublicKey { get; set; }

        string TransactionSignature { get; set; }

        TransactionType TransactionType { get; set; }

        string ToJson();
    }
}
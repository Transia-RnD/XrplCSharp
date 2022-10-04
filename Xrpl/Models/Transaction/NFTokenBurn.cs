﻿

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/NFTokenBurn.ts

namespace Xrpl.Models.Transaction
{
    /// <summary>
    /// The NFTokenBurn transaction is used to remove an NFToken object from the  NFTokenPage in which it is being held, effectively removing the token from  the ledger ("burning" it).<br/>
    /// If this operation succeeds, the corresponding NFToken is removed.<br/>
    /// If this  operation empties the NFTokenPage holding the NFToken or results in the  consolidation, thus removing an NFTokenPage, the owner’s reserve requirement  is reduced by one.
    /// </summary>
    public class NFTokenBurn : TransactionCommon, INFTokenBurn
    {
        public NFTokenBurn()
        {
            TransactionType = TransactionType.NFTokenBurn;
        }

        //public string Account { get; set; } // INHEIRTED FROM COMMON

        /// <inheritdoc />
        public string NFTokenID { get; set; }

        /// <inheritdoc />
        public string Owner { get; set; }
    }

    /// <summary>
    /// The NFTokenBurn transaction is used to remove an NFToken object from the  NFTokenPage in which it is being held, effectively removing the token from  the ledger ("burning" it).<br/>
    /// If this operation succeeds, the corresponding NFToken is removed.<br/>
    /// If this  operation empties the NFTokenPage holding the NFToken or results in the  consolidation, thus removing an NFTokenPage, the owner’s reserve requirement  is reduced by one.
    /// </summary>
    public interface INFTokenBurn : ITransactionCommon
    {
        //string Issuer { get; set; } // INHEIRTED FROM COMMON
        /// <summary>
        /// Identifies the NFToken object to be removed by the transaction.
        /// </summary>
        string NFTokenID { get; set; }
        /// <summary>
        /// Indicates which account currently owns the token if it is different than Account.<br/>
        /// Only used to burn tokens which have the lsfBurnable flag enabled and are not owned by the signing account.
        /// </summary>
        string Owner { get; set; }
    }

    public class NFTokenBurnResponse : TransactionResponseCommon, INFTokenBurn
    {
        /// <inheritdoc />
        public string NFTokenID { get; set; }

        /// <inheritdoc />
        public string Owner { get; set; }
    }
}

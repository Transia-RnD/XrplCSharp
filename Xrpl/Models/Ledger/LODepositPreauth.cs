// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/DepositPreauth.ts

namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// A DepositPreauth object tracks a preauthorization from one account to another.<br/>
    /// DepositPreauth transactions create these objects.
    /// </summary>
    public class LODepositPreauth : BaseLedgerEntry
    {
        public LODepositPreauth()
        {
            LedgerEntryType = LedgerEntryType.DepositPreauth;
        }

        /// <summary>
        /// The sender of the Check. Cashing the Check debits this address's balance.
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// The intended recipient of the Check.<br/>
        /// Only this address can cash the Check, using a CheckCash transaction.
        /// </summary>
        public string Authorize { get; set; }
        /// <summary>
        /// A bit-map of boolean flags.<br/>
        /// No flags are defined for Checks, so this value is always 0.
        /// </summary>
        public string Flags { get; set; }
        /// <summary>
        /// A hint indicating which page of the sender's owner directory links to this object,
        /// in case the directory consists of multiple pages.
        /// </summary>
        public string OwnerNode { get; set; }
        /// <summary>
        /// The identifying hash of the transaction that most recently modified this object.
        /// </summary>
        public string PreviousTxnID { get; set; }
        /// <summary>
        /// The index of the ledger that contains the transaction that most recently modified this object.
        /// </summary>
        public uint PreviousTxnLgrSeq { get; set; }
    }
}

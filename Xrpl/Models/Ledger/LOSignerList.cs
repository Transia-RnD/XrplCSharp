using System.Collections.Generic;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/SignerList.ts

namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// The SignerList object type represents a list of parties that, as a group,
    /// are authorized to sign a transaction in place of an individual account. <br/>
    /// You can create, replace, or remove a signer list using a SignerListSet transaction.
    /// </summary>
    public class LOSignerList : BaseLedgerEntry
    {
        /// <summary> create base object </summary>
        public LOSignerList()
        {
            LedgerEntryType = LedgerEntryType.SignerList;
        }
        /// <summary>
        /// A bit-map of Boolean flags enabled for this signer list.<br/>
        /// For more information, see SignerList Flags.
        /// </summary>
        public uint Flags { get; set; }
        /// <summary>
        /// A hint indicating which page of the owner directory links to this object, in case the directory consists of multiple pages.
        /// </summary>
        public string OwnerNode { get; set; }
        /// <summary>
        /// A target number for signer weights.<br/>
        /// To produce a valid signature for the owner of this SignerList,
        /// the signers must provide valid signatures whose weights sum to this value or more.
        /// </summary>
        public uint SignerQuorum { get; set; }
        /// <summary>
        /// An array of Signer Entry objects representing the parties who are part of this signer list.
        /// </summary>
        public List<SignerEntryWrapper> SignerEntries { get; set; }
        /// <summary>
        /// An ID for this signer list.<br/>
        /// Currently always set to 0.<br/>
        /// If a future amendment allows multiple signer lists for an account, this may change.
        /// </summary>
        public uint SignerListId { get; set; }

        /// <summary>
        /// The identifying hash of the transaction that most recently modified this object.
        /// </summary>
        public string PreviousTxnID { get; set; }
        /// <summary>
        /// The index of the ledger that contains the transaction that most recently modified this object.
        /// </summary>
        public uint PreviousTxnLgrSeq { get; set; }
    }

    public class SignerEntryWrapper
    {
        public SignerEntry SignerEntry { get; set; }
    }

    //https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/common/index.ts#L67
    /// <summary>
    /// The object that describes the signer in SignerEntries.
    /// </summary>
    public class SignerEntry
    {
        /// <summary>
        /// An XRP Ledger address whose signature contributes to the multi-signature.<br/>
        /// It does not need to be a funded address in the ledger.
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// The weight of a signature from this signer.<br/>
        /// A multi-signature is only valid if the sum weight of the signatures provided meets
        /// or exceeds the signer list's SignerQuorum value.
        /// </summary>
        public ushort SignerWeight { get; set; }

        /// <summary>
        /// An arbitrary 256-bit (32-byte) field that can be used to identify the signer, which  may be useful for smart contracts,
        /// or for identifying who controls a key in a large  organization.
        /// </summary>
        public string WalletLocator { get; set; }
    }
}

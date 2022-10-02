using System.Collections.Generic;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/DirectoryNode.ts

namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// The DirectoryNode object type provides a list of links to other objects in the ledger's state tree.
    /// </summary>
    public class LODirectoryNode : BaseLedgerEntry
    {

        public LODirectoryNode()
        {
            LedgerEntryType = LedgerEntryType.DirectoryNode;
        }

        /// <summary>
        /// A bit-map of boolean flags enabled for this directory.Currently,
        /// the protocol defines no flags for DirectoryNode objects.
        /// </summary>
        public uint Flags { get; set; }
        /// <summary>
        /// The ID of root object for this directory. 
        /// </summary>
        public string RootIndex { get; set; }
        /// <summary>
        /// The contents of this Directory: an array of IDs of other objects.
        /// </summary>
        public List<string> Indexes { get; set; }
        /// <summary>
        /// If this Directory consists of multiple pages,
        /// this ID links to the next object in the chain, wrapping around at the end.
        /// </summary>
        public string IndexNext { get; set; }
        /// <summary>
        /// If this Directory consists of multiple pages,
        /// this ID links to the previous object in the chain, wrapping around at the beginning.
        /// </summary>
        public string IndexPrevious { get; set; }
        /// <summary>
        /// The address of the account that owns the objects in this directory.
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// The currency code of the TakerPays amount from the offers in this directory.
        /// </summary>
        public string TakerPaysCurrency { get; set; }
        /// <summary>
        /// The issuer of the TakerPays amount from the offers in this directory. 
        /// </summary>
        public string TakerPaysIssuer { get; set; }
        /// <summary>
        /// The currency code of the TakerGets amount from the offers in this directory.
        /// </summary>
        public string TakerGetsCurrency { get; set; }
        /// <summary>
        /// The issuer of the TakerGets amount from the offers in this directory.
        /// </summary>
        public string TakerGetsIssuer { get; set; }
    }
}

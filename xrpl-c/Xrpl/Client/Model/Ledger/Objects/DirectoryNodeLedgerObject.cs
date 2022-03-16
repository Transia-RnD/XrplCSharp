using System.Collections.Generic;

namespace RippleDotNet.Model.Ledger.Objects
{
    public class DirectoryNodeLedgerObject : BaseRippleLedgerObject
    {

        public DirectoryNodeLedgerObject()
        {
            LedgerEntryType = LedgerEntryType.DirectoryNode;
        }

        public uint Flags { get; set; }

        public string RootIndex { get; set; }

        public List<string> Indexes { get; set; }

        public string IndexNext { get; set; }

        public string IndexPrevious { get; set; }

        public string Owner { get; set; }

        public string TakerPaysCurrency { get; set; }

        public string TakerPaysIssuer { get; set; }

        public string TakerGetsCurrency { get; set; }

        public string TakerGetsIssuer { get; set; }
    }
}

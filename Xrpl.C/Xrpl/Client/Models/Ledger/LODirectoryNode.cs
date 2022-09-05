using System.Collections.Generic;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/DirectoryNode.ts

namespace Xrpl.Client.Models.Ledger
{
    public class LODirectoryNode : BaseLedgerEntry
    {

        public LODirectoryNode()
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

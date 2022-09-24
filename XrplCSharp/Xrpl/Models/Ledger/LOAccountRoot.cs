using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models.Ledger;
using Xrpl.Models;
using Xrpl.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/AccountRoot.ts

namespace Xrpl.Models.Ledger
{
    [Flags]
    public enum AccountRootFlags : uint
    {
        lsfPasswordSpent = 65536,
        lsfRequireDestTag = 131072,
        lsfRequireAuth = 262144,
        lsfDisallowXRP = 524288,
        lsfDisableMaster = 1048576,
        lsfNoFreeze = 2097152,
        lsfGlobalFreeze = 4194304,
        lsfDefaultRipple = 8388608
    }
    public class LOAccountRoot : BaseLedgerEntry
    {
        public LOAccountRoot()
        {
            LedgerEntryType = LedgerEntryType.AccountRoot;
        }

        public string Account { get; set; }

        public AccountRootFlags Flags { get; set; }

        public uint Sequence { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Balance { get; set; }

        public uint OwnerCount { get; set; }

        public string PreviousTxnID { get; set; }

        public uint PreviousTxnLgrSeq { get; set; }

        public string AccountTxnID { get; set; }

        public string RegularKey { get; set; }

        public string EmailHash { get; set; }

        public string MessageKey { get; set; }

        public byte? TickSize { get; set; }

        public uint? TransferRate { get; set; }

        public string Domain { get; set; }
    }
}

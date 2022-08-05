using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/AccountRoot.ts

namespace Xrpl.Client.Models.Ledger
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
    public class LOAccountRoot : BaseRippleLO
    {
        public LOAccountRoot() => LedgerEntryType = LedgerEntryType.AccountRoot;

        public string Account { get; set; }

        public AccountRootFlags Flags { get; set; }

        public uint Sequence { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Balance { get; set; }

        public uint OwnerCount { get; set; }

        [JsonProperty("PreviousTxnID")]
        public string PreviousTransactionId { get; set; }

        [JsonProperty("PreviousTxnLgrSeq")]
        public uint PreviousTransactionLedgerSequence { get; set; }

        [JsonProperty("AccountTxnID")]
        public string AccountTransactionId { get; set; }

        public string RegularKey { get; set; }

        public string EmailHash { get; set; }

        public string MessageKey { get; set; }

        public byte? TickSize { get; set; }

        public uint? TransferRate { get; set; }

        public string Domain { get; set; }
    }
}

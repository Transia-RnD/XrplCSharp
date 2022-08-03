using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/Amendments.ts

namespace Xrpl.Client.Models.Ledger
{
    public enum EnableAmendmentFlags
    {
        tfGotMajority = 65536,
        tfLostMajority = 131072
    }

    public class LOAmendments : BaseRippleLO
    {
        public LOAmendments()
        {
            LedgerEntryType = LedgerEntryType.Amendments;
        }

        public List<Majority> Majorities { get; set; }

        public List<string> Amendments { get; set; }

        public uint Flags { get; set; }
    }

    public class Majority
    {
        public string Amendment { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CloseTime { get; set; }
    }
}

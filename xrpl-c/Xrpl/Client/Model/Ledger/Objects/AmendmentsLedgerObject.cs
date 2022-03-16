using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;

namespace RippleDotNet.Model.Ledger.Objects
{
    public class AmendmentsLedgerObject : BaseRippleLedgerObject
    {
        public AmendmentsLedgerObject()
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

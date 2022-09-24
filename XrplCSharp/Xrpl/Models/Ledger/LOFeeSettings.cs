
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/FeeSettings.ts

namespace Xrpl.Models.Ledger
{
    public class LOFeeSettings : BaseLedgerEntry
    {
        public LOFeeSettings()
        {
            LedgerEntryType = LedgerEntryType.FeeSettings;
        }

        public uint Flags { get; set; }

        //Transaction fee in drops of XRP as hexidecimal
        public string BaseFee { get; set; }

        public uint ReferenceFeeUnits { get; set; }

        public uint ReserveBase { get; set; }

        public uint ReserveIncrement { get; set; }
    }
}

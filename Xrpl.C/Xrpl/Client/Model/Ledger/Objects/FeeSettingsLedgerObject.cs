using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Ledger.Objects
{
    public class FeeSettingsLedgerObject : BaseRippleLedgerObject
    {
        public FeeSettingsLedgerObject()
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

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/FeeSettings.ts

namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// The FeeSettings object type contains the current base transaction cost and reserve amounts as determined by fee voting.
    /// </summary>
    public class LOFeeSettings : BaseLedgerEntry
    {
        public LOFeeSettings()
        {
            LedgerEntryType = LedgerEntryType.FeeSettings;
        }
        /// <summary>
        /// A bit-map of boolean flags for this object.<br/>
        /// No flags are defined for this type
        /// </summary>
        public uint Flags { get; set; }

        /// <summary>
        /// The transaction cost of the "reference transaction" in drops of XRP as hexadecimal.
        /// </summary>
        public string BaseFee { get; set; }
        /// <summary>
        /// The BaseFee translated into "fee units".
        /// </summary>
        public uint ReferenceFeeUnits { get; set; }
        /// <summary>
        /// The base reserve for an account in the XRP Ledger, as drops of XRP.
        /// </summary>
        public uint ReserveBase { get; set; }
        /// <summary>
        /// The incremental owner reserve for owning objects, as drops of XRP.
        /// </summary>
        public uint ReserveIncrement { get; set; }
    }
}

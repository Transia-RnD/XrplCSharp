using System;

using Xrpl.Models.Common;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/RippleState.ts

namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// There are several options which can be either enabled or disabled for a trust line.<br/>
    /// These options can be changed with a TrustSet transaction.<br/>
    /// In the ledger, flags are represented as binary values that can be combined with bitwise-or operations.<br/>
    /// The bit values for the flags in the ledger are different than the values used to enable or disable those flags in a transaction.<br/>
    /// Ledger flags have names that begin with lsf.
    /// </summary>
    [Flags]
    public enum RippleStateFlags
    {
        
        /// <summary> This RippleState object contributes to the low account's owner reserve. </summary>
        lsfLowReserve = 65536,// True, if entry counts toward reserve.
        /// <summary> This RippleState object contributes to the high account's owner reserve. </summary>
        lsfHighReserve = 131072,
        /// <summary> The low account has authorized the high account to hold tokens issued by the low account. </summary>
        lsfLowAuth = 262144,
        /// <summary> The high account has authorized the low account to hold tokens issued by the high account. </summary>
        lsfHighAuth = 524288,
        /// <summary> The low account has disabled rippling from this trust line. </summary>
        lsfLowNoRipple = 1048576,
        /// <summary> The high account has disabled rippling from this trust line. </summary>
        lsfHighNoRipple = 2097152,
        /// <summary> The low account has frozen the trust line, preventing the high account from transferring the asset. </summary>
        lsfLowFreeze = 4194304,// True, low side has set freeze flag
        /// <summary> The high account has frozen the trust line, preventing the low account from transferring the asset. </summary>
        lsfHighFreeze = 8388608// True, high side has set freeze flag
    }
    /// <summary>
    /// The RippleState object type connects two accounts in a single currency.
    /// </summary>
    public class LORippleState : BaseLedgerEntry
    {
        public LORippleState()
        {
            LedgerEntryType = LedgerEntryType.RippleState;
        }
        /// <summary>
        /// A bit-map of boolean options enabled for this object. 
        /// </summary>
        public RippleStateFlags Flags { get; set; }
        /// <summary>
        /// The balance of the trust line, from the perspective of the low account.<br/>
        /// A negative balance indicates that the low account has issued currency to the high account.<br/>
        /// The issuer is always the neutral value ACCOUNT_ONE.
        /// </summary>
        public Currency Balance { get; set; }
        /// <summary>
        /// The limit that the low account has set on the trust line.<br/>
        /// The issuer is the address of the low account that set this limit.
        /// </summary>
        public Currency LowLimit { get; set; }
        /// <summary>
        /// The limit that the high account has set on the trust line.<br/>
        /// The issuer is the address of the high account that set this limit.
        /// </summary>
        public Currency HighLimit { get; set; }
        /// <summary>
        /// The identifying hash of the transaction that most recently modified this object.
        /// </summary>
        public string PreviousTxnID { get; set; }
        /// <summary>
        /// The index of the ledger that contains the transaction that most recently modified this object.
        /// </summary>
        public uint PreviousTxnLgrSeq { get; set; }
        /// <summary>
        ///  A hint indicating which page of the low account's owner directory links to this object,
        /// in case the directory consists of multiple pages.
        /// </summary>
        public string LowNode { get; set; }
        /// <summary>
        /// A hint indicating which page of the high account's owner directory links to this object,
        /// in case the directory consists of multiple pages.
        /// </summary>
        public string HighNode { get; set; }
        /// <summary>
        /// The inbound quality set by the low account, as an integer in the implied ratio LowQualityIn:1,000,000,000.<br/>
        /// As a special case, the value 0 is equivalent to 1 billion, or face value.
        /// </summary>
        public uint? LowQualityIn { get; set; }
        /// <summary>
        /// The outbound quality set by the low account, as an integer in the implied ratio LowQualityOut:1,000,000,000.<br/>
        /// As a special case, the value 0 is equivalent to 1 billion, or face value.
        /// </summary>
        public uint? LowQualityOut { get; set; }
        /// <summary>
        /// The inbound quality set by the high account, as an integer in the implied ratio HighQualityIn:1,000,000,000.<br/>
        /// As a special case, the value 0 is equivalent to 1 billion, or face value.
        /// </summary>
        public uint? HighQualityIn { get; set; }
        /// <summary>
        /// The outbound quality set by the high account, as an integer in the implied ratio HighQualityOut:1,000,000,000.<br/>
        /// As a special case, the value 0 is equivalent to 1 billion, or face value.
        /// </summary>
        public uint? HighQualityOut { get; set; }
    }
}

using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/trustSet.ts 

namespace Xrpl.Client.Models.Transactions
{
    [Flags]
    public enum TrustSetFlags : uint
    {
        tfSetfAuth = 65536,
        tfSetNoRipple = 131072,
        tfClearNoRipple = 262144,
        tfSetFreeze = 1048576,
        tfClearFreeze = 2097152
    }
    
    public class TrustSet : TransactionCommon, ITrustSet
    {
        public TrustSet()
        {
            TransactionType = TransactionType.TrustSet;
            Flags = TrustSetFlags.tfSetNoRipple;
        }

        public new TrustSetFlags? Flags { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency LimitAmount {get; set; }

        public uint? QualityIn { get; set; }

        public uint? QualityOut { get; set; }
    }

    public interface ITrustSet : ITransactionCommon
    {
        new TrustSetFlags? Flags { get; set; }
        Currency LimitAmount { get; set; }
        uint? QualityIn { get; set; }
        uint? QualityOut { get; set; }
    }

    public class TrustSetResponse : TransactionResponseCommon, ITrustSet
    {
        public new TrustSetFlags? Flags { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency LimitAmount { get; set; }
        public uint? QualityIn { get; set; }
        public uint? QualityOut { get; set; }
    }
}

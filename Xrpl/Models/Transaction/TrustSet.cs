using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xrpl.Client.Exceptions;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/trustSet.ts 

namespace Xrpl.Models.Transaction
{
    /// <summary>
    /// Enum representing values of <see cref="ITrustSet"/> transaction flags.
    /// </summary>
    [Flags]
    public enum TrustSetFlags : uint
    {
        /// <summary>
        /// Authorize the other party to hold currency issued by this account.<br/>
        /// (No effect unless using the asfRequireAuth AccountSet flag.)<br/>
        /// Cannot be unset.
        /// </summary>
        tfSetfAuth = 65536,
        /// <summary>
        /// Enable the No Ripple flag, which blocks rippling between two trust lines.<br/>
        /// of the same currency if this flag is enabled on both.
        /// </summary>
        tfSetNoRipple = 131072,
        /// <summary>
        /// Disable the No Ripple flag, allowing rippling on this trust line.
        /// </summary>
        tfClearNoRipple = 262144,
        /// <summary>
        /// Freeze the trust line.
        /// </summary>
        tfSetFreeze = 1048576,
        /// <summary>
        /// Unfreeze the trust line.
        /// </summary>
        tfClearFreeze = 2097152
    }

    /// <inheritdoc cref="ITrustSet" />
    public class TrustSet : TransactionCommon, ITrustSet
    {
        public TrustSet()
        {
            TransactionType = TransactionType.TrustSet;
            Flags = TrustSetFlags.tfSetNoRipple;
        }

        /// <inheritdoc />
        public new TrustSetFlags? Flags { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency LimitAmount { get; set; }

        /// <inheritdoc />
        public uint? QualityIn { get; set; }

        /// <inheritdoc />
        public uint? QualityOut { get; set; }
    }

    /// <summary>
    /// Create or modify a trust line linking two accounts.
    /// </summary>
    /// <code>
    /// ```typescript  const trustSetTx: TrustSet =
    /// {
    /// 	TransactionType: 'TrustSet',
    /// 	Account: wallet2.getClassicAddress(),
    /// 	LimitAmount:{
    /// 	currency: 'FOO',
    /// 	issuer: wallet1.getClassicAddress(),
    /// 	value: '10000000000',
    ///     },
    /// 	Flags:{
    /// 	tfSetNoRipple: true
    ///     }
    /// }
    /// </code>
    public interface ITrustSet : ITransactionCommon
    {
        /// <summary>
        /// <see cref="ITrustSet"/> transaction flags
        /// </summary>
        new TrustSetFlags? Flags { get; set; }
        /// <summary>
        /// Object defining the trust line to create or modify, in the format of a Currency Amount.
        /// </summary>
        Currency LimitAmount { get; set; }
        /// <summary>
        /// Value incoming balances on this trust line at the ratio of this number per 1,000,000,000 units.<br/>
        /// A value of 0 is shorthand for treating balances at face value.
        /// </summary>
        uint? QualityIn { get; set; }
        /// <summary>
        /// Value outgoing balances on this trust line at the ratio of this number per 1,000,000,000 units.<br/>
        /// A value of 0 is shorthand for treating balances at face value.
        /// </summary>
        uint? QualityOut { get; set; }
    }

    /// <inheritdoc cref="ITrustSet" />
    public class TrustSetResponse : TransactionResponseCommon, ITrustSet
    {
        /// <inheritdoc />
        public new TrustSetFlags? Flags { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency LimitAmount { get; set; }
        /// <inheritdoc />
        public uint? QualityIn { get; set; }

        /// <inheritdoc />
        public uint? QualityOut { get; set; }
    }

    public partial class Validation
    {
        //https://github.com/XRPLF/xrpl.js/blob/b40a519a0d949679a85bf442be29026b76c63a22/packages/xrpl/src/models/transactions/trustSet.ts#L127
        /// <summary>
        /// Verify the form and type of a TrustSet at runtime.
        /// </summary>
        /// <param name="tx"> A TrustSet Transaction.</param>
        /// <exception cref="ValidationError">When the TrustSet is malformed.</exception>
        public static async Task ValidateTrustSet(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);
            if (!tx.TryGetValue("LimitAmount", out var LimitAmount) || LimitAmount is null)
                throw new ValidationError("TrustSet: missing field LimitAmount");
            // TODO: Review this function
            if (!Common.IsAmount(LimitAmount))
                throw new ValidationError("TrustSet: invalid LimitAmount");

            if (tx.TryGetValue("QualityIn", out var QualityIn) && QualityIn is not uint { })
                throw new ValidationError("TrustSet: invalid QualityIn");

            if (tx.TryGetValue("QualityOut", out var QualityOut) && QualityOut is not uint { })
                throw new ValidationError("TrustSet: invalid QualityOut");
        }
    }
}

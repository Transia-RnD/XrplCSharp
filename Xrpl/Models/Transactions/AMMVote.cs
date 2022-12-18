using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.BinaryCodec.Types;
using Xrpl.Client.Exceptions;

using static Xrpl.Models.Common.Common;

namespace Xrpl.Models.Transactions
{
    public class AMMVote : TransactionCommon, IAMMVote
    {
        public AMMVote()
        {
            TransactionType = TransactionType.AMMVote;
        }
        /// <inheritdoc />
        public IssuedCurrency Asset { get; set; }
        /// <inheritdoc />
        public IssuedCurrency Asset2 { get; set; }
        /// <inheritdoc />
        public uint TradingFee { get; set; }
    }

    public interface IAMMVote : ITransactionCommon
    {
        /// <summary>
        /// Specifies one of the pool assets (XRP or token) of the AMM instance.
        /// </summary>
        public IssuedCurrency Asset { get; set; }
        /// <summary>
        /// Specifies the other pool asset of the AMM instance.
        /// </summary>
        public IssuedCurrency Asset2 { get; set; }
        /// <summary>
        /// Specifies the fee, in basis point.
        /// Valid values for this field are between 0 and 1000 inclusive.
        /// A value of 1 is equivalent to 1/10 bps or 0.001%, allowing trading fee
        /// between 0% and 1%. This field is required.
        /// </summary>
        public uint TradingFee { get; set; }
    }
    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of an AMMVote at runtime.
        /// </summary>
        /// <param name="tx">An AMMVote Transaction.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"> When the AMMVote is Malformed.</exception>
        public static async Task ValidateAMMVote(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);
            tx.TryGetValue("Asset", out var Asset);
            tx.TryGetValue("Asset2", out var Asset2);
            tx.TryGetValue("TradingFee", out var TradingFee);

            if (Asset is null)
                throw new ValidationException("AMMVote: missing field Asset");

            if (!Xrpl.Models.Transactions.Common.IsIssue(Asset))
                throw new ValidationException("AMMVote: Asset must be an Issue");

            if (Asset2 is null)
                throw new ValidationException("AMMVote: missing field Asset2");

            if (!Xrpl.Models.Transactions.Common.IsIssue(Asset2))
                throw new ValidationException("AMMVote: Asset2 must be an Issue");
            if(TradingFee is null)
                throw new ValidationException("AMMVote: missing field TradingFee");
            if (TradingFee is not uint fee)
            {
                throw new ValidationException("AMMVote: TradingFee must be a number");
            }

            if (fee is < 0 or > AMM_MAX_TRADING_FEE)
            {
                throw new ValidationException($"AMMVote: TradingFee must be between 0 and {AMM_MAX_TRADING_FEE}");
            }
        }
    }

}

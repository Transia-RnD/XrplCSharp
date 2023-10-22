#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xrpl.BinaryCodec.Types;
using Xrpl.Client.Exceptions;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;
using Currency = Xrpl.Models.Common.Currency;

namespace Xrpl.Models.Transactions
{

    /// <summary>
    /// AMMCreate is used to create AccountRoot and the corresponding AMM ledger entries.
    /// This allows for the creation of only one AMM instance per unique asset pair.
    /// </summary>
    public class AMMCreate : TransactionCommon, IAMMCreate
    {
        public AMMCreate()
        {
            TransactionType = TransactionType.AMMCreate;
        }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Xrpl.Models.Common.Currency Amount { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Xrpl.Models.Common.Currency Amount2 { get; set; }
        /// <inheritdoc />
        public uint TradingFee { get; set; }
    }
    /// <summary>
    /// AMMCreate is used to create AccountRoot and the corresponding AMM ledger entries.
    /// This allows for the creation of only one AMM instance per unique asset pair.
    /// </summary>
    public interface IAMMCreate : ITransactionCommon
    {
        /// <summary>
        /// Specifies one of the pool assets (XRP or token) of the AMM instance.
        /// </summary>
        public Xrpl.Models.Common.Currency Amount { get; set; }
        /// <summary>
        /// Specifies the other pool asset of the AMM instance.
        /// </summary>
        public Xrpl.Models.Common.Currency Amount2 { get; set; }
        /// <summary>
        /// Specifies the fee, in basis point, to be charged
        /// to the traders for the trades executed against the AMM instance.
        /// Trading fee is a percentage of the trading volume.
        /// Valid values for this field are between 0 and 1000 inclusive.
        /// A value of 1 is equivalent to 1/10 bps or 0.001%, allowing trading fee
        /// between 0% and 1%.
        /// </summary>
        public uint TradingFee { get; set; }
    }

    /// <inheritdoc cref="IAMMCreate" />
    public class AMMCreateResponse : TransactionResponseCommon, IAMMCreate
    {
        #region Implementation of IAMMCreate

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount2 { get; set; }
        /// <inheritdoc />
        public uint TradingFee { get; set; }

        #endregion
    }

    public partial class Validation
    {
        public const uint AMM_MAX_TRADING_FEE = 1000;
        /// <summary>
        /// Verify the form and type of an AMMCreate at runtime.
        /// </summary>
        /// <param name="tx">An AMMCreate Transaction.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"> When the AMMCreate is Malformed.</exception>
        public static async Task ValidateAMMCreate(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            if (!tx.TryGetValue("Amount", out var Amount1) || Amount1 is null)
            {
                throw new ValidationException("AMMCreate: missing field Amount");
            }

            if (!Xrpl.Models.Transactions.Common.IsAmount(Amount1))
            {
                throw new ValidationException("AMMCreate: Amount must be an Amount");
            }

            if (!tx.TryGetValue("Amount2", out var Amount2) || Amount2 is null)
            {
                throw new ValidationException("AMMCreate: missing field Amount2");
            }

            if (!Xrpl.Models.Transactions.Common.IsAmount(Amount2))
            {
                throw new ValidationException("AMMCreate: Amount2 must be an Amount");
            }

            if (!tx.TryGetValue("TradingFee", out var TradingFee) || TradingFee is null)
            {
                throw new ValidationException("AMMCreate: missing field TradingFee");
            }

            if (TradingFee is not uint fee)
            {
                throw new ValidationException("AMMCreate: TradingFee must be a number");
            }

            if (fee is < 0 or > AMM_MAX_TRADING_FEE)
            {
                throw new ValidationException($"AMMCreate: TradingFee must be between 0 and {AMM_MAX_TRADING_FEE}");
            }
        }
    }
}


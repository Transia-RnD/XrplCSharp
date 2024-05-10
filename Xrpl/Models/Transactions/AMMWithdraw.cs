using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Xrpl.Models.Common.Common;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Common;
using Xrpl.Client.Json.Converters;

namespace Xrpl.Models.Transactions
{
    /// <summary>
    /// Enum representing values for AMMWithdrawFlags Transaction Flags  
    /// </summary>
    /// <category>Transaction Flags</category>
    public enum AMMWithdrawFlags : uint
    {
        /// <summary>
        /// Perform a double-asset withdrawal and receive the specified amount of LP Tokens.
        /// </summary>
        tfLPToken = 65536,//0x00010000
        /// <summary>
        /// 	Perform a double-asset withdrawal returning all your LP Tokens.
        /// </summary>
        tfWithdrawAll =131072,//0x00020000
        /// <summary>
        /// Perform a single-asset withdrawal returning all of your LP Tokens.
        /// </summary>
        tfOneAssetWithdrawAll = 262144,//0x00040000
        /// <summary>
        /// Perform a single-asset withdrawal with a specified amount of the asset to withdrawal.
        /// </summary>
        tfSingleAsset = 524288,//0x00080000
        /// <summary>
        /// Perform a double-asset withdrawal with specified amounts of both assets.
        /// </summary>
        tfTwoAsset = 1048576,//0x00100000
        /// <summary>
        /// Perform a single-asset withdrawal and receive the specified amount of LP Tokens.
        /// </summary>
        tfOneAssetLPToken = 2097152,//0x00200000
        /// <summary>
        /// Perform a single-asset withdrawal with a specified effective price.
        /// </summary>
        tfLimitLPToken = 4194304 //0x00400000
    }

    //public interface AMMWithdrawFlagsInterface : GlobalFlags
    //{
    //    bool? tfLPToken { get; set; }
    //    bool? tfWithdrawAll { get; set; }
    //    bool? tfOneAssetWithdrawAll { get; set; }
    //    bool? tfSingleAsset { get; set; }
    //    bool? tfTwoAsset { get; set; }
    //    bool? tfOneAssetLPToken { get; set; }
    //    bool? tfLimitLPToken { get; set; }
    //}

    /// <summary>
    /// AMMWithdraw is the withdraw transaction used to remove liquidity from the AMM
    /// instance pool, thus redeeming some share of the pools that one owns in the form
    /// of LPTokenIn.
    /// </summary>
    public class AMMWithdraw : TransactionCommon, IAMMWithdraw
    {
        public AMMWithdraw()
        {
            TransactionType = TransactionType.AMMWithdraw;
        }
        #region Implementation of IAMMWithdraw

        /// <inheritdoc />
        [JsonConverter(typeof(IssuedCurrencyConverter))]
        public IssuedCurrency Asset { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(IssuedCurrencyConverter))]
        public IssuedCurrency Asset2 { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency LPTokenIn { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount2 { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency EPrice { get; set; }

        #endregion
    }
    /// <summary>
    /// AMMWithdraw is the withdraw transaction used to remove liquidity from the AMM
    /// instance pool, thus redeeming some share of the pools that one owns in the form
    /// of LPTokenIn.
    /// </summary>
    public interface IAMMWithdraw : ITransactionCommon
    {
        /// <summary>
        /// Specifies one of the pool assets (XRP or token) of the AMM instance.
        /// </summary>
        IssuedCurrency Asset { get; set; }

        /// <summary>
        /// Specifies the other pool asset of the AMM instance.
        /// </summary>
        IssuedCurrency Asset2 { get; set; }

        /// <summary>
        /// Specifies the amount of shares of the AMM instance pools that the trader
        /// wants to redeem or trade in.
        /// </summary>
        Currency LPTokenIn { get; set; }

        /// <summary>
        /// Specifies one of the pools assets that the trader wants to remove. 
        /// If the asset is XRP, then the Amount is a string specifying the number of drops.
        /// Otherwise it is an IssuedCurrencyAmount object.
        /// </summary>
        Currency Amount { get; set; }

        /// <summary>
        /// Specifies the other pool asset that the trader wants to remove.
        /// </summary>
        Currency Amount2 { get; set; }

        /// <summary>
        /// Specifies the effective-price of the token out after successful execution of
        /// the transaction.
        /// </summary>
        Currency EPrice { get; set; }
    }

    /// <inheritdoc cref="IAMMWithdraw" />
    public class AMMWithdrawResponse : TransactionResponseCommon, IAMMWithdraw
    {
        #region Implementation of IAMMWithdraw

        /// <inheritdoc />
        [JsonConverter(typeof(IssuedCurrencyConverter))]
        public IssuedCurrency Asset { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(IssuedCurrencyConverter))]
        public IssuedCurrency Asset2 { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency LPTokenIn { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount2 { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency EPrice { get; set; }

        #endregion
    }

    public partial class Validation
    {

        /// <summary>
        /// Verify the form and type of an AMMWithdraw at runtime.
        /// </summary>
        /// <param name="tx">An AMMWithdraw Transaction.</param>
        /// <throws>When the AMMWithdraw is Malformed.</throws>
        public static async Task ValidateAMMWithdraw(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            tx.TryGetValue("Asset", out var Asset);
            tx.TryGetValue("Asset2", out var Asset2);
            tx.TryGetValue("Amount", out var Amount);
            tx.TryGetValue("Amount2", out var Amount2);
            tx.TryGetValue("EPrice", out var EPrice);
            tx.TryGetValue("LPTokenIn", out var LPTokenIn);

            if (Asset is null)
            {
                throw new ValidationException("AMMWithdraw: missing field Asset");
            }

            if (!Common.IsIssue(Asset))
            {
                throw new ValidationException("AMMWithdraw: Asset must be an Issue");
            }

            if (Asset2 is null)
            {
                throw new ValidationException("AMMWithdraw: missing field Asset2");
            }

            if (!Common.IsIssue(Asset2))
            {
                throw new ValidationException("AMMWithdraw: Asset2 must be an Issue");
            }

            if (Amount2 is not null && Amount is null)
            {
                throw new ValidationException("AMMWithdraw: must set Amount with Amount2");
            }
            else if (EPrice is not null && Amount is null)
            {
                throw new ValidationException("AMMWithdraw: must set Amount with EPrice");
            }

            if (LPTokenIn is not null && !Common.IsIssuedCurrency(LPTokenIn))
            {
                throw new ValidationException("AMMWithdraw: LPTokenIn must be an IssuedCurrencyAmount");
            }

            if (Amount is not null && !Common.IsAmount(Amount))
            {
                throw new ValidationException("AMMWithdraw: Amount must be an Amount");
            }

            if (Amount2 is not null && !Common.IsAmount(Amount2))
            {
                throw new ValidationException("AMMWithdraw: Amount2 must be an Amount");
            }

            if (EPrice is not null && !Common.IsAmount(EPrice))
            {
                throw new ValidationException("AMMWithdraw: EPrice must be an Amount");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Xrpl.Models.Common.Common;
using Xrpl.BinaryCodec.Types;
using Xrpl.Client.Exceptions;

namespace Xrpl.Models.Transactions
{
    /// <summary>
    /// Enum representing values for AMMDeposit Transaction Flags.
    /// </summary>
    public enum AMMDepositFlags : uint
    {
        /// <summary>
        /// Perform a double-asset deposit and receive the specified amount of LP Tokens.
        /// </summary>
        tfLPToken = 65536,//0x00010000
        /// <summary>
        /// Perform a single-asset deposit with a specified amount of the asset to deposit.
        /// </summary>
        tfSingleAsset = 524288,//0x00080000
        /// <summary>
        /// Perform a double-asset deposit with specified amounts of both assets.
        /// </summary>
        tfTwoAsset = 1048576,//0x00100000
        /// <summary>
        /// Perform a single-asset deposit and receive the specified amount of LP Tokens.
        /// </summary>
        tfOneAssetLPToken = 2097152,//0x00200000
        /// <summary>
        /// Perform a single-asset deposit with a specified effective price.
        /// </summary>
        tfLimitLPToken = 4194304 //0x00400000
    };

    //public interface AMMDepositFlagsInterface : GlobalFlags
    //{
    //    bool? tfLPToken { get; set; }
    //    bool? tfSingleAsset { get; set; }
    //    bool? tfTwoAsset { get; set; }
    //    bool? tfOneAssetLPToken { get; set; }
    //    bool? tfLimitLPToken { get; set; }
    //}

    /// <summary>
    /// AMMDeposit is the deposit transaction used to add liquidity to the AMM instance pool,
    /// thus obtaining some share of the instance's pools in the form of LPTokenOut.
    /// The following are the recommended valid combinations:
    /// - LPTokenOut
    /// - Amount
    /// - Amount and Amount2
    /// - Amount and LPTokenOut
    /// - Amount and EPrice
    /// </summary>
    public class AMMDeposit : TransactionCommon, IAMMDeposit
    {
        public AMMDeposit()
        {
            TransactionType = TransactionType.AMMDeposit;
        }
        /// <inheritdoc />
        public Issue Asset { get; set; }

        /// <inheritdoc />
        public Issue Asset2 { get; set; }

        /// <inheritdoc />
        public IssuedCurrencyAmount? LPTokenOut { get; set; }

        /// <inheritdoc />
        public Amount? Amount { get; set; }

        /// <inheritdoc />
        public Amount? Amount2 { get; set; }

        /// <inheritdoc />
        public Amount? EPrice { get; set; }
    }

    /// <summary>
    /// AMMDeposit is the deposit transaction used to add liquidity to the AMM instance pool,
    /// thus obtaining some share of the instance's pools in the form of LPTokenOut.
    /// The following are the recommended valid combinations:
    /// - LPTokenOut
    /// - Amount
    /// - Amount and Amount2
    /// - Amount and LPTokenOut
    /// - Amount and EPrice
    /// </summary>
    public interface IAMMDeposit : ITransactionCommon
    {
        /// <summary>
        /// Specifies one of the pool assets (XRP or token) of the AMM instance.
        /// </summary>
        public Issue Asset { get; set; }

        /// <summary>
        /// Specifies the other pool asset of the AMM instance.
        /// </summary>
        public Issue Asset2 { get; set; }

        /// <summary>
        /// Specifies the amount of shares of the AMM instance pools that the trader wants to redeem or trade in.
        /// </summary>
        public IssuedCurrencyAmount? LPTokenOut { get; set; }

        /// <summary>
        /// Specifies one of the pool assets (XRP or token) of the AMM instance to deposit more of its value.
        /// </summary>
        public Amount? Amount { get; set; }

        /// <summary>
        /// Specifies the other pool asset of the AMM instance to deposit more of its value.
        /// </summary>
        public Amount? Amount2 { get; set; }

        /// <summary>
        /// Specifies the maximum effective-price that LPTokenOut can be traded out.
        /// </summary>
        public Amount? EPrice { get; set; }
    }


    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of an AMMDeposit at runtime.
        /// </summary>
        /// <param name="tx">An AMMDeposit Transaction.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"> When the AMMDeposit is Malformed.</exception>
        public static async Task ValidateAMMDeposit(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            if (!tx.TryGetValue("Asset", out var Asset1) || Asset1 is null)
            {
                throw new ValidationException("AMMDeposit: missing field Asset");
            }

            if (!Xrpl.Models.Transactions.Common.IsIssue(Asset1))
            {
                throw new ValidationException("AMMDeposit: Asset must be an Issue");
            }

            if (!tx.TryGetValue("Asset2", out var Asset2) || Asset2 is null)
            {
                throw new ValidationException("AMMDeposit: missing field Asset2");
            }

            if (!Xrpl.Models.Transactions.Common.IsIssue(Asset2))
            {
                throw new ValidationException("AMMDeposit: Asset2 must be an Issue");
            }

            tx.TryGetValue("Amount", out var Amount);
            tx.TryGetValue("Amount2", out var Amount2);
            tx.TryGetValue("EPrice", out var EPrice);
            tx.TryGetValue("LPTokenOut", out var LPTokenOut);

            if (Amount2 is not null && Amount is null)
                throw new ValidationException("AMMDeposit: must set Amount with Amount2");
            if (EPrice is not null && Amount is null)
                throw new ValidationException("AMMDeposit: must set Amount with EPrice");
            if (LPTokenOut is null && Amount is null)
                throw new ValidationException("AMMDeposit: must set at least LPTokenOut or Amount");

            if (LPTokenOut is not null && !Common.IsIssuedCurrency(LPTokenOut))
                throw new ValidationException("AMMDeposit: LPTokenOut must be an IssuedCurrencyAmount");
            if (Amount is not null && !Common.IsAmount(Amount))
                throw new ValidationException("AMMDeposit: Amount must be an Amount");
            if (Amount2 is not null && !Common.IsAmount(Amount2))
                throw new ValidationException("AMMDeposit: Amount2 must be an Amount");
            if (EPrice is not null && !Common.IsAmount(EPrice))
                throw new ValidationException("AMMDeposit: EPrice must be an Amount");
        }
    }

}

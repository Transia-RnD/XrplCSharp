using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Xrpl.Client;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Methods;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/getFeeXrp.ts

namespace Xrpl.Sugar
{
    public static class GetFeeXrpSugar
    {
        private const int NUM_DECIMAL_PLACES = 6;
        private const int BASE_10 = 10;

        /// <summary>
        /// Calculates the current transaction fee for the ledger.
        /// Note: This is a public API that can be called directly.
        /// </summary>
        /// <param name="client">The Client used to connect to the ledger.</param>
        /// <param name="cushion">The fee cushion to use</param>
        /// <returns>The transaction fee</returns>
        public static async Task<string> GetFeeXrp(this IXrplClient client, double? cushion = null)
        {
            double feeCushion = cushion ?? client.feeCushion;
            ServerInfoRequest request = new ServerInfoRequest();
            ServerInfo serverInfo = await client.ServerInfo(request);
            double? baseFee = serverInfo.Info.ValidatedLedger?.BaseFeeXrp;
            if (baseFee == null)
            {
                throw new XrplException("getFeeXrp: Could not get base_fee_xrp from server_info");
            }

            decimal baseFeeXrp = (decimal)baseFee;

            if (serverInfo.Info.LoadFactor == null)
            {
                // https://github.com/ripple/rippled/issues/3812#issuecomment-816871100
                serverInfo.Info.LoadFactor = 1;
            }

            decimal fee = baseFeeXrp * (decimal)serverInfo.Info.LoadFactor * (decimal)feeCushion;

            // Cap fee to `client.maxFeeXRP`
            fee = Math.Min(fee, decimal.Parse(client.maxFeeXRP));
            // Round fee to 6 decimal places
            // TODO: Review To Fixed
            return fee.ToString(CultureInfo.InvariantCulture);
        }
    }
}


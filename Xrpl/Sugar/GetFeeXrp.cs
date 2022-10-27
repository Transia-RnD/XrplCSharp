using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xrpl;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;
using System.Numerics;
using Xrpl.Client;
using Xrpl.Client.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/getFeeXrp.ts

namespace Xrpl.Sugar
{
    public class GetFeeXrpSugar
    {
        private static int NUM_DECIMAL_PLACES = 6;

        /// <summary>
        /// Calculates the current transaction fee for the ledger.
        /// Note: This is a public API that can be called directly.
        /// </summary>
        /// <param name="client">The Client used to connect to the ledger.</param>
        /// <param name="cushion"></param>
        // <returns>The most recently validated ledger index.</returns>
        public async static Task<string> GetFeeXrp(IXrplClient client, double? cushion = null)
        {
            //double feeCushion = cushion ?? client.feeCushion;
            double feeCushion = (double)cushion;

            ServerInfoRequest request = new ServerInfoRequest();
            ServerInfo serverInfo = await client.ServerInfo(request);
            double baseFee = serverInfo.Info.ValidatedLedger.BaseFeeXrp;
            Debug.WriteLine(baseFee);
            if (baseFee == null)
            {
                throw new XrplError("getFeeXrp: Could not get base_fee_xrp from server_info");
            }
            BigInteger baseFeeXrp = new BigInteger(baseFee);

            if (serverInfo.Info.LoadFactor == null)
            {
                // https://github.com/ripple/rippled/issues/3812#issuecomment-816871100
                serverInfo.Info.LoadFactor = 1;
            }
            BigInteger fee = baseFeeXrp * BigInteger.Parse(serverInfo.Info.LoadFactor.ToString()) * BigInteger.Parse(feeCushion.ToString());

            // Cap fee to `client.maxFeeXRP`
            fee = BigInteger.Min(fee, BigInteger.Parse(client.maxFeeXRP));
            // Round fee to 6 decimal places
            // TODO: Review To Fixed
            return fee.ToString();
        }
    }
}


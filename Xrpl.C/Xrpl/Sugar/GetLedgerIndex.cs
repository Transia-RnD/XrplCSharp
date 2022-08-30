using System;
using System.Threading.Tasks;
using Xrpl.Client;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Methods;

namespace Xrpl.Sugar
{
    public class GetLedgerSugar
    {
        public async static Task<uint> GetLedgerIndex(IRippleClient client)
        {
            LedgerIndex index = new LedgerIndex(LedgerIndexType.Current);
            LedgerRequest request = new LedgerRequest() { LedgerIndex = index };
            LOLedger ledgerResponse = await client.Ledger(request);
            return ledgerResponse.LedgerIndex;
        }
    }
}


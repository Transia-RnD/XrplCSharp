using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;

using Xrpl.AddressCodec;
using Xrpl.Client;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;
using Xrpl.Utils;

using static Xrpl.AddressCodec.XrplAddressCodec;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/autofill.ts

namespace Xrpl.Sugar
{
    public class AddressNTag
    {
        public string ClassicAddress { get; set; }
        public uint? Tag { get; set; }
    }

    public static class AutofillSugar
    {

        const int LEDGER_OFFSET = 20;


        /// <summary>
        /// Autofills fields in a transaction. This will set `Sequence`, `Fee`,
        /// `lastLedgerSequence` according to the current state of the server this Client
        /// is connected to. It also converts all X-Addresses to classic addresses and
        /// flags interfaces into numbers.
        /// </summary>
        /// <param name="client">A client.</param>
        /// <param name="transaction">A {@link Transaction} in JSON format</param>
        /// <param name="signersCount">The expected number of signers for this transaction. Only used for multisigned transactions.</param>
        // <returns>The autofilled transaction.</returns>
        public static async Task<Dictionary<string, dynamic>> Autofill(this IXrplClient client, Dictionary<string, dynamic> transaction, int? signersCount)
        {

            Dictionary<string, dynamic> tx = transaction;

            tx.SetValidAddresses();

            //Flags.SetTransactionFlagsToNumber(tx);
            List<Task> promises = new List<Task>();
            bool hasTT = tx.TryGetValue("TransactionType", out var tt);
            if (!tx.ContainsKey("Sequence"))
            {
                promises.Add(client.SetNextValidSequenceNumber(tx));
            }
            if (!tx.ContainsKey("Fee"))
            {
                promises.Add(client.CalculateFeePerTransactionType(tx, signersCount ?? 0));
            }
            if (!tx.ContainsKey("LastLedgerSequence"))
            {
                promises.Add(client.SetLatestValidatedLedgerSequence(tx));
            }
            if (tt == "AccountDelete")
            {
                //todo error here
                //promises.Add(client.CheckAccountDeleteBlockers(tx));
            }
            await Task.WhenAll(promises);
            string jsonData = JsonConvert.SerializeObject(tx);
            return tx;
        }


        public static void SetValidAddresses(this Dictionary<string, dynamic> tx)
        {
            tx.ValidateAccountAddress("Account", "SourceTag");
            if (tx.ContainsKey("Destination"))
            {
                tx.ValidateAccountAddress("Destination", "DestinationTag");
            }

            // DepositPreauth:
            tx.ConvertToClassicAddress("Authorize");
            tx.ConvertToClassicAddress("Unauthorize");
            // EscrowCancel, EscrowFinish:
            tx.ConvertToClassicAddress("Owner");
            // SetRegularKey:
            tx.ConvertToClassicAddress("RegularKey");
        }

        public static void ValidateAccountAddress(this Dictionary<string, dynamic> tx, string accountField, string tagField)
        {
            // if X-address is given, convert it to classic address
            var ainfo = tx.TryGetValue(accountField, out var aField);
            
            AddressNTag classicAccount = GetClassicAccountAndTag((string)aField, null);
            tx[accountField] = classicAccount.ClassicAddress;

            var tinfo = tx.TryGetValue(tagField, out var tField);

            // XRPL: Does bool or int. Smells.
            if (classicAccount.Tag != null)
            {
                if (tField != null && (int)tField != classicAccount.Tag)
                {
                    throw new ValidationException($"The { tagField }, if present, must match the tag of the { accountField} X - address");
                }
                // eslint-disable-next-line no-param-reassign -- param reassign is safe
                tx[tagField] = classicAccount.Tag;
            }
        }

        public static AddressNTag GetClassicAccountAndTag(this string account, uint? expectedTag)
        {
            if (XrplAddressCodec.IsValidXAddress(account))
            {
                CodecAddress codecAddress = XrplAddressCodec.XAddressToClassicAddress(account);
                if (expectedTag != null && codecAddress.Tag != expectedTag)
                {
                    throw new ValidationException("address includes a tag that does not match the tag specified in the transaction");
                }
                return new AddressNTag { ClassicAddress = codecAddress.ClassicAddress, Tag = codecAddress.Tag };
            }
            return new AddressNTag { ClassicAddress = account, Tag = expectedTag };
        }

        public static void ConvertToClassicAddress(this Dictionary<string, dynamic> tx, string fieldName)
        {
            if (tx.ContainsKey(fieldName))
            {
                string account = (string)tx[fieldName];
                if (account is string)
                {
                    AddressNTag addressntag = account.GetClassicAccountAndTag(null);
                    tx[fieldName] = addressntag.ClassicAddress;
                }
            }
        }

        public static async Task SetNextValidSequenceNumber(this IXrplClient client, Dictionary<string, dynamic> tx)
        {
            LedgerIndex index = new LedgerIndex(LedgerIndexType.Current);
            AccountInfoRequest request = new AccountInfoRequest((string)tx["Account"]) { LedgerIndex = index };
            AccountInfo data = await client.AccountInfo(request);
            tx.TryAdd("Sequence", data.AccountData.Sequence);
        }

        public static async Task<BigInteger> FetchAccountDeleteFee(this IXrplClient client)
        {
            ServerStateRequest request = new ServerStateRequest();
            ServerState data = await client.ServerState(request);
            uint? fee = data.State.ValidatedLedger.ReserveInc;

            if (fee == null)
            {
                await Task.FromException(new XrplException("Could not fetch Owner Reserve."));
            }
            return BigInteger.Parse(fee.ToString());
        }

        public static async Task CalculateFeePerTransactionType(this IXrplClient client, Dictionary<string, dynamic> tx, int signersCount = 0)
        {
            var netFeeXRP = await client.GetFeeXrp();
            var netFeeDrops = XrpConversion.XrpToDrops(netFeeXRP);
            var baseFee = BigInteger.Parse(netFeeDrops, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent);

            // EscrowFinish Transaction with Fulfillment
            bool has_fulfillment = tx.TryGetValue("Fulfillment", out var Fulfillment);
            if (tx["TransactionType"] == "EscrowFinish" && has_fulfillment)
            {
                double fulfillmentBytesSize = Math.Ceiling((double)tx["Fulfillment"].Length / 2);
                // 10 drops × (33 + (Fulfillment size in bytes / 16))
                double resp = (33 + (fulfillmentBytesSize / 16));
                bool product = BigInteger.TryParse(ScaleValue(netFeeDrops, 33 + (fulfillmentBytesSize / 16)), out var result);
                baseFee = BigInteger.Parse(Math.Ceiling(((decimal)result)).ToString());
            }

            // AccountDelete Transaction
            if (tx["TransactionType"] == "AccountDelete")
            {
                baseFee = await FetchAccountDeleteFee(client);
            }

            /*
            * Multi-signed Transaction
            * 10 drops × (1 + Number of Signatures Provided)
            */
            if (signersCount > 0)
            {
                baseFee = BigInteger.Add(baseFee, BigInteger.Parse(ScaleValue(netFeeDrops, 1 + signersCount)));
            }

            var maxFeeDrops = XrpConversion.XrpToDrops(client.maxFeeXRP);
            var totalFee = tx["TransactionType"] == "AccountDelete" ? baseFee : BigInteger.Min(baseFee, BigInteger.Parse(maxFeeDrops));
            tx.TryAdd("Fee", Math.Ceiling(((decimal)totalFee)).ToString());
        }

        public static string ScaleValue(string value, double multiplier)
        {
            return (double.Parse(value)! * multiplier).ToString();
        }

        public static async Task SetLatestValidatedLedgerSequence(this IXrplClient client, Dictionary<string, dynamic> tx)
        {
            uint ledgerSequence = await client.GetLedgerIndex();
            tx.TryAdd("LastLedgerSequence", ledgerSequence + LEDGER_OFFSET);
        }

        public static async Task CheckAccountDeleteBlockers(this IXrplClient client, Dictionary<string, dynamic> tx)
        {
            LedgerIndex index = new LedgerIndex(LedgerIndexType.Validated);
            AccountObjectsRequest request = new AccountObjectsRequest((string)tx["Account"])
            {
                LedgerIndex = index,
                DeletionBlockersOnly = true,
            };
            AccountObjects response = await client.AccountObjects(request);
            TaskCompletionSource task = new TaskCompletionSource();
            if (response.AccountObjectList.Count > 0)
            {
                task.TrySetException(new XrplException($"Account {(string)tx["Account"]} cannot be deleted; there are Escrows, PayChannels, RippleStates, or Checks associated with the account."));
            }
            task.TrySetResult();
        }
    }
}
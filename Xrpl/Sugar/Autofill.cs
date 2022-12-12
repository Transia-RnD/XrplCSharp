using System;
using System.Linq;
using System.Threading.Tasks;
using Xrpl.AddressCodec;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;
using System.Numerics;
using static Xrpl.AddressCodec.XrplAddressCodec;
using System.Collections.Generic;
using Xrpl.Client;
using Xrpl.Client.Exceptions;
using Xrpl.Utils;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Math.EC.Multiplier;
using Xrpl.BinaryCodec.Types;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Xrpl.BinaryCodec.Enums;
using Org.BouncyCastle.Utilities;
using System.Security.Principal;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/autofill.ts

namespace Xrpl.Sugar
{
    public class AutofillSugar
    {

        static readonly int LEDGER_OFFSET = 20;

        public class AddressNTag
        {
            public string ClassicAddress { get; set; }
            public int? Tag { get; set; }
        }

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
        public static async Task<Dictionary<string, dynamic>> Autofill(IXrplClient client, Dictionary<string, dynamic> transaction, int? signersCount)
        {

            //Debug.WriteLine((string)transaction["TransactionType"]);
            Dictionary<string, dynamic> tx = transaction;

            SetValidAddresses(tx);

            //Flags.SetTransactionFlagsToNumber(tx);
            List<Task> promises = new List<Task>();
            bool hasTT = tx.TryGetValue("TransactionType", out var tt);
            if (!tx.ContainsKey("Sequence"))
            {
                // Debug.WriteLine("MISSING: Sequence");
                promises.Add(SetNextValidSequenceNumber(client, tx));
            }
            if (!tx.ContainsKey("Fee"))
            {
                // Debug.WriteLine("MISSING: Fee");
                promises.Add(CalculateFeePerTransactionType(client, tx, signersCount ?? 0));
            }
            if (!tx.ContainsKey("LastLedgerSequence"))
            {
                // Debug.WriteLine("MISSING: LastLedgerSequence");
                promises.Add(SetLatestValidatedLedgerSequence(client, tx));
            }
            if (tt == "AccountDelete")
            {
                // Debug.WriteLine("MISSING: AccountDelete");
                promises.Add(CheckAccountDeleteBlockers(client, tx));
            }
            await Task.WhenAll(promises);
            string jsonData = JsonConvert.SerializeObject(tx);
            return tx;
        }


        public static void SetValidAddresses(Dictionary<string, dynamic> tx)
        {
            ValidateAccountAddress(tx, "Account", "SourceTag");
            if (tx.ContainsKey("Destination"))
            {
                ValidateAccountAddress(tx, "Destination", "DestinationTag");
            }

            // DepositPreauth:
            ConvertToClassicAddress(tx, "Authorize");
            ConvertToClassicAddress(tx, "Unauthorize");
            // EscrowCancel, EscrowFinish:
            ConvertToClassicAddress(tx, "Owner");
            // SetRegularKey:
            ConvertToClassicAddress(tx, "RegularKey");
        }

        public static void ValidateAccountAddress(Dictionary<string, dynamic> tx, string accountField, string tagField)
        {
            // if X-address is given, convert it to classic address
            var ainfo = tx.TryGetValue(accountField, out var aField);
            
            AddressNTag classicAccount = GetClassicAccountAndTag((string)aField, null);
            tx[accountField] = classicAccount.ClassicAddress;

            var tinfo = tx.TryGetValue(tagField, out var tField);

            // XRPL: Does bool or int. Smells.
            //if (classicAccount.Tag != null && classicAccount.Tag != false)
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

        public static AddressNTag GetClassicAccountAndTag(string account, int? expectedTag)
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

        public static void ConvertToClassicAddress(Dictionary<string, dynamic> tx, string fieldName)
        {
            if (tx.ContainsKey(fieldName))
            {
                string account = (string)tx[fieldName];
                if (account is string)
                {
                    AddressNTag addressntag = GetClassicAccountAndTag(account, null);
                    tx[fieldName] = addressntag.ClassicAddress;
                }
            }
        }

        public static async Task SetNextValidSequenceNumber(IXrplClient client, Dictionary<string, dynamic> tx)
        {
            LedgerIndex index = new LedgerIndex(LedgerIndexType.Current);
            AccountInfoRequest request = new AccountInfoRequest((string)tx["Account"]) { LedgerIndex = index };
            AccountInfo data = await client.AccountInfo(request);
            tx.TryAdd("Sequence", data.AccountData.Sequence);
        }

        public static async Task<BigInteger> FetchAccountDeleteFee(IXrplClient client)
        {
            ServerInfoRequest request = new ServerInfoRequest();
            ServerInfo data = await client.ServerInfo(request);
            uint? fee = data.Info.ValidatedLedger.ReserveIncXrp;

            if (fee == null)
            {
                await Task.FromException(new XrplException("Could not fetch Owner Reserve."));
            }
            return new BigInteger(Convert.ToByte(fee));
        }

        public static async Task CalculateFeePerTransactionType(IXrplClient client, Dictionary<string, dynamic> tx, int signersCount = 0)
        {
            var netFeeXRP = await GetFeeXrpSugar.GetFeeXrp(client);
            var netFeeDrops = XrpConversion.XrpToDrops(netFeeXRP);
            var baseFee = BigInteger.Parse(netFeeDrops, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent);

            // EscrowFinish Transaction with Fulfillment
            if (tx["TransactionType"] == "EscrowFinish" && tx["Fulfillment"] != null)
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

        public static async Task SetLatestValidatedLedgerSequence(IXrplClient client, Dictionary<string, dynamic> tx)
        {
            uint ledgerSequence = await client.GetLedgerIndex();
            tx.TryAdd("LastLedgerSequence", ledgerSequence + LEDGER_OFFSET);
        }

        public static async Task CheckAccountDeleteBlockers(IXrplClient client, Dictionary<string, dynamic> tx)
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
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
            Dictionary<string, dynamic> tx = transaction;

            SetValidAddresses(tx);

            //Flags.SetTransactionFlagsToNumber(tx);
            List<Task> promises = new List<Task>();
            if (!tx.ContainsKey("Sequence"))
            {
                promises.Add(SetNextValidSequenceNumber(client, tx));
            }
            if (!tx.ContainsKey("Fee") || tx["Fee"] == "10")
            {
                promises.Append(CalculateFeePerTransactionType(client, tx, signersCount ?? 0));
            }
            if (!tx.ContainsKey("LastLedgerSequence"))
            {
                promises.Append(SetLatestValidatedLedgerSequence(client, tx));
            }
            //if (tx.TransactionType === 'AccountDelete')
            //{
            //    promises.push(CheckAccountDeleteBlockers(client, tx))
            //}
            Task.WaitAll(promises.ToArray());
            //Debug.WriteLine(JsonConvert.SerializeObject(tx));
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
            AddressNTag classicAccount = GetClassicAccountAndTag((string)tx[accountField], null);
            tx[accountField] = classicAccount.ClassicAddress;

            // XRPL: Does bool or int. Smells.
            //if (classicAccount.Tag != null && classicAccount.Tag != false)
            if (classicAccount.Tag != null)
            {
                if (tx[tagField] != null && (int)tx[tagField] != classicAccount.Tag)
                {
                    throw new ValidationError($"The { tagField }, if present, must match the tag of the { accountField} X - address");
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
                    throw new ValidationError("address includes a tag that does not match the tag specified in the transaction");
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
            tx["Sequence"] = data.AccountData.Sequence;
        }

        public static async Task<BigInteger> FetchAccountDeleteFee(IXrplClient client)
        {
            ServerInfoRequest request = new ServerInfoRequest();
            ServerInfo data = await client.ServerInfo(request);
            uint? fee = data.Info.ValidatedLedger.ReserveIncXrp;

            if (fee == null)
            {
                Task.FromException(new XrplError("Could not fetch Owner Reserve."));
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
                var fulfillmentBytesSize = Math.Ceiling(tx["Fulfillment"].Length / 2);
                // 10 drops × (33 + (Fulfillment size in bytes / 16))
                var product = new BigInteger(ScaleValue(netFeeDrops, 33 + fulfillmentBytesSize / 16));
                baseFee = BigInteger.Parse(Math.Ceiling(((decimal)product)).ToString());
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
            tx["Fee"] = Math.Ceiling(((decimal)totalFee)).ToString();
        }

        public static string ScaleValue(string value, int multiplier)
        {
            return (int.Parse(value)! * multiplier).ToString();
        }

    public static async Task SetLatestValidatedLedgerSequence(IXrplClient client, Dictionary<string, dynamic> tx)
        {
            uint ledgerSequence = await client.GetLedgerIndex();
            tx["LastLedgerSequence"] = ledgerSequence + LEDGER_OFFSET;
        }
    }
}
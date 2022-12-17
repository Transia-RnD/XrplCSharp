

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/baseTransaction.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUBaseTransaction
    {

        // todo: ask ripple/xrplf This should actually fail.
        //[TestMethod]
        //public async Task TestVerify_Valid_all_optional_BaseTransaction()
        //{
        //    var tx = new Dictionary<string, dynamic>
        //    {
        //        { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
        //        { "TransactionType", "Payment" },
        //        {"Fee", "12"},
        //        {"Sequence", 100u},
        //        {"AccountTxnID", "DEADBEEF"},
        //        {"Flags", 15u},
        //        {"LastLedgerSequence", 15u},
        //        {"Memos", new List<dynamic>
        //        {
        //            new Dictionary<string,dynamic>()
        //            {
        //                {"MemoType","687474703a2f2f6578616d706c652e636f6d2f6d656d6f2f67656e65726963"},
        //                {"MemoData","72656e74"},
        //            },
        //            new Dictionary<string,dynamic>()
        //            {
        //                {"MemoType","687474703a2f2f6578616d706c652e636f6d2f6d656d6f2f67656e65726963"},
        //                {"MemoData","72656e74"},
        //            },
        //            new Dictionary<string,dynamic>()
        //            {
        //                {"MemoData","72656e74"},
        //            },
        //        }},
        //        {"Signers",new List<dynamic>()
        //        {
        //            new Dictionary<string,dynamic>()
        //            {
        //                { "Account", "r...." },
        //                { "TxnSignature", "DEADBEEF" },
        //                { "SigningPubKey", "hex-string" },

        //            }
        //        }},
        //        {"SourceTag",31u},
        //        {"SigningPublicKey","03680DD274EE55594F7244F489CD38CF3A5A1A4657122FB8143E185B2BA043DF36"},
        //        {"TicketSequence",10u},
        //        {"TxnSignature","3045022100C6708538AE5A697895937C758E99A595B57A16393F370F11B8D4C032E80B532002207776A8E85BB9FAF460A92113B9C60F170CD964196B1F084E0DAB65BAEC368B66"},
        //    };
        //    await Common.ValidateBaseTransaction(tx);
        //}

        [TestMethod]
        public async Task TestVerify_Valid_only_required_BaseTransaction()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
                {"TransactionType", "Payment"},
            };
            await Common.ValidateBaseTransaction(tx);
        }
        [TestMethod]
        public async Task TestVerify_Invalid_Fee()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
                {"TransactionType", "Payment"},
                {"Fee", 1000},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Common.ValidateBaseTransaction(tx), "BaseTransaction: invalid Fee - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_Sequence()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
                {"TransactionType", "Payment"},
                {"Sequence", "145"},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Common.ValidateBaseTransaction(tx), "BaseTransaction: invalid Sequence - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_AccountTxnID()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
                {"TransactionType", "Payment"},
                {"AccountTxnID",new List<dynamic>(){"WRONG"}},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Common.ValidateBaseTransaction(tx), "BaseTransaction: invalid AccountTxnID - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_LastLedgerSequence()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
                {"TransactionType", "Payment"},
                {"LastLedgerSequence","1000"},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Common.ValidateBaseTransaction(tx), "BaseTransaction: invalid LastLedgerSequence - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_SourceTag()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
                {"TransactionType", "Payment"},
                {"SourceTag",new List<dynamic>(){"ARRAY"}},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Common.ValidateBaseTransaction(tx), "BaseTransaction: invalid SourceTag - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_SigningPubKey()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
                {"TransactionType", "Payment"},
                {"SigningPubKey",1000},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Common.ValidateBaseTransaction(tx), "BaseTransaction: invalid SigningPubKey - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_TicketSequence()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
                {"TransactionType", "Payment"},
                {"TicketSequence","1000"},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Common.ValidateBaseTransaction(tx), "BaseTransaction: invalid TicketSequence - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_TxnSignature()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
                {"TransactionType", "Payment"},
                {"TxnSignature",1000},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Common.ValidateBaseTransaction(tx), "BaseTransaction: invalid TxnSignature - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_Signers_1()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
                {"TransactionType", "Payment"},
                {"Signers",new List<dynamic>() { }},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Common.ValidateBaseTransaction(tx), "BaseTransaction: invalid Signers - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_Signers_2()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
                {"TransactionType", "Payment"},
                {"Signers",new List<dynamic>()
                {
                    new Dictionary<string,dynamic>()
                    {
                        { "Account", "r...." },

                    }
                }},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Common.ValidateBaseTransaction(tx), "BaseTransaction: invalid Signers - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_Memo()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "Account", "r97KeayHuEsDwyU1yPBVtMLLoQr79QcRFe" },
                {"TransactionType", "Payment"},
                {"Memos", new List<dynamic>
                {
                    new Dictionary<string,dynamic>()
                    {
                        {"MemoType","HI"},
                        {"MemoData","WRONG"}, //todo no memo check for hex
                    },
                }},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Common.ValidateBaseTransaction(tx), "BaseTransaction: invalid Memos");
        }
    }

}


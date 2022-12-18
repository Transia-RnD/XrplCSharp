

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/signerListSet.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    public class TestSignerListSet
    {
        public TestSignerListSet()
        {
        }
    }
    [TestClass]
    public class TestUSignerListSet
    {
        public static Dictionary<string, dynamic> signerListSetTx;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            signerListSetTx = new Dictionary<string, dynamic>
            {
                {"Flags", 0u},
                {"TransactionType", "SignerListSet"},
                {"Account", "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn"},
                {"Fee", "12"},
                {"SignerQuorum", 3u},
                {"SignerEntries",new List<dynamic>()
                {
                    new Dictionary<string,dynamic>()
                    {
                        { "SignerEntry", new Dictionary<string,dynamic>()
                            {
                                { "Account", "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW" },
                                { "SignerWeight", 2 },
                            }
                        }
                    },
                    new Dictionary<string,dynamic>()
                    {
                        { "SignerEntry", new Dictionary<string,dynamic>()
                            {
                                { "Account", "rUpy3eEg8rqjqfUoLeBnZkscbKbFsKXC3v" },
                                { "SignerWeight", 1 },
                            }
                        },
                    },
                    new Dictionary<string,dynamic>()
                    {
                        { "SignerEntry", new Dictionary<string,dynamic>()
                            {
                                { "Account", "raKEEVSGnKSD9Zyvxu4z6Pqpm4ABH8FS6n" },
                                { "SignerWeight", 1 },
                            }
                        },
                    }
                }},
                {"RegularKey", "rAR8rR8sUkBoCZFawhkWzY4Y5YoyuznwD"},
            };
        }

        [TestMethod]
        public async Task TestVerifyValid()
        {
            //verifies valid SignerListSet
            await Validation.ValidateSignerListSet(signerListSetTx);
            await Validation.Validate(signerListSetTx);


            // throws w/ missing SignerQuorum
            signerListSetTx.Remove("SignerQuorum");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateSignerListSet(signerListSetTx), "SignerListSet: missing field SignerQuorum");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(signerListSetTx), "SignerListSet: missing field SignerQuorum");
            signerListSetTx["RegularKey"] = "rAR8rR8sUkBoCZFawhkWzY4Y5YoyuznwD";

            // throws w/ missing SignerEntries
            signerListSetTx["SignerEntries"] = new List<dynamic>();
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateSignerListSet(signerListSetTx), "SignerListSet: need at least 1 member in SignerEntries");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(signerListSetTx), "SignerListSet: need at least 1 member in SignerEntries");
            signerListSetTx["SignerEntries"] = new List<dynamic>()
            {
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW" },
                            { "SignerWeight", 2 },
                        }
                    }
                },
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rUpy3eEg8rqjqfUoLeBnZkscbKbFsKXC3v" },
                            { "SignerWeight", 1 },
                        }
                    },
                },
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "raKEEVSGnKSD9Zyvxu4z6Pqpm4ABH8FS6n" },
                            { "SignerWeight", 1 },
                        }
                    },
                }
            };

            // throws w/ missing SignerEntries
            signerListSetTx["SignerEntries"] = "khgfgyhujk";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateSignerListSet(signerListSetTx), "SignerListSet: invalid SignerEntries");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(signerListSetTx), "SignerListSet: invalid SignerEntries");
            signerListSetTx["SignerEntries"] = new List<dynamic>()
            {
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW" },
                            { "SignerWeight", 2 },
                        }
                    }
                },
                new  Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rUpy3eEg8rqjqfUoLeBnZkscbKbFsKXC3v" },
                            { "SignerWeight", 1 },
                        }
                    },
                },
                new  Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "raKEEVSGnKSD9Zyvxu4z6Pqpm4ABH8FS6n" },
                            { "SignerWeight", 1 },
                        }
                    },
                }
            };

            // throws w/ maximum of 32 members allowed in SignerEntries
            var accounts = new List<string>()
            {
                "rBFBipte4nAQCTsRxd2czwvSurhCpAf4X6",
                "r3ijUH32iiy9tYNj3rD7hKWYjy1BFUxngm",
                "rpwq8vi4Mn3L5kDJmb8Mg59CanPFPzMCnj",
                "rB72Gzqfejai46nkA4HaKYBHwAnn2yUoT4",
                "rGqsJSAW71pCfUwDD5m52bLw69RzFg6kMW",
                "rs8smPRA31Ym4mGxb1wzgwxtU5eVK82Gyk",
                "rLrugpGxzezUQLDh7Jv1tZpouuV4MQLbU9",
                "rUQ6zLXQdh1jJLGwMXp9P8rgi42kwuafzs",
                "rMjY8sPdfxsyRrnVKQcutxr4mTHNXy9dEF",
                "rUaxYLeFGm6SmMoa2WCqLKSyHwJyvaQmeG",
                "r9wUfeVtqMfqrcDTfCpNYbNZvs5q9M9Rpo",
                "rQncVNak5kvJGPUFa6fuKH7t8Usjs7Np1c",
                "rnwbSSnPbVbUzuBa4etkeYrfy5v7SyhtPu",
                "rDXh5D3t48MdBJyXByXq47k5P8Kuf1758B",
                "rh1D4jd2mAiqUPHfAZ2cY9Nbfa3kAkaQXP",
                "r9T129tXgtnyfGoLeS35c2HctaZAZSQoCH",
                "rUd2uKsyCWfJP7Ve36mKoJbNCA7RYThnYk",
                "r326x8PaAFtnaH7uoxaKrcDWuwpeHn4wDa",
                "rpN3mkXkYhfNadcXPrY4LniM1KpM3egyQM",
                "rsPKbR155hz1zrA4pSJp5Y2fxasZAatcHb",
                "rsyWFLaEKTpaoSJusjpcDvGexuHCwMnqss",
                "rUbc5RXfyF81oLDMgd3d7jpY9YMNMZG4XN",
                "rGpYHM88BZe1iVKFHm5xiWYYxR74oxJEXf",
                "rPsetWAtR1KxDtxzgHjRMD7Rc87rvXk5nD",
                "rwSeNhL6Hi34igr12mCr61jY42psfTkWTq",
                "r46Mygy98qjkDhVB6qs4sBnqaf7FPiA2vU",
                "r4s8GmeYN4CiwVate1nMUvwMQbundqf5cW",
                "rKAr4dQWDYG8cG2hSwJUVp4ry4WNaWiNgp",
                "rPWXRLp1vqeUHEH3WiSKuyo9GM9XhaENQU",
                "rPgmdBdRKGmndxNEYxUrrsYCZaS6go9RvW",
                "rPDJZ9irzgwKRKScfEmuJMvUgrqZAJNCbL",
                "rDuU2uSXMfEaoxN1qW8sj7aUNFLGEn3Hr2",
                "rsbjSjA4TCB9gtm7x7SrWbZHB6g4tt9CGU",
            };

            signerListSetTx["SignerQuorum"] = (uint)accounts.Count;
            signerListSetTx["SignerEntries"] = new List<dynamic>(
                accounts.Select(
                    c => new Dictionary<string, dynamic>()
                    {
                        { "SignerEntry", new Dictionary<string,dynamic>()
                            {
                                { "Account", c },
                                { "SignerWeight", 1 },
                            }
                        }

                    }));
            var error = "SignerListSet: maximum of 32 members allowed in SignerEntries";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateSignerListSet(signerListSetTx), error);
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(signerListSetTx), error);
            signerListSetTx["SignerEntries"] = new List<dynamic>()
            {
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW" },
                            { "SignerWeight", 2 },
                        }
                    }
                },
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rUpy3eEg8rqjqfUoLeBnZkscbKbFsKXC3v" },
                            { "SignerWeight", 1 },
                        }
                    },
                },
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "raKEEVSGnKSD9Zyvxu4z6Pqpm4ABH8FS6n" },
                            { "SignerWeight", 1 },
                        }
                    },
                }
            };

            // verifies valid WalletLocator in SignerEntries
            signerListSetTx["SignerQuorum"] = 3u;
            signerListSetTx["SignerEntries"] = new List<dynamic>()
            {
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rBFBipte4nAQCTsRxd2czwvSurhCpAf4X6" },
                            { "SignerWeight", 1 },
                            { "WalletLocator", "CAFECAFECAFECAFECAFECAFECAFECAFECAFECAFECAFECAFECAFECAFECAFECAFE" },
                        }
                    }
                },
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "r3ijUH32iiy9tYNj3rD7hKWYjy1BFUxngm" },
                            { "SignerWeight", 1 },
                        }
                    },
                },
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rpwq8vi4Mn3L5kDJmb8Mg59CanPFPzMCnj" },
                            { "SignerWeight", 1 },
                            { "WalletLocator", "00000000000000000000000000000000000000000000000000000000DEADBEEF" },
                        }
                    },
                }
            };
            await Validation.ValidateSignerListSet(signerListSetTx);
            await Validation.Validate(signerListSetTx);
            signerListSetTx["SignerEntries"] = new List<dynamic>()
            {
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW" },
                            { "SignerWeight", 2 },
                        }
                    }
                },
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rUpy3eEg8rqjqfUoLeBnZkscbKbFsKXC3v" },
                            { "SignerWeight", 1 },
                        }
                    },
                },
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "raKEEVSGnKSD9Zyvxu4z6Pqpm4ABH8FS6n" },
                            { "SignerWeight", 1 },
                        }
                    },
                }
            };

            // verifies valid WalletLocator in SignerEntries
            signerListSetTx["SignerQuorum"] = 2u;
            signerListSetTx["SignerEntries"] = new List<dynamic>()
            {
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rBFBipte4nAQCTsRxd2czwvSurhCpAf4X6" },
                            { "SignerWeight", 1 },
                            { "WalletLocator", "not_valid" },
                        }
                    }
                },
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "r3ijUH32iiy9tYNj3rD7hKWYjy1BFUxngm" },
                            { "SignerWeight", 1 },
                        }
                    },
                },
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateSignerListSet(signerListSetTx), "SignerListSet: WalletLocator in SignerEntry must be a 256-bit (32-byte) hexadecimal value");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(signerListSetTx), "SignerListSet: WalletLocator in SignerEntry must be a 256-bit (32-byte) hexadecimal value");
            signerListSetTx["SignerEntries"] = new List<dynamic>()
            {
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW" },
                            { "SignerWeight", 2 },
                        }
                    }
                },
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "rUpy3eEg8rqjqfUoLeBnZkscbKbFsKXC3v" },
                            { "SignerWeight", 1 },
                        }
                    },
                },
                new Dictionary<string, dynamic>()
                {
                    {
                        "SignerEntry", new Dictionary<string, dynamic>()
                        {
                            { "Account", "raKEEVSGnKSD9Zyvxu4z6Pqpm4ABH8FS6n" },
                            { "SignerWeight", 1 },
                        }
                    },
                }
            };

        }
    }

}


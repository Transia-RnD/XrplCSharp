

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/paymentChannelClaim.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Models.Common;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUPaymentChannelClaim
    {
        public static Dictionary<string, dynamic> channel;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            channel = new Dictionary<string, dynamic>
            {
                {"TransactionType", "PaymentChannelClaim"},
                {"Account", "rB5Ux4Lv2nRx6eeoAAsZmtctnBQ2LiACnk"},
                {"Channel", "C1AE6DDDEEC05CF2978C0BAD6FE302948E9533691DC749DCDD3B9E5992CA6198"},
                {"Balance", new Currency() { ValueAsXrp = 10000000 }},
                {"Amount", new Currency() { ValueAsXrp = 10000000 }},
                {"Signature", "30440220718D264EF05CAED7C781FF6DE298DCAC68D002562C9BF3A07C1E721B420C0DAB02203A5A4779EF4D2CCC7BC3EF886676D803A9981B928D3B8ACA483B80ECA3CD7B9B"},
                {"PublicKey", "32D2471DB72B27E3310F355BB33E339BF26F8392D5A93D3BC0FC3B566612DA0F0A"},
            };
        }

        [TestMethod]
        public async Task TestVerifyValid()
        {

            //verifies valid PaymentChannelClaim
            await Validation.ValidatePaymentChannelClaim(channel);
            await Validation.Validate(channel);

            // verifies valid PaymentChannelClaim w/o optional
            channel.Remove("Balance");
            channel.Remove("Amount");
            channel.Remove("Signature");
            channel.Remove("PublicKey");
            await Validation.ValidatePaymentChannelClaim(channel);
            await Validation.Validate(channel);
            channel["Balance"] = "1000000";
            channel["Amount"] = "1000000";
            channel["Signature"] = "30440220718D264EF05CAED7C781FF6DE298DCAC68D002562C9BF3A07C1E721B420C0DAB02203A5A4779EF4D2CCC7BC3EF886676D803A9981B928D3B8ACA483B80ECA3CD7B9B";
            channel["PublicKey"] = "32D2471DB72B27E3310F355BB33E339BF26F8392D5A93D3BC0FC3B566612DA0F0A";


            // throws w/ missing Channel
            channel.Remove("Channel");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePaymentChannelClaim(channel), "PaymentChannelClaim: missing Channel");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(channel), "PaymentChannelClaim: missing Channel");
            channel["Channel"] = "C1AE6DDDEEC05CF2978C0BAD6FE302948E9533691DC749DCDD3B9E5992CA6198";

            // throws w/ invalid Channel
            channel["Channel"] = 100;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePaymentChannelClaim(channel), "PaymentChannelClaim: Channel must be a string");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(channel), "PaymentChannelClaim: Channel must be a string");
            channel["Channel"] = "C1AE6DDDEEC05CF2978C0BAD6FE302948E9533691DC749DCDD3B9E5992CA6198";

            // throws w/ invalid Balance
            channel["Balance"] = 100;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePaymentChannelClaim(channel), "PaymentChannelClaim: Balance must be a Currency");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(channel), "PaymentChannelClaim: Balance must be a Currency");
            channel["Balance"] = "1000000";

            // throws w/ invalid Amount
            channel["Amount"] = 100;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePaymentChannelClaim(channel), "PaymentChannelClaim: Amount must be a Currency");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(channel), "PaymentChannelClaim: Amount must be a Currency");
            channel["Amount"] = "1000000";

            // throws w/ invalid Signature
            channel["Signature"] = 1000;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePaymentChannelClaim(channel), "PaymentChannelClaim: Signature must be a string");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(channel), "PaymentChannelClaim: Signature must be a string");
            channel["Signature"] = "30440220718D264EF05CAED7C781FF6DE298DCAC68D002562C9BF3A07C1E721B420C0DAB02203A5A4779EF4D2CCC7BC3EF886676D803A9981B928D3B8ACA483B80ECA3CD7B9B";

            // throws w/ invalid PublicKey
            channel["PublicKey"] = new List<string>() { "100000" };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePaymentChannelClaim(channel), "PaymentChannelClaim: PublicKey must be a string");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(channel), "PaymentChannelClaim: PublicKey must be a string");
            channel["PublicKey"] = "32D2471DB72B27E3310F355BB33E339BF26F8392D5A93D3BC0FC3B566612DA0F0A";

        }
    }

}


using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Types;

namespace Ripple.Binary.Codec.Tests
{
    [TestClass]
    public class AmountTests
    {
        [TestMethod, ExpectedException(typeof(InvalidJsonException))]
        public void TestInvalidJToken()
        {
            var token = JToken.Parse("[1,2,3]");
            Amount.FromJson(token);
        }

        [TestMethod]
        public void TestIntegerJTokenConversion()
        {
            var a = Amount.FromJson(JToken.Parse("10"));
            Assert.IsTrue(a.IsNative);
        }

        [TestMethod]
        public void TestImplicitIntegerConversion()
        {
            Amount b = 10;
            Assert.IsTrue(b.IsNative);
        }
    }
}

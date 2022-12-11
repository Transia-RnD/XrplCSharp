

using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client;
using Xrpl.Client.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/client/constructor.ts

namespace XrplTests.Xrpl.ClientLib
{
    [TestClass]
    public class TestUConstructor
    {

        //public static SetupUnitClient runner;

        //[ClassInitialize]
        //public static async Task MyClassInitializeAsync(TestContext testContext)
        //{
        //    runner = await new SetupUnitClient().SetupClient();
        //}

        //[ClassCleanup]
        //public static void MyClassCleanupAsync()
        //{
        //    runner.client.Disconnect().Wait();
        //}

        [TestMethod]
        public void TestImplicitPort()
        {
            new XrplClient("wss://s1.ripple.com");
        }

        //[TestMethod]
        //public void TestInvalidOptions()
        //{
        //    new XrplClient("wss://s1.ripple.com");
        //}

        [TestMethod]
        public void TestValidOptions()
        {
            XrplClient client = new XrplClient("wss://s:1");
            string privateConnectionUrl = client.Url();
            Assert.AreEqual("wss://s:1", privateConnectionUrl);
        }

        //[TestMethod]
        //[ExpectedException(typeof(XrplException))]
        //public void TestInvalidServer()
        //{
        //    new XrplClient("wss://s:1");
        //}
    }
}


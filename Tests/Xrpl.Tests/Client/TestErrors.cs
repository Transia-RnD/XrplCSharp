using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client;
using Xrpl.Client.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/client/errors.ts

namespace XrplTests.Xrpl.ClientLib
{
    [TestClass]
    public class TestUErrors
    {
        //[TestMethod]
        //public void TestErrorWithData()
        //{
        //    XrplError error = new XrplError("_message_", "_data_");
        //    Assert.AreEqual("[XrplError(_message_, '_data_')]", error.ToString());
        //}

        [TestMethod]
        public void TestErrorNotFound()
        {
            XrplError error = new NotFoundError();
            Assert.AreEqual("Xrpl.Client.Exceptions.NotFoundError: Not Found", error.ToString());
        }
    }
}


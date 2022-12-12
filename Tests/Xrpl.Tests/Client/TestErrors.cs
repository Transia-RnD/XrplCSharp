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
        //    XrplException error = new XrplException("_message_", "_data_");
        //    Assert.AreEqual("[XrplException(_message_, '_data_')]", error.ToString());
        //}

        [TestMethod]
        public void TestErrorNotFound()
        {
            XrplException error = new NotFoundException();
            Assert.AreEqual("Xrpl.Client.Exceptions.NotFoundException: Not Found", error.ToString());
        }
    }
}


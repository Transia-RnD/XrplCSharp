using Microsoft.VisualStudio.TestTools.UnitTesting;

using Xrpl.BinaryCodecLib.Binary;

// https://github.com/XRPLF/xrpl-py/blob/master/tests/unit/core/binarycodec/test_binary_parser.py

namespace XrplTests.BinaryCodecTests
{
    [TestClass]
    public class BinaryParserTest
    {
        [TestMethod]
        public void TestPeekSkipReadMethods()
        {
            string test_hex = "00112233445566";
            byte[]  testBytes = test_hex.FromHex();
            BufferParser binaryParser = new BufferParser(test_hex);

            byte firstByte = binaryParser.Peek();
            Assert.AreEqual(firstByte, testBytes[0]);

            binaryParser.Skip(3);
            //Assert.AreEqual(testBytes[3:], binaryParser.ToBytes());

            var nextNBytes = binaryParser.Read(2);
            //Assert.AreEqual(testBytes[3:5], nextNBytes);
        }

        //[TestMethod]
        //public void TestIntReadMethods()
        //{
        //    string test_hex = "01000200000003";
        //    BufferParser binaryParser = new BufferParser(test_hex);

        //    var int8 = binaryParser.Read;
        //    Assert.AreEqual(firstByte, testBytes[0]);
        //}
    }
}


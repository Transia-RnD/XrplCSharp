using System;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Utilities;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Types;

// https://github.com/XRPLF/xrpl-py/blob/master/tests/unit/core/binarycodec/test_binary_parser.py

namespace Xrpl.BinaryCodec.Tests
{
    [TestClass]
    public class TestBinaryParser
    {
        [TestMethod]
        public void TestPeekSkipReadMethods()
        {
            string test_hex = "00112233445566";
            byte[] testBytes = test_hex.FromHex();
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

        //    var int8 = binaryParser.ReadUInt8();
        //    var int16 = binaryParser.ReadUInt16();
        //    var int32 = binaryParser.ReadUInt32();
        //    Assert.AreEqual(int8, 1);
        //    Assert.AreEqual(int16, 2);
        //    Assert.AreEqual(int32, 3);
        //}

        //[TestMethod]
        //public void TestReadVariableLength()
        //{
        //    int[] cases = { 100 };
        //    for (var i = 0; i < cases.Length; i++)
        //    {
        //        int _case = cases[i];
        //        BytesList list = new BytesList();
        //        BinarySerializer binarySerializer = new BinarySerializer(list);
        //        string byteString = "A2".Repeat(_case);
        //        Blob blob = Blob.FromHex(byteString);

        //        binarySerializer.AddLengthEncoded(blob);
        //        // hex string representation of encoded length prefix
        //        string encodedLength = binarySerializer._sink.ToString();
        //        BufferParser binaryParser = new BufferParser(encodedLength);
        //        int decodedLength = binaryParser.ReadVlLength();
        //        Assert.AreEqual(_case, decodedLength);
        //    }
        //}
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Types;
using XrplTests.BinaryCodecTests;

// https://github.com/XRPLF/xrpl-py/blob/master/tests/unit/core/binarycodec/test_binary_serializer.py

namespace XrplTests.BinaryCodecTests
{
    [TestClass]
    public class TestBinarySerializer
    {
        [TestMethod]
        public void TestWriteLengthEncoded()
        {
            int[] cases = { 100 };
            for (var i = 0; i < cases.Length; i++)
            {
                int _case = cases[i];
                BytesList list = new BytesList();
                BinarySerializer binarySerializer = new BinarySerializer(list);
                string byteString = "A2".Repeat(_case);
                Blob blob = Blob.FromHex(byteString);
                Assert.AreEqual(blob.Buffer.Length, _case);

                binarySerializer.WriteLengthEncoded(blob);
                // hex string representation of encoded length prefix
                BufferParser binaryParser = new BufferParser(binarySerializer._sink.ToHex());
                int decodedLength = binaryParser.ReadLengthPrefix();
                Assert.AreEqual(_case, decodedLength);
            }
        }
    }
}
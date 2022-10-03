//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Xrpl.BinaryCodec.Binary;
//using Xrpl.BinaryCodec.Types;
//using Xrpl.BinaryCodec.Tests;

//// https://github.com/XRPLF/xrpl-py/blob/master/tests/unit/core/binarycodec/test_binary_serializer.py

//namespace Xrpl.BinaryCodec.Tests
//{
//    [TestClass]
//    public class TestBinarySerializer
//    {
//        [TestMethod]
//        public void TestWriteLengthEncoded()
//        {
//            int[] cases = { 100 };
//            for (var i = 0; i < cases.Length; i++)
//            {
//                int _case = cases[i];
//                BytesList list = new BytesList();
//                BinarySerializer binarySerializer = new BinarySerializer(list);
//                string byteString = "A2".Repeat(_case);
//                Blob blob = Blob.FromHex(byteString);
//                Assert.AreEqual(blob.Buffer.Length, _case);

//                binarySerializer.AddLengthEncoded(blob);
//                // hex string representation of encoded length prefix
//                BufferParser binaryParser = new BufferParser(binarySerializer._sink.ToString());
//                int decodedLength = binaryParser.ReadVlLength();
//                Assert.AreEqual(_case, decodedLength);
//            }
//        }
//    }
//}
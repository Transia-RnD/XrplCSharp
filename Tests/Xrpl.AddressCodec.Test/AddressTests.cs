using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Diagnostics;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-address-codec/src/index.test.js

namespace Xrpl.AddressCodec.Tests
{
    using static Xrpl.AddressCodec.XrplAddressCodec;
    using static XrplAddressCodec;

    [TestClass]
    public class TestUAddressCodec
    {

        private static uint MAX_32_BIT_UNSIGNED_INT = 4294967295;

        [TestMethod]
        public void InvalidXAddressF()
        {
            var _testCases = "[[\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",false,\"X7AcgcsBL6XDcUb289X4mJ8djcdyKaB5hJDWMArnXr61cqZ\",\"T719a5UwUCnEs54UsxG9CJYYDhwmFCqkr7wxCcNcfZ6p5GZ\"],[\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",1,\"X7AcgcsBL6XDcUb289X4mJ8djcdyKaGZMhc9YTE92ehJ2Fu\",\"T719a5UwUCnEs54UsxG9CJYYDhwmFCvbJNZbi37gBGkRkbE\"],[\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",14,\"X7AcgcsBL6XDcUb289X4mJ8djcdyKaGo2K5VpXpmCqbV2gS\",\"T719a5UwUCnEs54UsxG9CJYYDhwmFCvqXVCALUGJGSbNV3x\"],[\"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",11747,\"X7AcgcsBL6XDcUb289X4mJ8djcdyKaLFuhLRuNXPrDeJd9A\",\"T719a5UwUCnEs54UsxG9CJYYDhwmFCziiNHtUukubF2Mg6t\"],[\"rLczgQHxPhWtjkaQqn3Q6UM8AbRbbRvs5K\",false,\"XVZVpQj8YSVpNyiwXYSqvQoQqgBttTxAZwMcuJd4xteQHyt\",\"TVVrSWtmQQssgVcmoMBcFQZKKf56QscyWLKnUyiuZW8ALU4\"],[\"rpZc4mVfWUif9CRoHRKKcmhu1nx2xktxBo\",false,\"X7YenJqxv3L66CwhBSfd3N8RzGXxYqPopMGMsCcpho79rex\",\"T77wVQzA8ntj9wvCTNiQpNYLT5hmhRsFyXDoMLqYC4BzQtV\"],[\"rpZc4mVfWUif9CRoHRKKcmhu1nx2xktxBo\",58,\"X7YenJqxv3L66CwhBSfd3N8RzGXxYqV56ZkTCa9UCzgaao1\",\"T77wVQzA8ntj9wvCTNiQpNYLT5hmhR9kej6uxm4jGcQD7rZ\"],[\"rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW\",23480,\"X7d3eHCXzwBeWrZec1yT24iZerQjYL8m8zCJ16ACxu1BrBY\",\"T7YChPFWifjCAXLEtg5N74c7fSAYsvSokwcmBPBUZWhxH5P\"],[\"rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW\",11747,\"X7d3eHCXzwBeWrZec1yT24iZerQjYLo2CJf8oVC5CMWey5m\",\"T7YChPFWifjCAXLEtg5N74c7fSAYsvTcc7nEfwuEEvn5Q4w\"],[\"rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf\",false,\"XVLhHMPHU98es4dbozjVtdWzVrDjtV5fdx1mHp98tDMoQXb\",\"TVE26TYGhfLC7tQDno7G8dGtxSkYQn49b3qD26PK7FcGSKE\"],[\"rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf\",0,\"XVLhHMPHU98es4dbozjVtdWzVrDjtV8AqEL4xcZj5whKbmc\",\"TVE26TYGhfLC7tQDno7G8dGtxSkYQnSy8RHqGHoGJ59spi2\"],[\"rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf\",1,\"XVLhHMPHU98es4dbozjVtdWzVrDjtV8xvjGQTYPiAx6gwDC\",\"TVE26TYGhfLC7tQDno7G8dGtxSkYQnSz1uDimDdPYXzSpyw\"],[\"rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf\",2,\"XVLhHMPHU98es4dbozjVtdWzVrDjtV8zpDURx7DzBCkrQE7\",\"TVE26TYGhfLC7tQDno7G8dGtxSkYQnTryP9tG9TW8GeMBmd\"],[\"rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf\",32,\"XVLhHMPHU98es4dbozjVtdWzVrDjtVoYiC9UvKfjKar4LJe\",\"TVE26TYGhfLC7tQDno7G8dGtxSkYQnT2oqaCDzMEuCDAj1j\"],[\"rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf\",276,\"XVLhHMPHU98es4dbozjVtdWzVrDjtVoKj3MnFGMXEFMnvJV\",\"TVE26TYGhfLC7tQDno7G8dGtxSkYQnTMgJJYfAbsiPsc6Zg\"],[\"rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf\",65591,\"XVLhHMPHU98es4dbozjVtdWzVrDjtVozpjdhPQVdt3ghaWw\",\"TVE26TYGhfLC7tQDno7G8dGtxSkYQn7ryu2W6njw7mT1jmS\"],[\"rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf\",16781933,\"XVLhHMPHU98es4dbozjVtdWzVrDjtVqrDUk2vDpkTjPsY73\",\"TVE26TYGhfLC7tQDno7G8dGtxSkYQnVsw45sDtGHhLi27Qa\"],[\"rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf\",4294967294,\"XVLhHMPHU98es4dbozjVtdWzVrDjtV1kAsixQTdMjbWi39u\",\"TVE26TYGhfLC7tQDno7G8dGtxSkYQnX8tDFQ53itLNqs6vU\"],[\"rGWrZyQqhTp9Xu7G5Pkayo7bXjH4k4QYpf\",4294967295,\"XVLhHMPHU98es4dbozjVtdWzVrDjtV18pX8yuPT7y4xaEHi\",\"TVE26TYGhfLC7tQDno7G8dGtxSkYQnXoy6kSDh6rZzApc69\"],[\"rPEPPER7kfTD9w2To4CQk6UCfuHM9c6GDY\",false,\"XV5sbjUmgPpvXv4ixFWZ5ptAYZ6PD2gYsjNFQLKYW33DzBm\",\"TVd2rqMkYL2AyS97NdELcpeiprNBjwLZzuUG5rZnaewsahi\"],[\"rPEPPER7kfTD9w2To4CQk6UCfuHM9c6GDY\",0,\"XV5sbjUmgPpvXv4ixFWZ5ptAYZ6PD2m4Er6SnvjVLpMWPjR\",\"TVd2rqMkYL2AyS97NdELcpeiprNBjwRQUBetPbyrvXSTuxU\"],[\"rPEPPER7kfTD9w2To4CQk6UCfuHM9c6GDY\",13371337,\"XV5sbjUmgPpvXv4ixFWZ5ptAYZ6PD2qwGkhgc48zzcx6Gkr\",\"TVd2rqMkYL2AyS97NdELcpeiprNBjwVUDvp3vhpXbNhLwJi\"]]";
            List<List<dynamic>> testCases = JsonConvert.DeserializeObject<List<List<dynamic>>>(_testCases);

            // MAIN
            for (int i = 0; i < testCases.Count; i++)
            {
                var testCase = testCases[i];
                string classicAddress = testCase[0];
                uint? tag = testCase[1] is bool && testCase[1] == false ? null : (uint)testCase[1];
                string xAddress = testCase[2];
                Assert.AreEqual(xAddress, ClassicAddressToXAddress(classicAddress, tag, false));
                CodecAddress myClassicAddress = XAddressToClassicAddress(xAddress);
                Assert.AreEqual(myClassicAddress.ClassicAddress, classicAddress);
                Assert.AreEqual(myClassicAddress.Tag, tag);
                Assert.AreEqual(myClassicAddress.Test, false);
                Assert.IsTrue(IsValidXAddress(xAddress));

            }

            // TEST
            for (int i = 0; i < testCases.Count; i++)
            {
                var testCase = testCases[i];
                string classicAddress = testCase[0];
                uint? tag = testCase[1] is bool && testCase[1] == false ? null : (uint)testCase[1];
                string xAddress = testCase[3];
                Assert.AreEqual(xAddress, ClassicAddressToXAddress(classicAddress, tag, true));
                CodecAddress myClassicAddress = XAddressToClassicAddress(xAddress);
                Assert.AreEqual(myClassicAddress.ClassicAddress, classicAddress);
                Assert.AreEqual(myClassicAddress.Tag, tag);
                Assert.AreEqual(myClassicAddress.Test, true);
                Assert.IsTrue(IsValidXAddress(xAddress));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AddressCodecException), "Unsupported X-address")]
        public void InvalidXAddress()
        {
            XAddressToClassicAddress("XVLhHMPHU98es4dbozjVtdWzVrDjtV18pX8zeUygYrCgrPh");
        }

        [TestMethod]
        [ExpectedException(typeof(AddressCodecException), "Account ID must be 20 bytes")]
        public void InvalidAccountID()
        {
            EncodeXAddress(new byte[] { 0x0, 0x0, 0x0 }, 0, false);
        }

        [TestMethod]
        public void InvalidXAddressReturns()
        {
            Assert.IsFalse(IsValidXAddress("XVLhHMPHU98es4dbozjVtdWzVrDjtV18pX8zeUygYrCgrPh"));
        }

        [TestMethod]
        public void ConvertTagFalse()
        {
            string classicAddress = "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59";
            uint? tag = null;
            string xAddress = "X7AcgcsBL6XDcUb289X4mJ8djcdyKaB5hJDWMArnXr61cqZ";
            bool isTestAddress = false;
            Assert.AreEqual(xAddress, ClassicAddressToXAddress(classicAddress, tag, isTestAddress));
            CodecAddress myClassicAddress = XAddressToClassicAddress(xAddress);
            Assert.AreEqual(myClassicAddress.ClassicAddress, classicAddress);
            Assert.AreEqual(myClassicAddress.Tag, tag);
            Assert.AreEqual(myClassicAddress.Test, isTestAddress);
            Assert.IsTrue(IsValidXAddress(xAddress));
            // Notice that converting an X-address to a classic address has `result.tag === false` (not undefined)
            Assert.IsNull(myClassicAddress.Tag);
        }
    }
}
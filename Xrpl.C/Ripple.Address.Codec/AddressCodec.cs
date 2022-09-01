using System;
using System.Diagnostics;
using Ripple.Address.Codec.Exceptions;
using System.Text;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-address-codec/src/index.ts

namespace Ripple.Address.Codec
{
    public class AddressCodec
    {

        private static byte[] CopyOfRange(byte[] source, int from_, int to)
        {
            var range = new byte[to - from_];
            Array.Copy(source, from_, range, 0, range.Length);
            return range;
        }

        public class CodecAddress
        {
            public string ClassicAddress { get; set; }
            public int Tag { get; set; }
            public bool Test { get; set; }
        }

        public class CodecAccountID
        {
            public byte[] AccountID { get; set; }
            public int Tag { get; set; }
            public bool Test { get; set; }
        }

        private static uint MAX_32_BIT_UNSIGNED_INT = 4294967295;
        private static byte[] PREFIX_BYTES_MAIN = { 0x05, 0x44 };
        private static byte[] PREFIX_BYTES_TEST = { 0x04, 0x93 };

        private static readonly B58 B58;
        public const string Alphabet = "rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz";
        static AddressCodec()
        {
            B58 = new B58(Alphabet);
        }

        public static string ClassicAddressToXAddress(string classicAddress, int tag, bool isTest)
        {
            byte[] accountId = XrplCodec.DecodeAccountID(accountId: classicAddress);
            return EncodeXAddress(accountId, tag, isTest);
        }

        public static string EncodeXAddress(byte[] accountId, int? tag, bool isTest)
        {
            if (accountId.Length != 20)
            {
                // RIPEMD160 is 160 bits = 20 bytes
                throw new AddressCodecException("Account ID must be 20 bytes");
            }
            if (tag > MAX_32_BIT_UNSIGNED_INT)
            {
                throw new AddressCodecException("Invalid tag");
            }
            int theTag = tag ?? 0;
            int flags = tag == null ? 0 : 1;
            byte[] prefix = isTest ? PREFIX_BYTES_TEST : PREFIX_BYTES_MAIN;
            byte[] postbytes = {
                (byte)flags,
                (byte)(theTag & 0xff),
                (byte)((theTag >> 8) & 0xff),
                (byte)((theTag >> 16) & 0xff),
                (byte)((theTag >> 24) & 0xff),
                0,
                0,
                0,
                0
            };
            byte[] bytes = Utils.Combine(prefix, accountId, postbytes);
            byte[] checkSum = CopyOfRange(HashUtils.DoubleDigest(bytes), 0, 4);
            byte[] fbytes = Utils.Combine(bytes, checkSum);
            return B58.EncodeToString(fbytes);
        }

        public static CodecAddress XAddressToClassicAddress(string xAddress)
        {
            Debug.WriteLine("FUNC: XAddressToClassicAddress");
            CodecAccountID account = DecodeXAddress(xAddress);
            string classicAddress = XrplCodec.EncodeAccountID(account.AccountID);
            return new CodecAddress { ClassicAddress = classicAddress, Tag = account.Tag, Test = account.Test };
        }

        public static CodecAccountID DecodeXAddress(string xAddress)
        {
            Debug.WriteLine("FUNC: DecodeXAddress");
            byte[] decoded = B58.Decode(xAddress);
            bool isTest = IsBufferForTestAddress(decoded);
            byte[] accountId = CopyOfRange(decoded, 2, 22);
            int tag = TagFromBuffer(decoded);
            return new CodecAccountID { AccountID = accountId, Tag = tag, Test = isTest };
        }

        public static bool IsBufferForTestAddress(byte[] buf)
        {
            byte[] decodedPrefix = CopyOfRange(buf, 0, 2);
            if (PREFIX_BYTES_MAIN == decodedPrefix)
            {
                return false;
            }
            if (PREFIX_BYTES_TEST == decodedPrefix)
            {
                return true;
            }
            throw new AddressCodecException("Invalid X-address: bad prefix");
        }

        public static int TagFromBuffer(byte[] buf)
        {
            byte flag = buf[22];
            if (flag >= 2)
            {
                // No support for 64-bit tags at this time
                throw new AddressCodecException("Unsupported X-address");
            }
            if (flag == 1)
            {
                // Little-endian to big-endian
                return buf[23] + buf[24] * 0x100 + buf[25] * 0x10000 + buf[26] * 0x1000000;
            }
            //assert.strictEqual(flag, 0, 'flag must be zero to indicate no tag')
            //assert.ok(
            //  Buffer.from('0000000000000000', 'hex').equals(buf.slice(23, 23 + 8)),
            //'remaining bytes must be zero',
            //)
            return -1;
        }

        public static bool IsValidXAddress(string xAddress)
        {
            try
            {
                DecodeXAddress(xAddress);

            } catch
            {
                return false;
            }
            return true;
        }
    }
}
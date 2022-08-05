using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Hashing;
using System.IO;

namespace Ripple.Binary.Codec.Types
{
    public class StReader
    {
        private readonly BinaryParser _parser;

        public StReader(BinaryParser parser) => _parser = parser;

        public static StReader FromFile(string path)
        {
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return new StReader(new StreamParser(stream));
        }
        public StReader FromHex(string hex) => new StReader(new BufferParser(hex));

        public bool End() => _parser.End();

        public BinaryParser Parser() => _parser;


        public AccountId ReadAccountId() => AccountId.FromParser(_parser);

        public Amount ReadAmount() => Amount.FromParser(_parser);

        public Blob ReadBlob()
        {
            var hint = _parser.ReadVlLength();
            return Blob.FromParser(_parser, hint);
        }

        public Currency ReadCurrency() => Currency.FromParser(_parser);

        public Hash128 ReadHash128() => Hash128.FromParser(_parser);

        public Hash160 ReadHash160() => Hash160.FromParser(_parser);

        public Hash256 ReadHash256() => Hash256.FromParser(_parser);

        public HashPrefix ReadHashPrefix()
        {
            var four = ReadUint32();
            return (HashPrefix)four.Value;
        }

        public int ReadOneInt() => _parser.ReadOneInt();

        public PathSet ReadPathSet() => PathSet.FromParser(_parser);

        public StArray ReadStArray() => StArray.FromParser(_parser);

        public StObject ReadStObject() => StObject.FromParser(_parser);

        public Uint16 ReadUint16() => Uint16.FromParser(_parser);

        public Uint32 ReadUint32() => Uint32.FromParser(_parser);

        public Uint64 ReadUint64() => Uint64.FromParser(_parser);

        public Uint8 ReadUint8() => Uint8.FromParser(_parser);

        public Vector256 ReadVector256() => Vector256.FromParser(_parser);

        public StObject ReadVlStObject() => StObject.FromParser(_parser, _parser.ReadVlLength());

        // Reader methods may be define via use of extension methods
        // eg. see: TransactionResult
    }
}

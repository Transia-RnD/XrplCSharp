using Ripple.Core.Binary;
using Ripple.Core.Hashing;
using System.IO;

namespace Ripple.Core.Types
{
    public class StReader
    {
        private readonly BinaryParser _parser;

        public StReader(BinaryParser parser)
        {
            _parser = parser;
        }

        public static StReader FromFile(string path)
        {
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return new StReader(new StreamParser(stream));
        }
        public StReader FromHex(string hex)
        {
            return new StReader(new BufferParser(hex));
        }

        public bool End()
        {
            return _parser.End();
        }

        public BinaryParser Parser()
        {
            return _parser;
        }


        public AccountId ReadAccountId()
        {
            return AccountId.FromParser(_parser);
        }

        public Amount ReadAmount()
        {
            return Amount.FromParser(_parser);
        }

        public Blob ReadBlob()
        {
            var hint = _parser.ReadVlLength();
            return Blob.FromParser(_parser, hint);
        }

        public Currency ReadCurrency()
        {
            return Currency.FromParser(_parser);
        }

        public Hash128 ReadHash128()
        {
            return Hash128.FromParser(_parser);
        }

        public Hash160 ReadHash160()
        {
            return Hash160.FromParser(_parser);
        }

        public Hash256 ReadHash256()
        {
            return Hash256.FromParser(_parser);
        }

        public HashPrefix ReadHashPrefix()
        {
            var four = ReadUint32();
            return (HashPrefix)four.Value;
        }

        public int ReadOneInt()
        {
            return _parser.ReadOneInt();
        }

        public PathSet ReadPathSet()
        {
            return PathSet.FromParser(_parser);
        }

        public StArray ReadStArray()
        {
            return StArray.FromParser(_parser);
        }

        public StObject ReadStObject()
        {
            return StObject.FromParser(_parser);
        }

        public Uint16 ReadUint16()
        {
            return Uint16.FromParser(_parser);
        }

        public Uint32 ReadUint32()
        {
            return Uint32.FromParser(_parser);
        }

        public Uint64 ReadUint64()
        {
            return Uint64.FromParser(_parser);
        }

        public Uint8 ReadUint8()
        {
            return Uint8.FromParser(_parser);
        }
        public Vector256 ReadVector256()
        {
            return Vector256.FromParser(_parser);
        }
        public StObject ReadVlStObject()
        {
            return StObject.FromParser(_parser, _parser.ReadVlLength());
        }

        // Reader methods may be define via use of extension methods
        // eg. see: TransactionResult
    }
}

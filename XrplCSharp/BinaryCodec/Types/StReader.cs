using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Hashing;

using System.IO;

namespace Xrpl.BinaryCodecLib.Types
{
    public class StReader
    {
        private readonly BinaryParser _parser;
        /// <summary>
        /// create StReader reader from binary parser
        /// </summary>
        /// <param name="parser"></param>
        public StReader(BinaryParser parser)
        {
            _parser = parser;
        }
        /// <summary>
        /// create StReader reader from file
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns></returns>
        public static StReader FromFile(string path)
        {
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return new StReader(new StreamParser(stream));
        }
        /// <summary>
        /// create StReader reader from hex string
        /// </summary>
        /// <param name="hex">hex string</param>
        /// <returns></returns>
        public static StReader FromHex(string hex)
        {
            return new StReader(new BufferParser(hex));
        }
        /// <summary>
        /// check if this is end of parser
        /// </summary>
        /// <returns></returns>
        public bool End()
        {
            return _parser.End();
        }
        /// <summary>
        /// get this binary parser
        /// </summary>
        /// <returns></returns>
        public BinaryParser Parser()
        {
            return _parser;
        }

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.AccountId"/> from this parser </summary>
        public AccountId ReadAccountId() => AccountId.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.Amount"/> from this parser </summary>
        public Amount ReadAmount() => Amount.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.Blob"/> from this parser </summary>
        public Blob ReadBlob()
        {
            var hint = _parser.ReadLengthPrefix();
            return Blob.FromParser(_parser, hint);
        }

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.Currency"/> from this parser </summary>
        public Currency ReadCurrency() => Currency.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.Hash128"/> from this parser </summary>
        public Hash128 ReadHash128() => Hash128.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.Hash160"/> from this parser </summary>
        public Hash160 ReadHash160() => Hash160.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.Hash256"/> from this parser </summary>
        public Hash256 ReadHash256() => Hash256.FromParser(_parser);
        /// <summary>
        /// Read Hash Prefix
        /// </summary>
        /// <returns><see cref="Xrpl.BinaryCodecLib.Hashing.HashPrefix"/> prefix type</returns>
        public HashPrefix ReadHashPrefix()
        {
            var four = ReadUint32();
            return (HashPrefix)four.Value;
        }
        /// <summary>
        /// read one integer
        /// </summary>
        /// <returns></returns>
        public int ReadOneInt() => _parser.ReadOneInt();

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.PathSet"/> from this parser </summary>
        public PathSet ReadPathSet() => PathSet.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.StArray"/> from this parser </summary>
        public StArray ReadStArray() => StArray.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.StObject"/> from this parser </summary>
        public StObject ReadStObject() => StObject.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.Uint16"/> from this parser </summary>
        public Uint16 ReadUint16() => Uint16.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.Uint32"/> from this parser </summary>
        public Uint32 ReadUint32() => Uint32.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.Uint64"/> from this parser </summary>
        public Uint64 ReadUint64() => Uint64.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.Uint8"/> from this parser </summary>
        public Uint8 ReadUint8() => Uint8.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.Vector256"/> from this parser </summary>
        public Vector256 ReadVector256() => Vector256.FromParser(_parser);

        /// <summary> read field as <see cref="Xrpl.BinaryCodecLib.Types.StObject"/> from this parser </summary>
        public StObject ReadVlStObject() => StObject.FromParser(_parser, _parser.ReadLengthPrefix());

        // Reader methods may be define via use of extension methods
        // eg. see: TransactionResult
    }
}

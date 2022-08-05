using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Enums;
using Ripple.Binary.Codec.Hashing;
using Ripple.Binary.Codec.Util;

namespace Ripple.Binary.Codec.Types
{
    public class StObject : ISerializedType
    {
        protected internal readonly SortedDictionary<Field, ISerializedType> Fields;

        public StObject() => Fields = new SortedDictionary<Field, ISerializedType>();

        internal class BuildFrom
        {
            public  FromParser Parser;
            public  FromJson Json;

            public BuildFrom(FromJson json, FromParser parser)
            {
                Parser = parser;
                Json = json;
            }
        }

        static StObject()
        {
            var d2 = new Dictionary<FieldType, BuildFrom>
            {
                [FieldType.StObject] = new(FromJson, FromParser),
                [FieldType.StArray] = new(StArray.FromJson, StArray.FromParser),
                [FieldType.Uint8] = new(Uint8.FromJson, Uint8.FromParser),
                [FieldType.Uint32] = new(Uint32.FromJson, Uint32.FromParser),
                [FieldType.Uint64] = new(Uint64.FromJson, Uint64.FromParser),
                [FieldType.Uint16] = new(Uint16.FromJson, Uint16.FromParser),
                [FieldType.Amount] = new(Amount.FromJson, Amount.FromParser),
                [FieldType.Hash128] = new(Hash128.FromJson, Hash128.FromParser),
                [FieldType.Hash256] = new(Hash256.FromJson, Hash256.FromParser),
                [FieldType.Hash160] = new(Hash160.FromJson, Hash160.FromParser),
                [FieldType.AccountId] = new(AccountId.FromJson, AccountId.FromParser),
                [FieldType.Blob] = new(Blob.FromJson, Blob.FromParser),
                [FieldType.PathSet] = new(PathSet.FromJson, PathSet.FromParser),
                [FieldType.Vector256] = new(Vector256.FromJson, Vector256.FromParser),
            };
            foreach (var field in Field.Values.Where(
                        field => d2.ContainsKey(field.Type)))
            {
                var buildFrom = d2[field.Type];
                field.FromJson = buildFrom.Json;
                field.FromParser= buildFrom.Parser;
            }

            Field.TransactionType.FromJson = TransactionType.Values.FromJson;
            Field.TransactionType.FromParser= TransactionType.Values.FromParser;
            Field.TransactionResult.FromJson = EngineResult.Values.FromJson;
            Field.TransactionResult.FromParser = EngineResult.Values.FromParser;
            Field.LedgerEntryType.FromJson = LedgerEntryType.Values.FromJson;
            Field.LedgerEntryType.FromParser = LedgerEntryType.Values.FromParser;

        }

        public static StObject FromParser(BinaryParser parser, int? hint = null)
        {
            var so = new StObject();

            // hint, is how many bytes to parse
            if (hint != null)
            {
                // end hint
                hint = parser.Pos() + hint;
            }

            while (!parser.End(hint))
            {
                var field = parser.ReadField();
                if (field == Field.ObjectEndMarker)
                {
                    break;
                }
                var sizeHint = field.IsVlEncoded ? parser.ReadVlLength() : (int?) null;
                var st = field.FromParser(parser, sizeHint);
                if (st == null)
                {
                    throw new InvalidOperationException("Parsed " + field + " as null");
                }
                so.Fields[field] = st;
            }
            return so;
        }

        public static StObject FromJson(JToken token) => FromJson(token, false);

        public static StObject FromJson(JToken token, bool strict)
        {
            if (token.Type != JTokenType.Object)
            {
                throw new InvalidJsonException("{token} is not an object");
            }

            var so = new StObject();
            foreach (var (key, j_token) in (JObject) token)
            {
                if (!Field.Values.Has(key))
                {
                    if (strict)
                    {
                        throw new InvalidJsonException($"unknown field {key}");
                    }
                    continue;
                }
                var fieldForType = Field.Values[key];
                ISerializedType st;
                try
                {
                    st = fieldForType.FromJson(j_token);
                }
                catch (Exception e) when (e is InvalidOperationException or FormatException or OverflowException or PrecisionException)
                {
                    throw new InvalidJsonException($"Can't decode `{fieldForType}` " +
                                          $"from `{j_token}`", e);
                }
                so.Fields[fieldForType] = st;
            }
            return so;
        }

        public void ToBytes(IBytesSink to) => ToBytes(to, null);

        public JToken ToJson() => ToJsonObject();

        public JObject ToJsonObject()
        {
            var json = new JObject();
            foreach (var pair in Fields)
            {
                json[pair.Key] = pair.Value.ToJson();
            }
            return json;
        }

        public void ToBytes(IBytesSink to, Func<Field, bool> p)
        {
            var serializer = new BinarySerializer(to);
            foreach (var pair in Fields.Where(pair => pair.Key.IsSerialised &&
                                                    (p == null || p(pair.Key))))
            {
                serializer.Add(pair.Key, pair.Value);
            }
        }

        public static implicit operator StObject(JToken v) => FromJson(v);

        public static StObject FromHex(string s) => FromParser(new BufferParser(s));

        public bool Has(Field field) => Fields.ContainsKey(field);

        public byte[] SigningData()
        {
            var list = new BytesList();
            list.Put(HashPrefix.TxSign.Bytes());
            ToBytes(list, f => f.IsSigningField);
            return list.Bytes();
        }

        public byte[] ToBytes()
        {
            var list = new BytesList();
            ToBytes(list, f => f.IsSerialised);
            return list.Bytes();
        }
        public AccountId this[AccountIdField f]
        {
            get => (AccountId)Fields[f];
            set => Fields[f] = value;
        }
        public Amount this[AmountField f]
        {
            get => (Amount)Fields[f];
            set => Fields[f] = value;
        }
        public Blob this[BlobField f]
        {
            get => (Blob)Fields[f];
            set => Fields[f] = value;
        }
        public Hash128 this[Hash128Field f]
        {
            get => (Hash128)Fields[f];
            set => Fields[f] = value;
        }
        public Hash160 this[Hash160Field f]
        {
            get => (Hash160)Fields[f];
            set => Fields[f] = value;
        }
        public Hash256 this[Hash256Field f]
        {
            get => (Hash256)Fields[f];
            set => Fields[f] = value;
        }
        public PathSet this[PathSetField f]
        {
            get => (PathSet)Fields[f];
            set => Fields[f] = value;
        }
        public StArray this[StArrayField f]
        {
            get => (StArray)Fields[f];
            set => Fields[f] = value;
        }
        public StObject this[StObjectField f]
        {
            get => (StObject)Fields[f];
            set => Fields[f] = value;
        }
        public Uint16 this[Uint16Field f]
        {
            get => (Uint16)Fields[f];
            set => Fields[f] = value;
        }
        public LedgerEntryType this[LedgerEntryTypeField f]
        {
            get => (LedgerEntryType)Fields[f];
            set => Fields[f] = value;
        }
        public TransactionType this[TransactionTypeField f]
        {
            get => (TransactionType)Fields[f];
            set => Fields[f] = value;
        }
        public Uint32 this[Uint32Field f]
        {
            get => (Uint32)Fields[f];
            set => Fields[f] = value;
        }
        public Uint64 this[Uint64Field f]
        {
            get => (Uint64)Fields[f];
            set => Fields[f] = value;
        }
        public Uint8 this[Uint8Field f]
        {
            get => (Uint8)Fields[f];
            set => Fields[f] = value;
        }
        public EngineResult this[EngineResultField f]
        {
            get => (EngineResult)Fields[f];
            set => Fields[f] = value;
        }
        public Vector256 this[Vector256Field f]
        {
            get => (Vector256)Fields[f];
            set => Fields[f] = value;
        }

        public StObject SetFlag(uint flags)
        {
            if (Has(Field.Flags))
            {
                flags |= this[Field.Flags];
            }
            this[Field.Flags] = flags;
            return this;
        }
    }

    internal static class Extensions
    {
        internal static byte[] Bytes(this HashPrefix hp) => Bits.GetBytes((uint)hp);
    }
}
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Ripple.Core.Binary;

namespace Ripple.Core.Types
{
    public class PathHop
    {
        public static byte TypeAccount = 0x01;
        public static byte TypeCurrency = 0x10;
        public static byte TypeIssuer = 0x20;

        public readonly AccountId Account;
        public readonly AccountId Issuer;
        public readonly Currency Currency;
        public readonly int Type;

        public PathHop(AccountId account, AccountId issuer, Currency currency)
        {
            Account = account;
            Issuer = issuer;
            Currency = currency;
            Type = SynthesizeType();
        }

        public static PathHop FromJson(JToken json)
        {
            return new PathHop(json["account"], json["issuer"], json["currency"]);
        }

        public bool HasIssuer()
        {
            return Issuer != null;
        }
        public bool HasCurrency()
        {
            return Currency != null;
        }
        public bool HasAccount()
        {
            return Account != null;
        }

        public int SynthesizeType()
        {
            var type = 0;

            if (HasAccount())
            {
                type |= TypeAccount;
            }
            if (HasCurrency())
            {
                type |= TypeCurrency;
            }
            if (HasIssuer())
            {
                type |= TypeIssuer;
            }
            return type;
        }

        public JObject ToJson()
        {
            var hop = new JObject {["type"] = Type};

            if (HasAccount())
            {
                hop["account"] = Account;
            }
            if (HasCurrency())
            {
                hop["currency"] = Currency;
            }
            if (HasIssuer())
            {
                hop["issuer"] = Issuer;
            }
            return hop;
        }
    }
    public class Path : List<PathHop>
    {
        public Path()
        {
        }

        public Path(IEnumerable<PathHop> enumerable) : base(enumerable)
        {
        }
        public static Path FromJson(JToken json)
        {
            return new Path(json.Select(PathHop.FromJson));
        }
        public JArray ToJson()
        {
            var array = new JArray();
            foreach (var hop in this)
            {
                array.Add(hop.ToJson());
            }
            return array;
        }
    }

    public class PathSet : List<Path>, ISerializedType
    {
        public static byte PathSeparatorByte = 0xFF;
        public static byte PathsetEndByte = 0x00;

        private PathSet()
        {
            
        }

        public PathSet(IEnumerable<Path> collection) : base(collection)
        {
        }

        public void ToBytes(IBytesSink buffer)
        {
            var n = 0;
            foreach (var path in this)
            {
                if (n++ != 0)
                {
                    buffer.Put(PathSeparatorByte);
                }
                foreach (var hop in path)
                {
                    buffer.Put((byte)hop.Type);
                    if (hop.HasAccount())
                    {
                        buffer.Put(hop.Account.Buffer);
                    }
                    if (hop.HasCurrency())
                    {
                        buffer.Put(hop.Currency.Buffer);
                    }
                    if (hop.HasIssuer())
                    {
                        buffer.Put(hop.Issuer.Buffer);
                    }
                }
            }
            buffer.Put(PathsetEndByte);
        }

        public JToken ToJson()
        {
            var array = new JArray();
            foreach (var path in this)
            {
                array.Add(path.ToJson());
            }
            return array;
        }

        public static PathSet FromJson(JToken token)
        {
            return new PathSet(token.Select(Path.FromJson));
        }

        public static PathSet FromParser(BinaryParser parser, int? hint=null)
        {
            var pathSet = new PathSet();
            Path path = null;
            while (!parser.End())
            {
                byte type = parser.ReadOne();
                if (type == PathsetEndByte)
                {
                    break;
                }
                if (path == null)
                {
                    path = new Path();
                    pathSet.Add(path);
                }
                if (type == PathSeparatorByte)
                {
                    path = null;
                    continue;
                }

                AccountId account = null;
                AccountId issuer = null;
                Currency currency = null;

                if ((type & PathHop.TypeAccount) != 0)
                {
                    account = AccountId.FromParser(parser);
                }
                if ((type & PathHop.TypeCurrency) != 0)
                {
                    currency = Currency.FromParser(parser);
                }
                if ((type & PathHop.TypeIssuer) != 0)
                {
                    issuer = AccountId.FromParser(parser);
                }
                var hop = new PathHop(account, issuer, currency);
                path.Add(hop);

            }
            return pathSet;
        }

    }
}
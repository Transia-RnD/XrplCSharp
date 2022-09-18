using Newtonsoft.Json.Linq;

using Ripple.Binary.Codec.Binary;

using System.Collections.Generic;
using System.Linq;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/path-set.ts
//https://xrpl.org/serialization.html#pathset-fields

namespace Ripple.Binary.Codec.Types
{
    /// <summary> The object representation of a Hop, an issuer AccountID, an account AccountID, and a Currency </summary>
    public class PathHop
    {
        #region Constant for masking types of a Hop

        /// <summary> TypeAccount const byte </summary>
        public const byte TypeAccount = 0x01;
        /// <summary> TypeCurrency const byte </summary>
        public const byte TypeCurrency = 0x10;
        /// <summary> type issuer const byte </summary>
        public const byte TypeIssuer = 0x20;

        #endregion
        /// <summary> account AccountID </summary>
        public readonly AccountId Account;
        /// <summary> issuer AccountID </summary>
        public readonly AccountId Issuer;
        /// <summary> Currency </summary>
        public readonly Currency Currency;
        /// <summary> Hop type </summary>
        public readonly int Type;
        /// <summary> Create a Hop </summary>
        /// <param name="account">account AccountID</param>
        /// <param name="issuer">issuer AccountID</param>
        /// <param name="currency">Currency</param>
        public PathHop(AccountId account, AccountId issuer, Currency currency)
        {
            Account = account;
            Issuer = issuer;
            Currency = currency;
            Type = SynthesizeType();
        }
        /// <summary> Deserialize Hot </summary>
        /// <param name="json">json token</param>
        /// <returns></returns>
        public static PathHop FromJson(JToken json)
        {
            return new PathHop(json["account"], json["issuer"], json["currency"]);
        }
        /// <summary> check that hop has issuer AccountID </summary>
        public bool HasIssuer() => Issuer != null;
        /// <summary> check that hop has currency</summary>
        public bool HasCurrency() => Currency != null;
        /// <summary> check that hop has account AccountID </summary>
        public bool HasAccount() => Account != null;
        /// <summary>
        /// generate type for current hop
        /// </summary>
        /// <returns></returns>
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
        /// <summary> Serialize Hop  </summary>
        /// <returns></returns>
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
    /// <summary> Class for serializing/deserializing Paths </summary>
    public class Path : List<PathHop>
    {
        /// <summary> construct a Path </summary>
        public Path()
        {
        }
        /// <summary>
        /// construct a Path from an Enumerable of Hops
        /// </summary>
        /// <param name="enumerable">Path or array of HopObjects to construct a Path</param>
        public Path(IEnumerable<PathHop> enumerable) : base(enumerable)
        {
        }
        /// <summary> Deserialize Path </summary>
        /// <param name="json">json token</param>
        /// <returns></returns>
        public static Path FromJson(JToken json) => new Path(json.Select(PathHop.FromJson));
        /// <summary> Serialize Path  </summary>
        /// <returns></returns>
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
    /// <summary> Deserialize and Serialize the PathSet type </summary>
    public class PathSet : List<Path>, ISerializedType
    {

        #region Constants for separating Paths in a PathSet
        /// <summary>
        /// PathSeparator const
        /// </summary>
        public const byte PathSeparatorByte = 0xFF;
        /// <summary>
        /// PathsetEnd const
        /// </summary>
        public const byte PathsetEndByte = 0x00;

        #endregion
        /// <summary> Construct a PathSet </summary>
        private PathSet()
        {
            
        }
        /// <summary>
        /// Construct a PathSet from an Array of Arrays representing paths
        /// </summary>
        /// <param name="collection">A PathSet or Array of Array of HopObjects</param>
        public PathSet(IEnumerable<Path> collection) : base(collection)
        {
        }

        /// <inheritdoc />
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
        /// <summary>
        /// Get the JSON representation of this PathSet
        /// </summary>
        /// <returns>Array of Array of HopObjects, representing this PathSet</returns>
        public JToken ToJson()
        {
            var array = new JArray();
            foreach (var path in this)
            {
                array.Add(path.ToJson());
            }
            return array;
        }
        /// <summary> Deserialize PathSet </summary>
        /// <param name="token">json token</param>
        /// <returns></returns>
        public static PathSet FromJson(JToken token)
        {
            return new PathSet(token.Select(Path.FromJson));
        }
        /// <summary>
        /// Construct a PathSet from a BinaryParser
        /// </summary>
        /// <param name="parser">A BinaryParser to read PathSet from</param>
        /// <returns></returns>
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
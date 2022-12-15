using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Types;
using static Xrpl.BinaryCodec.Types.Hop;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/path-set.ts

namespace Xrpl.BinaryCodec.Types
{
    public class Hop : SerializedType
    {
        /// <summary>
        /// 
        /// </summary>
        public class HopObject : JsonObject
        {
            /// <summary>
            /// 
            /// </summary>
            public string issuer { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string account { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string currency { get; set; }
        }

        public static readonly byte PATHSET_END_BYTE = 0x00;
        public static readonly byte PATH_SEPARATOR_BYTE = 0xff;

        public static readonly byte TYPE_ACCOUNT = 0x01;
        public static readonly byte TYPE_CURRENCY = 0x10;
        public static readonly byte TYPE_ISSUER = 0x20;

        public static Hop From(Hop hop)
        {
            return hop;
        }

        public static Hop From(HopObject hopObject)
        {
            var bytes = new List<byte>();
            bytes.Add(0);

            if (hopObject.account != null)
            {
                bytes.AddRange(AccountID.From(hopObject.account).ToBytes());
                bytes[0] |= TYPE_ACCOUNT;
            }

            if (hopObject.currency != null)
            {
                bytes.AddRange(Currency.From(hopObject.currency).ToBytes());
                bytes[0] |= TYPE_CURRENCY;
            }

            if (hopObject.issuer != null)
            {
                bytes.AddRange(AccountID.From(hopObject.issuer).ToBytes());
                bytes[0] |= TYPE_ISSUER;
            }

            return new Hop(bytes.ToArray());
        }

        public static Hop FromParser(BinaryParser parser)
        {
            var type = parser.ReadUInt8();
            var bytes = new List<byte>();
            bytes.Add(type);

            if ((type & TYPE_ACCOUNT) != 0)
            {
                bytes.AddRange(parser.Read(AccountID.WIDTH));
            }

            if ((type & TYPE_CURRENCY) != 0)
            {
                bytes.AddRange(parser.Read(Currency.WIDTH));
            }

            if ((type & TYPE_ISSUER) != 0)
            {
                bytes.AddRange(parser.Read(AccountID.WIDTH));
            }

            return new Hop(bytes.ToArray());
        }

        public Hop(byte[] bytes) : base(bytes)
        {
        }

        public HopObject ToJSON()
        {
            var hopParser = new BinaryParser(this.bytes);
            var type = hopParser.ReadUInt8();

            string account = null;
            string currency = null;
            string issuer = null;

            if ((type & TYPE_ACCOUNT) != 0)
            {
                account = (AccountID.FromParser(hopParser) as AccountID).ToJSON();
            }

            if ((type & TYPE_CURRENCY) != 0)
            {
                currency = (Currency.FromParser(hopParser) as Currency).ToJSON();
            }

            if ((type & TYPE_ISSUER) != 0)
            {
                issuer = (AccountID.FromParser(hopParser) as AccountID).ToJSON();
            }

            var result = new HopObject();
            if (account != null)
            {
                result.account = account;
            }

            if (issuer != null)
            {
                result.issuer = issuer;
            }

            if (currency != null)
            {
                result.currency = currency;
            }

            return result;
        }

        public byte Type()
        {
            return this.bytes[0];
        }
    }

    /// <summary>
    /// Class for serializing/deserializing Paths
    /// </summary>
    public class Path : SerializedType
    {
        /// <summary>
        /// construct a Path from an array of Hops
        /// </summary>
        /// <param name="value">Path or array of HopObjects to construct a Path</param>
        /// <returns>the Path</returns>
        public static Path From(object value)
        {
            if (value is Path)
            {
                return (Path)value;
            }

            List<byte[]> bytes = new List<byte[]>();
            foreach (HopObject hop in (List<HopObject>)value)
            {
                bytes.Add(Hop.From(hop).ToBytes());
            }

            return new Path(Utils.Concat(bytes));
        }

        /// <summary>
        /// Read a Path from a BinaryParser
        /// </summary>
        /// <param name="parser">BinaryParser to read Path from</param>
        /// <returns>the Path represented by the bytes read from the BinaryParser</returns>
        public static Path FromParser(BinaryParser parser)
        {
            List<byte[]> bytes = new List<byte[]>();
            while (!parser.End())
            {
                bytes.Add(Hop.FromParser(parser).ToBytes());

                if (parser.Peek() == PATHSET_END_BYTE || parser.Peek() == PATH_SEPARATOR_BYTE)
                {
                    break;
                }
            }
            return new Path(Utils.Concat(bytes));
        }

        /// <summary>
        /// Get the JSON representation of this Path
        /// </summary>
        /// <returns>an Array of HopObject constructed from this.bytes</returns>
        public List<HopObject> ToJSON()
        {
            List<HopObject> json = new List<HopObject>();
            BinaryParser pathParser = new BinaryParser(this.ToString());

            while (!pathParser.End())
            {
                json.Add(Hop.FromParser(pathParser).ToJSON());
            }

            return json;
        }
    }

    /// <summary>
    /// Deserialize and Serialize the PathSet type
    /// </summary>
    public class PathSet : SerializedType
    {
        /// <summary>
        /// Construct a PathSet from an Array of Arrays representing paths
        /// </summary>
        /// <param name="value">A PathSet or Array of Array of HopObjects</param>
        /// <returns>the PathSet constructed from value</returns>
        public static PathSet From(object value)
        {
            if (value is PathSet)
            {
                return (PathSet)value;
            }

            if (IsPathSet(value))
            {
                List<byte[]> bytes = new List<byte[]>();

                foreach (var path in (object[])value)
                {
                    bytes.Add(Path.From(path).ToBytes());
                    bytes.Add(new byte[] { PATH_SEPARATOR_BYTE });
                }

                bytes[bytes.Count - 1] = new byte[] { PATHSET_END_BYTE };

                return new PathSet(bytes.ToArray());
            }

            throw new Exception("Cannot construct PathSet from given value");
        }

        /// <summary>
        /// Construct a PathSet from a BinaryParser
        /// </summary>
        /// <param name="parser">A BinaryParser to read PathSet from</param>
        /// <returns>the PathSet read from parser</returns>
        public static PathSet FromParser(BinaryParser parser)
        {
            List<byte[]> bytes = new List<byte[]>();

            while (!parser.End())
            {
                bytes.Add(Path.FromParser(parser).ToBytes());
                bytes.Add(parser.Read(1));

                if (bytes[bytes.Count - 1][0] == PATHSET_END_BYTE)
                {
                    break;
                }
            }

            return new PathSet(bytes.ToArray());
        }

        /// <summary>
        /// Get the JSON representation of this PathSet
        /// </summary>
        /// <returns>an Array of Array of HopObjects, representing this PathSet</returns>
        public object[] ToJSON()
        {
            List<object[]> json = new List<object[]>();
            BinaryParser pathParser = new BinaryParser(this.ToString());

            while (!pathParser.End())
            {
                json.Add(Path.FromParser(pathParser).ToJSON());
                pathParser.Skip(1);
            }

            return json.ToArray();
        }
    }
}
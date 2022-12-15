using System.Collections.Generic;

namespace Xrpl.BinaryCodec.Enums
{
    public class Bytes
    {
        public Bytes(string name, int ordinal, int ordinalWidth)
        {
            Name = name;
            Ordinal = ordinal;
            OrdinalWidth = ordinalWidth;
            IBytes = new byte[ordinalWidth];
            for (int i = 0; i < ordinalWidth; i++)
            {
                IBytes[ordinalWidth - i - 1] = (byte)(ordinal >> (i * 8) & 0xff);
            }
        }

        public string Name { get; }
        public int Ordinal { get; }
        public int OrdinalWidth { get; }
        public byte[] IBytes { get; }
    }

    public class BytesLookup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nth"></param>
        /// <returns></returns>
        public static byte[] FieldHeader(int type, int nth)
        {
            var header = new List<byte>();
            if (type < 16)
            {
                if (nth < 16)
                {
                    header.Add((byte)((type << 4) | nth));
                }
                else
                {
                    header.Add((byte)(type << 4));
                    header.Add((byte)nth);
                }
            }
            else if (nth < 16)
            {
                header.Add((byte)nth);
                header.Add((byte)type);
            }
            else
            {
                header.Add(0);
                header.Add((byte)type);
                header.Add((byte)nth);
            }
            return header.ToArray();
        }

        public BytesLookup(Dictionary<string, int> types, int ordinalWidth)
        {
            OrdinalWidth = ordinalWidth;
            foreach (var kv in types)
            {
                var k = kv.Key;
                var v = kv.Value;
                var bytes = new Bytes(k, v, ordinalWidth);
                this[k] = bytes;
                this[v.ToString()] = bytes;
            }
        }

        public int OrdinalWidth { get; }

        public Bytes From(object value)
        {
            return value is Bytes bytes ? bytes : this[value.ToString()];
        }

        public Bytes FromParser(object parser)
        {
            return From(parser.ReadUIntN(OrdinalWidth).ToString());
        }

        public Bytes this[string key]
        {
            get => _bytes[key];
            set => _bytes[key] = value;
        }

        private readonly Dictionary<string, Bytes> _bytes = new Dictionary<string, Bytes>();
    }

    public class FieldInstance
    {
        public FieldInstance(string name, int nth, bool isVariableLengthEncoded, bool isSerialized, bool isSigningField, Bytes type, int ordinal, string associatedType)
        {
            Name = name;
            Nth = nth;
            IsVariableLengthEncoded = isVariableLengthEncoded;
            IsSerialized = isSerialized;
            IsSigningField = isSigningField;
            Type = type;
            Ordinal = ordinal;
            AssociatedType = associatedType;
            Header = FieldHeader(type.Ordinal, nth);
        }

        public string Name { get; }
        public int Nth { get; }
        public bool IsVariableLengthEncoded { get; }
        public bool IsSerialized { get; }
        public bool IsSigningField { get; }
        public Bytes Type { get; }
        public int Ordinal { get; }
        public string AssociatedType { get; }
        public byte[] Header { get; }

        private static byte[] FieldHeader(int type, int nth)
        {
            var header = new List<byte>();
            if (type < 16)
            {
                if (nth < 16)
                {
                    header.Add((byte)((type << 4) | nth));
                }
                else
                {
                    header.Add((byte)(type << 4));
                    header.Add((byte)nth);
                }
            }
            else if (nth < 16)
            {
                header.Add((byte)nth);
                header.Add((byte)type);
            }
            else
            {
                header.Add(0);
                header.Add((byte)type);
                header.Add((byte)nth);
            }

            return header.ToArray();
        }
    }

    public class FieldLookup
    {
        public FieldLookup(Dictionary<string, FieldInfo> fields)
        {
            foreach (var kv in fields)
            {
                var k = kv.Key;
                var v = kv.Value;
                var field = BuildField(k, v);
                this[k] = field;
                this[field.Ordinal.ToString()] = field;
            }
        }

        public FieldInstance FromString(string value)
        {
            return this[value];
        }

        public FieldInstance this[string key]
        {
            get => _fields[key];
            set => _fields[key] = value;
        }

        private readonly Dictionary<string, FieldInstance> _fields = new Dictionary<string, FieldInstance>();

        private static FieldInstance BuildField(string name, FieldInfo info)
        {
            var typeOrdinal = Enums.Types[info.Type];
            var field = FieldHeader(typeOrdinal, info.Nth);
            return new FieldInstance(name, info.Nth, info.IsVLEncoded, info.IsSerialized, info.IsSigningField, new Bytes(info.Type, typeOrdinal, 2), (typeOrdinal << 16) | info.Nth, "SerializedType");
        }
    }

    public class FieldInfo
    {
        public int Nth { get; set; }
        public bool IsVLEncoded { get; set; }
        public bool IsSerialized { get; set; }
        public bool IsSigningField { get; set; }
        public string Type { get; set; }
    }

    public static class Enums
    {
        public static readonly Dictionary<string, int> Types = new Dictionary<string, int>
        {
            {"Done", -1},
            {"Unknown", -2},
            {"NotPresent", 0},
            {"UInt16", 1},
            {"UInt32", 2},
            {"UInt64", 3},
            {"Hash128", 4},
            {"Hash256", 5},
            {"Amount", 6},
            {"Blob", 7},
            {"AccountID", 8},
            {"STObject", 14},
            {"STArray", 15},
            {"UInt8", 16},
            {"Hash160", 17},
            {"PathSet", 18},
            {"Vector256", 19},
            {"UInt96", 20},
            {"UInt192", 21},
            {"UInt384", 22},
            {"UInt512", 23},
            {"Transaction", 10001},
            {"LedgerEntry", 10002},
            {"Validation", 10003},
            {"Metadata", 10004}
        };

        public static readonly Dictionary<string, int> LedgerEntryTypes = new Dictionary<string, int>
        {
            {"Invalid", -1},
            {"AccountRoot", 97},
            {"DirectoryNode", 100},
            {"RippleState", 114},
            {"Ticket", 84},
            {"SignerList", 83},
            {"Offer", 111},
            {"LedgerHashes", 104},
            {"Amendments", 102},
            {"FeeSettings", 115},
            {"Escrow", 117},
            {"PayChannel", 120},
            {"Check", 67},
            {"DepositPreauth", 112},
            {"NegativeUNL", 78},
            {"NFTokenPage", 80},
            {"NFTokenOffer", 55},
            {"Any", -3},
            {"Child", -2},
            {"Nickname", 110},
            {"Contract", 99},
            {"GeneratorMap", 103}
        };

        public static readonly Dictionary<string, int> TransactionResults = new Dictionary<string, int>
        {
            {"telLOCAL_ERROR", -399},
            {"telBAD_DOMAIN", -398},
            {"telBAD_PATH_COUNT", -397},
            {"telBAD_PUBLIC_KEY", -396},
            {"telFAILED_PROCESSING", -395},
            {"telINSUF_FEE_P", -394},
            {"telNO_DST_PARTIAL", -393},
            {"telCAN_NOT_QUEUE", -392},
            {"telCAN_NOT_QUEUE_BALANCE", -391},
            {"telCAN_NOT_QUEUE_BLOCKS", -390},
            {"telCAN_NOT_QUEUE_BLOCKED", -389},
            {"telCAN_NOT_QUEUE_FEE", -388},
            {"telCAN_NOT_QUEUE_FULL", -387},
            {"temMALFORMED", -299},
            {"temBAD_AMOUNT", -298},
            {"temBAD_CURRENCY", -297},
            {"temBAD_EXPIRATION", -296},
            {"temBAD_FEE", -295},
            {"temBAD_ISSUER", -294},
            {"temBAD_LIMIT", -293},
            {"temBAD_OFFER", -292},
            {"temBAD_PATH", -291},
            {"temBAD_PATH_LOOP", -290},
            {"temBAD_REGKEY", -289},
            {"temBAD_SEND_XRP_LIMIT", -288},
            {"temBAD_SEND_XRP_MAX", -287},
            {"temBAD_SEND_XRP_NO_DIRECT", -286},
            {"temBAD_SEND_XRP_PARTIAL", -285},
            {"temBAD_SEND_XRP_PATHS", -284},
            {"temBAD_SEQUENCE", -283},
            {"temBAD_SIGNATURE", -282},
            {"temBAD_SRC_ACCOUNT", -281},
            {"temBAD_TRANSFER_RATE", -280},
            {"temDST_IS_SRC", -279},
            {"temDST_NEEDED", -278},
            {"temINVALID", -277},
            {"temINVALID_FLAG", -276},
            {"temREDUNDANT", -275},
            {"temRIPPLE_EMPTY", -274},
            {"temDISABLED", -273},
            {"temBAD_SIGNER", -272},
            {"temBAD_QUORUM", -271},
            {"temBAD_WEIGHT", -270},
            {"temBAD_TICK_SIZE", -269},
            {"temINVALID_ACCOUNT_ID", -268},
            {"temCANNOT_PREAUTH_SELF", -267},
            {"temINVALID_COUNT", -266},
            {"temUNCERTAIN", -265},
            {"temUNKNOWN", -264},
            {"temSEQ_AND_TICKET", -263},
            {"temBAD_NFTOKEN_TRANSFER_FEE", -262},
            {"tefFAILURE", -199},
            {"tefALREADY", -198},
            {"tefBAD_ADD_AUTH", -197},
            {"tefBAD_AUTH", -196},
            {"tefBAD_LEDGER", -195},
            {"tefCREATED", -194},
            {"tefEXCEPTION", -193},
            {"tefINTERNAL", -192},
            {"tefNO_AUTH_REQUIRED", -191},
            {"tefPAST_SEQ", -190},
            {"tefWRONG_PRIOR", -189},
            {"tefMASTER_DISABLED", -188},
            {"tefMAX_LEDGER", -187},
            {"tefBAD_SIGNATURE", -186},
            {"tefBAD_QUORUM", -185},
            {"tefNOT_MULTI_SIGNING", -184},
            {"tefBAD_AUTH_MASTER", -183},
            {"tefINVARIANT_FAILED", -182},
            {"tefTOO_BIG", -181},
            {"tefNO_TICKET", -180},
            {"tefNFTOKEN_IS_NOT_TRANSFERABLE", -179},
            {"terRETRY", -99},
            {"terFUNDS_SPENT", -98},
            {"terINSUF_FEE_B", -97},
            {"terNO_ACCOUNT", -96},
            {"terNO_AUTH", -95},
            {"terNO_LINE", -94},
            {"terOWNERS", -93},
            {"terPRE_SEQ", -92},
            {"terLAST", -91},
            {"terNO_RIPPLE", -90},
            {"terQUEUED", -89},
            {"terPRE_TICKET", -88},
            {"tesSUCCESS", 0},
            {"tecCLAIM", 100},
            {"tecPATH_PARTIAL", 101},
            {"tecUNFUNDED_ADD", 102},
            {"tecUNFUNDED_OFFER", 103},
            {"tecUNFUNDED_PAYMENT", 104},
            {"tecFAILED_PROCESSING", 105},
            {"tecDIR_FULL", 121},
            {"tecINSUF_RESERVE_LINE", 122},
            {"tecINSUF_RESERVE_OFFER", 123},
            {"tecNO_DST", 124},
            {"tecNO_DST_INSUF_XRP", 125},
            {"tecNO_LINE_INSUF_RESERVE", 126},
            {"tecNO_LINE_REDUNDANT", 127},
            {"tecPATH_DRY", 128},
            {"tecUNFUNDED", 129},
            {"tecNO_ALTERNATIVE_KEY", 130},
            {"tecNO_REGULAR_KEY", 131},
            {"tecOWNERS", 132},
            {"tecNO_ISSUER", 133},
            {"tecNO_AUTH", 134},
            {"tecNO_LINE", 135},
            {"tecINSUFF_FEE", 136},
            {"tecFROZEN", 137},
            {"tecNO_TARGET", 138},
            {"tecNO_PERMISSION", 139},
            {"tecNO_ENTRY", 140},
            {"tecINSUFFICIENT_RESERVE", 141},
            {"tecNEED_MASTER_KEY", 142},
            {"tecDST_TAG_NEEDED", 143},
            {"tecINTERNAL", 144},
            {"tecOVERSIZE", 145},
            {"tecCRYPTOCONDITION_ERROR", 146},
            {"tecINVARIANT_FAILED", 147},
            {"tecEXPIRED", 148},
            {"tecDUPLICATE", 149},
            {"tecKILLED", 150},
            {"tecHAS_OBLIGATIONS", 151},
            {"tecTOO_SOON", 152},
            {"tecMAX_SEQUENCE_REACHED", 154},
            {"tecNO_SUITABLE_NFTOKEN_PAGE", 155},
            {"tecNFTOKEN_BUY_SELL_MISMATCH", 156},
            {"tecNFTOKEN_OFFER_TYPE_MISMATCH", 157},
            {"tecCANT_ACCEPT_OWN_NFTOKEN_OFFER", 158},
            {"tecINSUFFICIENT_FUNDS", 159},
            {"tecOBJECT_NOT_FOUND", 160},
            {"tecINSUFFICIENT_PAYMENT", 161}
        };

        public static readonly Dictionary<string, int> TransactionTypes = new Dictionary<string, int>
        {
            {"Invalid", -1},
            {"Payment", 0},
            {"EscrowCreate", 1},
            {"EscrowFinish", 2},
            {"AccountSet", 3},
            {"EscrowCancel", 4},
            {"SetRegularKey", 5},
            {"NickNameSet", 6},
            {"OfferCreate", 7},
            {"OfferCancel", 8},
            {"Contract", 9},
            {"TicketCreate", 10},
            {"TicketCancel", 11},
            {"SignerListSet", 12},
            {"PaymentChannelCreate", 13},
            {"PaymentChannelFund", 14},
            {"PaymentChannelClaim", 15},
            {"CheckCreate", 16},
            {"CheckCash", 17},
            {"CheckCancel", 18},
            {"DepositPreauth", 19},
            {"TrustSet", 20},
            {"AccountDelete", 21},
            {"SetHook", 22},
            {"NFTokenMint", 25},
            {"NFTokenBurn", 26},
            {"NFTokenCreateOffer", 27},
            {"NFTokenCancelOffer", 28},
            {"NFTokenAcceptOffer", 29},
            {"EnableAmendment", 100},
            {"SetFee", 101},
            {"UNLModify", 102}
        };
    }
}
using Newtonsoft.Json.Linq;
using Ripple.Core.Enums;

namespace Ripple.Core.Types
{
    public class LedgerEntryType : SerializedEnumItem<ushort>
    {
        public class Enumeration : SerializedEnumeration<LedgerEntryType, ushort>{}
        public static Enumeration Values = new Enumeration();
        private LedgerEntryType(string name, int ordinal) : base(name, ordinal){}
        private static LedgerEntryType Add(string reference, int ordinal)
        {
            return Values.AddEnum(new LedgerEntryType(reference, ordinal));
        }
        public static readonly LedgerEntryType Invalid = Add(nameof(Invalid), -1);
        public static readonly LedgerEntryType AccountRoot = Add(nameof(AccountRoot), 'a');
        public static readonly LedgerEntryType DirectoryNode = Add(nameof(DirectoryNode), 'd');
        public static readonly LedgerEntryType GeneratorMap = Add(nameof(GeneratorMap), 'g');
        public static readonly LedgerEntryType RippleState = Add(nameof(RippleState), 'r');
        public static readonly LedgerEntryType Offer = Add(nameof(Offer), 'o');
        public static readonly LedgerEntryType Contract = Add(nameof(Contract), 'c');
        public static readonly LedgerEntryType LedgerHashes = Add(nameof(LedgerHashes), 'h');
        public static readonly LedgerEntryType EnabledAmendments = Add(nameof(EnabledAmendments), 'f');
        public static readonly LedgerEntryType FeeSettings = Add(nameof(FeeSettings), 's');
        public static readonly LedgerEntryType Ticket = Add(nameof(Ticket), 'T');
        public static readonly LedgerEntryType SignerList = Add(nameof(SignerList), 'S');

        public static LedgerEntryType FromJson(JToken jToken)
        {
            return Values.FromJson(jToken);
        }
    }
}

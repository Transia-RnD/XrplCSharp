namespace Ripple.Core.Enums
{
    public class Uint16Field : Field {
        public Uint16Field(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.Uint16,
                    isSigningField, isSerialised) {}
    }

    public class TransactionTypeField : Uint16Field
    {
        public TransactionTypeField(string name, int nthOfType) :
                base(name, nthOfType)
        { }
    }
    public class LedgerEntryTypeField : Uint16Field
    {
        public LedgerEntryTypeField(string name, int nthOfType) :
                base(name, nthOfType)
        { }
    }
}

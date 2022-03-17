namespace Ripple.Binary.Codec.Enums
{
    public class AmountField : Field {
        public AmountField(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.Amount,
                    isSigningField, isSerialised) {}
    }
}

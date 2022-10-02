//https://xrpl.org/serialization.html#amount-fields
namespace Xrpl.BinaryCodec.Enums
{
    public class AmountField : Field {
        public AmountField(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.Amount,
                    isSigningField, isSerialised) {}
    }
}

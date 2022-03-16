namespace Ripple.Core.Enums
{
    public class Uint32Field : Field {
        public Uint32Field(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.Uint32,
                    isSigningField, isSerialised) {}
    }
}

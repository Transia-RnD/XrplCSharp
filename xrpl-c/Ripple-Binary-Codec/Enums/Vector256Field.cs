namespace Ripple.Core.Enums
{
    public class Vector256Field : Field {
        public Vector256Field(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.Vector256,
                    isSigningField, isSerialised) {}
    }
}

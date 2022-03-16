namespace Ripple.Core.Enums
{
    public class StArrayField : Field {
        public StArrayField(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.StArray,
                    isSigningField, isSerialised) {}
    }
}

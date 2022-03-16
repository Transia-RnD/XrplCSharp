namespace Ripple.Core.Enums
{
    public class PathSetField : Field {
        public PathSetField(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.PathSet,
                    isSigningField, isSerialised) {}
    }
}

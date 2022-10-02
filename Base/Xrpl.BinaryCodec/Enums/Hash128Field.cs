//https://xrpl.org/serialization.html#hash-fields
namespace Xrpl.BinaryCodecLib.Enums
{
    public class Hash128Field : Field {
        public Hash128Field(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.Hash128,
                    isSigningField, isSerialised) {}
    }
}

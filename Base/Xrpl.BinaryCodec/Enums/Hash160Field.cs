//https://xrpl.org/serialization.html#hash-fields
namespace Xrpl.BinaryCodec.Enums
{
    public class Hash160Field : Field {
        public Hash160Field(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.Hash160,
                    isSigningField, isSerialised) {}
    }
}

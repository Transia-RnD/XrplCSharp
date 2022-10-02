//https://xrpl.org/serialization.html#blob-fields
namespace Xrpl.BinaryCodec.Enums
{
    public class BlobField : Field {
        public BlobField(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.Blob,
                    isSigningField, isSerialised) {}
    }
}

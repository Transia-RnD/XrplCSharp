//https://xrpl.org/serialization.html#uint-fields
namespace Xrpl.BinaryCodec.Enums
{
    public class IssueField : Field
    {
        public IssueField(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.Uint8,
                    isSigningField, isSerialised)
        { }
    }
}

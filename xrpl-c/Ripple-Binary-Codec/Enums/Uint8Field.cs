namespace Ripple.Core.Enums
{
    public class Uint8Field : Field {
        public Uint8Field(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.Uint8,
                    isSigningField, isSerialised) {}
    }

    public class EngineResultField : Uint8Field
    {
        public EngineResultField(string name, int nthOfType) :
                base(name, nthOfType)
        { }
    }

}

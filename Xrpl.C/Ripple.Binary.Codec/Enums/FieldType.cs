namespace Ripple.Binary.Codec.Enums
{
    public class FieldType : EnumItem
    {
        public static Enumeration<FieldType> Values = new(); 
        public FieldType(string name, int ordinal) : base(name, ordinal)
        {
            Values.AddEnum(this);
        }
        public static readonly FieldType Unknown = new(nameof(Unknown), -2);
        public static readonly FieldType Done = new(nameof(Done), -1);
        public static readonly FieldType NotPresent = new(nameof(NotPresent), 0);
        public static readonly FieldType Uint16 = new(nameof(Uint16), 1);
        public static readonly FieldType Uint32 = new(nameof(Uint32), 2);
        public static readonly FieldType Uint64 = new(nameof(Uint64), 3);
        public static readonly FieldType Hash128 = new(nameof(Hash128), 4);
        public static readonly FieldType Hash256 = new(nameof(Hash256), 5);
        public static readonly FieldType Amount = new(nameof(Amount), 6);
        public static readonly FieldType Blob = new(nameof(Blob), 7);
        public static readonly FieldType AccountId = new(nameof(AccountId), 8);
        public static readonly FieldType StObject = new(nameof(StObject), 14);
        public static readonly FieldType StArray = new(nameof(StArray), 15);
        public static readonly FieldType Uint8 = new(nameof(Uint8), 16);
        public static readonly FieldType Hash160 = new(nameof(Hash160), 17);
        public static readonly FieldType PathSet = new(nameof(PathSet), 18);
        public static readonly FieldType Vector256 = new(nameof(Vector256), 19);
        public static readonly FieldType Transaction = new(nameof(Transaction), 10001);
        public static readonly FieldType LedgerEntry = new(nameof(LedgerEntry), 10002);
        public static readonly FieldType Validation = new(nameof(Validation), 10003);


        /*

        public static readonly FieldType Uint8 = new FieldType(nameof(Uint8), 16);
          public static readonly FieldType Uint16 = new FieldType(nameof(Uint16), 1);
        public static readonly FieldType Uint32 = new FieldType(nameof(Uint32), 2);
        public static readonly FieldType Uint64 = new FieldType(nameof(Uint64), 3);

        public static readonly FieldType Hash128 = new FieldType(nameof(Hash128), 4);
        public static readonly FieldType Hash256 = new FieldType(nameof(Hash256), 5);
        public static readonly FieldType Hash160 = new FieldType(nameof(Hash160), 17);
            public static readonly FieldType AccountId = new FieldType(nameof(AccountId), 8);
        
        public static readonly FieldType Vector256 = new FieldType(nameof(Vector256), 19);
        public static readonly FieldType Blob = new FieldType(nameof(Blob), 7);
        
        public static readonly FieldType Amount = new FieldType(nameof(Amount), 6);
        public static readonly FieldType PathSet = new FieldType(nameof(PathSet), 18);
        
        public static readonly FieldType StObject = new FieldType(nameof(StObject), 14);
        public static readonly FieldType StArray = new FieldType(nameof(StArray), 15);

        */
    }
}
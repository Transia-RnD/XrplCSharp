﻿// https://xrpl.org/serialization.html#accountid-fields
namespace Xrpl.BinaryCodec.Enums
{
    public class AccountIdField : Field {
        public AccountIdField(string name, int nthOfType,
            bool isSigningField = true, bool isSerialised = true) :
                base(name, nthOfType, FieldType.AccountId,
                    isSigningField, isSerialised) {}
    }
}

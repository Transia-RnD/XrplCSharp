using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using Xrpl.BinaryCodec;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Hashing;
using Xrpl.BinaryCodec.Types;

// https://github.com/XRPLF/xrpl-py/blob/master/xrpl/core/binarycodec/definitions/field_instance.py

namespace Xrpl.BinaryCodec.Definitions
{
    public class FieldInstance
    {
        public int _nth;
        public bool _isVLEncoded;
        public bool _isSerialized;
        public bool _isSigningField;
        public string _type;
        public string _name;
        public FieldHeader _header;
        public int _ordinal;
        public ISerializedType _associatedType;

        //public ISerializedType GetTypeByName(string name)
        //{
        //    var typeMap: [string: SerializedIType] = [:]
        //    for (name, objectType) in binaryTypes { typeMap[name] = objectType.self }
        //    return typeMap[name];
        //}

        public FieldInstance(
            FieldInfo fieldInfo,
            string fieldName,
            FieldHeader fieldHeader
        )
        {
            _nth = fieldInfo._nth;
            _isVLEncoded = fieldInfo._isVLEncoded;
            _isSerialized = fieldInfo._isSerialized;
            _isSigningField = fieldInfo._isSigningField;
            _type = fieldInfo._type;
            _name = fieldName;
            _header = fieldHeader;
            _ordinal = _header.typeCode << 16 | _nth;
            //_associatedType = GetTypeByName(name: this._type);
        }
    }
}
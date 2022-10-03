using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Hashing;
using Xrpl.BinaryCodec.Types;

// https://github.com/XRPLF/xrpl-py/blob/master/xrpl/core/binarycodec/definitions/field_info.py

namespace Xrpl.BinaryCodec.Definitions
{
    public class FieldInfo
    {
        public int _nth;
        public bool _isVLEncoded;
        public bool _isSerialized;
        public bool _isSigningField;
        public string _type;

        public FieldInfo(Dictionary<string, dynamic> dict)
        {
            _nth = (int)dict["nth"];
            _isVLEncoded = (bool)dict["isVLEncoded"];
            _isSerialized = (bool)dict["isSerialized"];
            _isSigningField = (bool)dict["isSigningField"];
            _type = (string)dict["type"];
        }

        public FieldInfo(
            int nth,
            bool isVLEncoded,
            bool isSerialized,
            bool isSigningField,
            string type
        )
        {
            _nth = nth;
            _isVLEncoded = isVLEncoded;
            _isSerialized = isSerialized;
            _isSigningField = isSigningField;
            _type = type;
        }
    }
}

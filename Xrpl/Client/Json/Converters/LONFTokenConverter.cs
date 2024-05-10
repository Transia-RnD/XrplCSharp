using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;

namespace Xrpl.Client.Json.Converters;

/// <summary>
/// <see cref="BaseLedgerEntry"/> json converter
/// </summary>
public class LONFTokenConverter : JsonConverter
{

    /// <summary>
    /// write <see cref="NFToken"/>  to json object
    /// </summary>
    /// <param name="writer">writer</param>
    /// <param name="value"> <see cref="NFToken"/>  value</param>
    /// <param name="serializer">json serializer</param>
    /// <exception cref="NotSupportedException">Can't create ledger type</exception>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }


    /// <summary> read <see cref="BaseLedgerEntry"/>  from json object </summary>
    /// <param name="reader">json reader</param>
    /// <param name="objectType">object type</param>
    /// <param name="existingValue">object value</param>
    /// <param name="serializer">json serializer</param>
    /// <returns><see cref="NFToken"/> </returns>
    /// <exception cref="NotSupportedException">Cannot convert value</exception>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jObject = JObject.Load(reader);
        var value = jObject.GetValue("NFToken");
        var target = new NFToken();
        serializer.Populate(value.CreateReader(), target);

        return target;
    }

    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException();
    }

    public override bool CanWrite => false;
}
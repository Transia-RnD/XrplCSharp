using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Xrpl.Client.Json.Converters
{
    /// <summary> Ripple datetime converter </summary>
    public class RippleDateTimeConverter : DateTimeConverterBase
    {
        /// <summary> ripple start time </summary>
        private static DateTime RippleStartTime = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// write  <see cref="DateTime"/>  to json object
        /// </summary>
        /// <param name="writer">writer</param>
        /// <param name="value"> <see cref="DateTime"/> value</param>
        /// <param name="serializer">json serializer</param>
        /// <exception cref="NotSupportedException">value  provided is not a DateTime</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime date_time)
            {
                long totalSeconds = (long)(date_time - RippleStartTime).TotalSeconds;
                writer.WriteValue(totalSeconds);                
            }
            else
            {
                throw new ArgumentException("value  provided is not a DateTime", "value");
            }
        }

        /// <summary> read  <see cref="DateTime"/>  from json object </summary>
        /// <param name="reader">json reader</param>
        /// <param name="objectType">object type</param>
        /// <param name="existingValue">object value</param>
        /// <param name="serializer">json serializer</param>
        /// <returns><see cref="DateTime"/></returns>
        /// <exception cref="Exception">Invalid double value. or Invalid token. Expected string</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null: return null;
                case JsonToken.String or JsonToken.Integer:
                {
                    double totalSeconds;

                    try
                    {
                        totalSeconds = Convert.ToDouble(reader.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        throw new Exception("Invalid double value.");
                    }

                    return RippleStartTime.AddSeconds(totalSeconds);
                }
                default: throw new Exception("Invalid token. Expected string");
            }
        }
    }
}

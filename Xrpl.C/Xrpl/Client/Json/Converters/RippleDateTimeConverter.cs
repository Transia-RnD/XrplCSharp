using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Xrpl.Client.Json.Converters
{
    public class RippleDateTimeConverter : DateTimeConverterBase
    {
        private static DateTime RippleStartTime = new(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime date_time)
            {
                var totalSeconds = (long)(date_time - RippleStartTime).TotalSeconds;
                writer.WriteValue(totalSeconds);                
            }
            else
            {
                throw new ArgumentException("value  provided is not a DateTime", nameof(value));
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null: return null;
                case JsonToken.String:
                case JsonToken.Integer:
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

using Newtonsoft.Json;
using System;

public class CustomDateTimeConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.Value == null)
            return null;

        if (reader.ValueType == typeof(DateTime))
            return reader.Value;

        string dateString = reader.Value.ToString();
        DateTime resultDateTime;
        if (DateTime.TryParse(dateString, out resultDateTime))
        {
            return resultDateTime;
        }
        else
        {
            // If parsing fails, handle accordingly
            throw new JsonSerializationException("Invalid DateTime format");
        }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        DateTime dateTimeValue = (DateTime)value;
        writer.WriteValue(dateTimeValue.ToString("yyyyMMddHHmmss"));
    }
}

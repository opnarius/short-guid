using System;
using Newtonsoft.Json;

namespace ShortGuid
{
    public class ShortGuidJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(global::ShortGuid.ShortGuid);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value?.ToString(), typeof(string));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            global::ShortGuid.ShortGuid value;
            return global::ShortGuid.ShortGuid.TryParse(serializer.Deserialize<string>(reader), out value)
                ? value
                : existingValue;
        }
    }
}
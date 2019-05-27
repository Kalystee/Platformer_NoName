using Newtonsoft.Json;
using System;

public class AbilitySerializer : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        AbilityBase tile = value as AbilityBase;
        writer.WriteValue(tile.Name);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            string s = reader.Value.ToString();
            return AbilityDefOf.GetAbilityBaseNamed(s);
        }
        return null;
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(AbilityBase).IsAssignableFrom(objectType);
    }
}

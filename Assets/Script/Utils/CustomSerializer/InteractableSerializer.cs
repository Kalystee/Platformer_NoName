using Newtonsoft.Json;
using System;

public class InteractableSerializer : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        InteractableBase tile = value as InteractableBase;
        writer.WriteValue(tile.Name);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            string s = reader.Value.ToString();
            return InteractableDefOf.GetInteractableBaseNamed(s);
        }
        return null;
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(InteractableBase).IsAssignableFrom(objectType);
    }
}

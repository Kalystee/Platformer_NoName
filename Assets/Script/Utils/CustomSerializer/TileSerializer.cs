using Newtonsoft.Json;
using System;

public class TileSerializer : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        TileBase tile = value as TileBase;
        writer.WriteValue(tile.Name);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            string s = reader.Value.ToString();
            return TileDefOf.GetTileBaseNamed(s);
        }
        return null;
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(TileBase).IsAssignableFrom(objectType);
    }
}

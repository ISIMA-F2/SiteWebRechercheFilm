using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace FilmFront.Components.SharedModels
{
    public class OmdbFilmDetail
    {
        [JsonPropertyName("Title")]
        public string Title { get; set; } = "";
        [JsonConverter(typeof(StringOrIntConverter))]
        public string Year { get; set; }
        [JsonPropertyName("ImdbID")]
        public string ImdbID { get; set; } = "";
        [JsonPropertyName("Poster")]
        public string Poster { get; set; } = "";
    }
}

public class StringOrIntConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32().ToString();
        }
        throw new JsonException($"Unexpected token {reader.TokenType} when parsing string.");
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
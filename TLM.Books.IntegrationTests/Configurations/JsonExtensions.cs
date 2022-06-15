using System.Text.Json;
using System.Text.Json.Serialization;

namespace TLM.Books.IntegrationTests.Configurations;

public static class JsonExtensions
{
    public static JsonSerializerOptions SerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            IgnoreNullValues = true
        };

        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        return options;
    }
}
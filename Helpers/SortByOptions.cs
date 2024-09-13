using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[JsonConverter(typeof(StringEnumConverter))]
public enum SortByOptions
{
    Name,
    Genre,
    ReleaseYear
}

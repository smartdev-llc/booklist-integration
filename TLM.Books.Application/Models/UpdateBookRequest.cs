using System.Text.Json.Serialization;

namespace TLM.Books.Application.Models;

public class UpdateBookRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Author { get; set; }
    public string? ISBN { get; set; }
}
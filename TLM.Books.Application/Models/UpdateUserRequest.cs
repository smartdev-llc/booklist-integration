using System.Text.Json.Serialization;

namespace TLM.Books.Application.Models;

public class UpdateUserRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
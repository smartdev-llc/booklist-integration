using System.Text.Json.Serialization;

namespace TLM.Books.Application.Models;

public class AddBookToUserRequest
{
    [JsonIgnore] public Guid UserId { get; set; }
    public List<Guid> BookIds { get; set; }
}
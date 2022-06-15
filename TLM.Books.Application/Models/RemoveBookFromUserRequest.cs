using System.Text.Json.Serialization;

namespace TLM.Books.Application.Models;

public class RemoveBookFromUserRequest
{
    [JsonIgnore] public Guid UserId { get; set; }
    public List<Guid> BookIds { get; set; }
}
namespace TLM.Books.Application.Models;

public class CreateBookResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
}
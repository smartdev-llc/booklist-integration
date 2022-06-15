namespace TLM.Books.Application.Models;

public class BookView
{
    public string Name { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public Guid Id { get; set; }
}
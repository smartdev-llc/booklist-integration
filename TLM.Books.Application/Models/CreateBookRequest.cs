namespace TLM.Books.Application.Models;

public class CreateBookRequest
{
    public string Name { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
}
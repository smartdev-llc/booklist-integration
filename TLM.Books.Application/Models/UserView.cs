namespace TLM.Books.Application.Models;

public class UserView
{
    public string Name { get; set; }
    public Guid Id { get; set; }

    public IEnumerable<BookView> BookViews { get; set; }
}
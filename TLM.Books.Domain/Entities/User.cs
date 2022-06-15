using System.ComponentModel.DataAnnotations;

namespace TLM.Books.Domain.Entities;

public class User : BaseEntity
{
    [Required] public string Name { get; set; }
    public ICollection<Book> Books { get; set; }
}
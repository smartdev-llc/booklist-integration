using System.ComponentModel.DataAnnotations;

namespace TLM.Books.Domain.Entities;

public class Book : BaseEntity
{
    [Required]
    public string Name { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    
    public ICollection<User>? Users { get; set; }
}
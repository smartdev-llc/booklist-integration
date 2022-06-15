using Microsoft.EntityFrameworkCore;
using TLM.Books.Domain.Entities;

namespace TLM.Books.Application.Interfaces;

public interface IBookDbContext
{
    DbSet<Book> Books { get; set; }
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync();
}
using Microsoft.EntityFrameworkCore;
using TLM.Books.Application.Interfaces;
using TLM.Books.Domain.Entities;

namespace TLM.Books._Infrastructure;

public class BookDbContext: DbContext, IBookDbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.Entity<User>()
        //     .HasMany(c => c.Books)
        //     ;
        modelBuilder.Entity<Book>()
            .HasMany(c => c.Users)
            ;
        // modelBuilder.Entity<User>()
        //     
        //     .HasOptional(m => m.SourceLocation) //Optional or Required
        //     .WithMany(u => u.);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}
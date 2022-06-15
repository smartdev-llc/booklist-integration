using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TLM.Books._Infrastructure;
using TLM.Books.Domain.Entities;
using Xunit;

namespace TLM.Books.IntegrationTests.Configurations;

public abstract class BaseWebApplicationFactory : WebApplicationFactory<Program>
{
    public IEnumerable<User> Users { get; protected set; }
    
    public IEnumerable<Book> Books { get; protected set; }
    public abstract string TestDataFolderPath { get; }
    
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<BookDbContext>));
            services.AddDbContext<BookDbContext>(options =>
                options.UseInMemoryDatabase("Testing", root));
            
            using (var serviceProvider = services.BuildServiceProvider())
            using (var scope = serviceProvider.CreateScope())
            using (var dbContext = scope.ServiceProvider.GetRequiredService<BookDbContext>())
            {
                InitializeDatabase(dbContext);
                SeedData(dbContext);
                dbContext.SaveChanges();
            }
        });

        return base.CreateHost(builder);
    }
    
    protected virtual void InitializeDatabase(BookDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    protected virtual void SeedData(BookDbContext context)
    {
        
    }
    
    
}
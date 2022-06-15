using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TLM.Books.Application.Interfaces;

namespace TLM.Books._Infrastructure;

public static class InfraSetup
{
    public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("BookDb"),
                b => b.MigrationsAssembly(typeof(BookDbContext).Assembly.FullName)));

        services.AddScoped<IBookDbContext>(provider => provider.GetService<BookDbContext>());
    }
}
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace TLM.Books.Application;


public static class AppSetup
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}
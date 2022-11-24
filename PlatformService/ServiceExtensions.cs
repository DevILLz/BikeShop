using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds all Services which required for PlatformService work
    /// </summary>
    public static IServiceCollection AddPlatformServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"))
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddScopes()                
                .AddControllers();

        services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

        return services;
    }

    /// <summary>
    /// Adds all scopes which required for PlatformService work
    /// </summary>
    public static IServiceCollection AddScopes(this IServiceCollection services)
    {
        services.AddScoped<IPlatformRepository, PlatformRepository>();

        return services;
    }
}
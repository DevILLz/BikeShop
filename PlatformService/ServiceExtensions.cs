using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds all Services which required for PlatformService work
    /// </summary>
    public static IServiceCollection AddPlatformServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            Console.WriteLine("--> Dev mode with INMem DB");
            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
        }
        else
        {
            Console.WriteLine("--> MSSQL DB");
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("PlatformsCon")));
        }

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
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
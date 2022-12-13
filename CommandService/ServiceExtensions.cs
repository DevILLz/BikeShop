using CommandService.AsyncDataServices;
using CommandService.Data;
using CommandService.EventProcessing;
using CommandService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds all Services which required for PlatformService work
    /// </summary>
    public static IServiceCollection AddPlatformServices(this IServiceCollection services)
    {
        services
                .AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"))
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddScopes()
                .AddControllers();

        return services;
    }

    /// <summary>
    /// Adds all scopes which required for PlatformService work
    /// </summary>
    public static IServiceCollection AddScopes(this IServiceCollection services)
    {
        services.AddScoped<ICommandsRepository, CommandsRepository>();
        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddSingleton<IPlatformDataClient, PlatformDataClient>();

        services.AddHostedService<MessageBussSubscriber>();

        return services;
    }
}
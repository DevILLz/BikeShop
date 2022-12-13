using CommandService;
using CommandService.Data;
using CommandService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

public static class WebAppExtensions
{
    /// <summary>
    /// Adds all Services which required for PlatformService work
    /// </summary>
    public static async Task RunPlatform(this WebApplication app)
    {
        await app.SetUpDb();
        app.UseMiddleware<ExceptionMiddleware>()
           .UseRouting()
           .UseEndpoints(endpoints =>
           {
               endpoints.MapControllers();
           });

        app.Run();
    }

    /// <summary>
    /// Adds all scopes which required for PlatformService work
    /// </summary>
    public static async Task<IApplicationBuilder> SetUpDb(this IApplicationBuilder app)
    {
        using (var services = app.ApplicationServices.CreateScope())
        {
            var grpcClient = services.ServiceProvider.GetService<IPlatformDataClient>();

            var platforms = grpcClient.ReturnAllPlatforms();
            await CommandSeed.SeedData(services.ServiceProvider.GetService<ICommandsRepository>(), platforms);
        }        

        return app;
    }
}
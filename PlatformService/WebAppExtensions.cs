using PlatformService;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;

public static class WebAppExtensions
{
    /// <summary>
    /// Adds all Services which required for PlatformService work
    /// </summary>
    public static async Task RunPlatform(this WebApplication app)
    {
        await app.SetUpDb(app.Environment.IsProduction());
        app.UseMiddleware<ExceptionMiddleware>()
           .UseRouting()
           .UseEndpoints(endpoints =>
           {
               endpoints.MapControllers();
               endpoints.MapGrpcService<GrpcPlatformService>();

               endpoints.MapGet("/protos/platforms.proto", async context =>
               {
                   await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
               });
           });

        app.Run();
    }

    /// <summary>
    /// Adds all scopes which required for PlatformService work
    /// </summary>
    public static async Task<IApplicationBuilder> SetUpDb(this IApplicationBuilder app, bool isProdaction)
    {
        using (var services = app.ApplicationServices.CreateScope())
        {
            var dbContext = services.ServiceProvider.GetService<AppDbContext>();
            //await dbContext.Database.MigrateAsync();
            await PlatformSeed.SeedData(dbContext, isProdaction);
        }        

        return app;
    }
}
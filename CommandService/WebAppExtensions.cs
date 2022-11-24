using CommandService;
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
            //var dbContext = services.ServiceProvider.GetService<AppDbContext>();
            //await dbContext.Database.MigrateAsync();
            //await PlatformSeed.SeedData(dbContext);
        }        

        return app;
    }
}
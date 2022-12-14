using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PlatformSeed 
{
    public static async Task SeedData(AppDbContext context, bool isProdaction)
    {
        if (isProdaction)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                context.Database.Migrate();
                Console.WriteLine("--> Migrations applied");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }            
        }

        if (context.Platforms.FirstOrDefault() is not null) return;

        await context.Platforms.AddRangeAsync(
            new Platform { Name = "First", Cost = 111, Publisher = "Dev" },
            new Platform { Name = "Second", Cost = 222, Publisher = "Dev" }
            );
        await context.SaveChangesAsync();
    }
}
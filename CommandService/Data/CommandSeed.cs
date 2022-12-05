using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data;

public static class CommandSeed
{
    public static async Task SeedData(AppDbContext context, bool isProdaction)
    {
        if (isProdaction)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                //context.Database.Migrate();
                Console.WriteLine("--> Migrations applied");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }

        if (context.Platforms.FirstOrDefault() is not null) return;

        await context.Platforms.AddRangeAsync(
            new Platform { Name = "First1" },
            new Platform { Name = "Second2" }
            );
        await context.SaveChangesAsync();
    }
}
using PlatformService.Models;

namespace PlatformService.Data;

public static class PlatformSeed 
{
    public static async Task SeedData(AppDbContext context)
    {
        if (context.Platforms.FirstOrDefault() is not null) return;

        await context.Platforms.AddRangeAsync(
            new Platform { Name = "First", Cost = 111, Publisher = "Dev" },
            new Platform { Name = "Second", Cost = 222, Publisher = "Dev" }
            );
        await context.SaveChangesAsync();
    }
}
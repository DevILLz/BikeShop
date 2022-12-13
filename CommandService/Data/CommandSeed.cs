using CommandService.Models;

namespace CommandService.Data;

public static class CommandSeed
{
    public static Task SeedData(ICommandsRepository repos, IEnumerable<Platform> platforms)
    {
        Console.WriteLine($"Seeding platforms...");

        foreach (var platform in platforms)
        {
            if (repos.ExternalPlatformExist(platform.ExternalID) is false)
            {
                repos.CreatePlatform(platform);
            }
        }
        repos.SaveChanges();

        return Task.CompletedTask;
    }
}
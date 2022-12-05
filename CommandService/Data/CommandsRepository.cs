using CommandService.Models;

namespace CommandService.Data;

public class CommandsRepository : ICommandsRepository
{
    private readonly AppDbContext context;

    public CommandsRepository(AppDbContext context)
    {
        this.context = context;
    }

    public void CreatePlatform(Platform platform)
    {
        if (platform is null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        context.Platforms.Add(platform);
        //SaveChanges();
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return context.Platforms.ToList();
    }

    public Platform GetPlatformById(int id)
    {
        return context.Platforms.FirstOrDefault(p => p.Id == id);
    }

    public bool DeletePlatformById(int id)
    {
        var platform = context.Platforms.FirstOrDefault(p => p.Id == id);
        if (platform is null) return false;

        context.Platforms.Remove(platform);
        return SaveChanges();
    }

    public bool SaveChanges()
    {
        return context.SaveChanges() >= 0;
    }

    public bool PlatformExists(int platformId)
    {
        return context.Platforms.Any(p => p.Id == platformId);
    }

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
        return context.Commands
            .Where(c => c.PlatformId == platformId)
            .OrderBy(c => c.Platform.Name);
    }

    public void CreateCommand(int platformId, Command command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        command.PlatformId = platformId;

        context.Commands.Add(command);
    }

    public Command GetCommand(int platformId, int commandId)
    {
        return context.Commands
            .Where(c => c.PlatformId == platformId && c.Id == commandId)
            .FirstOrDefault();
    }
}
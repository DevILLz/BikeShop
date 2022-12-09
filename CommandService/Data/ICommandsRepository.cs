using CommandService.Models;

namespace CommandService.Data;

public interface ICommandsRepository
{
    bool SaveChanges();

    IEnumerable<Platform> GetAllPlatforms();
    void CreatePlatform(Platform platform);
    bool PlatformExists(int platformId);
    bool ExternalPlatformExist(int externalPlatformId);

    IEnumerable<Command> GetCommandsForPlatform(int platformId);
    void CreateCommand(int platformId, Command command);
    Command GetCommand(int platformId, int commandId);
}
using PlatformService.DTOs;

namespace PlatformService.AsyncDataServices;

public interface IMessageBussClient
{
    void PublishNewPlatform(PlatformPublishDto dto);
}

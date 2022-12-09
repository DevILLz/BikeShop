using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using CommandService.Models;
using System.Text.Json;

namespace CommandService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly IMapper mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        this.scopeFactory = scopeFactory;
        this.mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        switch (DetermineEventType(message))
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                return;
            default: 
                return;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandsRepository>();
            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishedMessage);

            try
            {
                var platform = mapper.Map<Platform>(platformPublishedDto);

                if (repo.ExternalPlatformExist(platform.ExternalID) is false)
                {
                    repo.CreatePlatform(platform);
                    repo.SaveChanges();
                    Console.WriteLine("--> Platform added");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
            }
        }
    }

    private EventType DetermineEventType(string message)
    {
        Console.WriteLine("--> Determining Event");

        return JsonSerializer.Deserialize<GenericEventDto>(message).Event switch
        {
            "Platform_Published" => EventType.PlatformPublished,
            _ => EventType.Undetermined,
        };
    }

    private enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
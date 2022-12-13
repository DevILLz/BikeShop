using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.SyncDataServices.Grpc;

public class PlatformDataClient : IPlatformDataClient
{
    private readonly IConfiguration config;
    private readonly IMapper mapper;

    public PlatformDataClient(IConfiguration config, IMapper mapper)
    {
        this.config = config;
        this.mapper = mapper;
    }

    public IEnumerable<Platform> ReturnAllPlatforms()
    {
        var address = config["GrpcPlatform"];
        Console.WriteLine($"--> Calling GRPC Service ({address})");

        var channel = GrpcChannel.ForAddress(address);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllPlatforms(request);
            return mapper.Map<IEnumerable<Platform>>(reply.Platforms);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not call GRPC server: {ex.Message}");
        }

        return Enumerable.Empty<Platform>();
    }
}

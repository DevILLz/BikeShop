using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IMapper mapper;
    private readonly IPlatformRepository repository;

    public GrpcPlatformService(IMapper mapper, IPlatformRepository repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var response = new PlatformResponse();
        var platforms = repository.GetAllPlatforms();

        foreach (var item in platforms)
        {
            response.Platforms.Add(mapper.Map<GrpcPlatformModel>(item));
        }

        return Task.FromResult(response);
    }
}

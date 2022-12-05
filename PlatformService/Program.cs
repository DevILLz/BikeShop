var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPlatformServices(builder.Configuration, builder.Environment);
await builder.Build().RunPlatform();
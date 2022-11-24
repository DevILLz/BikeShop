var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPlatformServices();
await builder.Build().RunPlatform();
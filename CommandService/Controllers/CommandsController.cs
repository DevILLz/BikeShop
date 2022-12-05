using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;
namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/c/platforms/{platformId}/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandsRepository repos;
        private readonly IMapper mapper;

        public CommandsController(ICommandsRepository commandRepository, IMapper mapper)
        {
            repos = commandRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll(int platformId)
        {
            if (repos.PlatformExists(platformId) is false)
                return NotFound();
            
            return Ok(mapper.Map<IEnumerable<CommandReadDto>>(repos.GetCommandsForPlatform(platformId)));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public IActionResult GetCommandForPlatform(int platformId, int commandId)
        {
            if (repos.PlatformExists(platformId) is false)
                return NotFound();

            var command = repos.GetCommand(platformId, commandId);

            if (command is null)
                return NotFound();

            return Ok(mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public IActionResult Create(int platformId, CommandCreateDto command)
        {
            if (repos.PlatformExists(platformId) is false)
                return NotFound();

            var createdCommand = mapper.Map<Command>(command);

            repos.CreateCommand(platformId, createdCommand);
            // Id adds after creating
            repos.SaveChanges();

            return CreatedAtRoute(nameof(GetCommandForPlatform), new 
            {
                platformId,
                commandId = createdCommand.Id
            }, createdCommand);
        }
    }
}
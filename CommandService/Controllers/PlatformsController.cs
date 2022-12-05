using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandsRepository repos;
        private readonly IMapper mapper;

        public PlatformsController(ICommandsRepository commandRepository, IMapper mapper)
        {
            repos = commandRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(repos.GetAllPlatforms()));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            //var platform = repos.GetPlatformById(id);
            //if (platform is null)
                return NotFound(id);

            //return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platform));
        }

        [HttpPost]
        public IActionResult Create(PlatformReadDto platform)
        {
            
            //var createdPlatform = mapper.Map<Platform>(platform);
            //repos.CreatePlatform(createdPlatform);
            //// Id adds after creating
            //repos.SaveChanges();

            return Ok($"Hello there from CommandService \n {platform.ToString()}");
        }
    }
}
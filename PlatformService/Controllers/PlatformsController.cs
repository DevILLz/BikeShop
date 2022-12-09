using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase 
    {
        private readonly ICommandDataClient commandDataClient;
        private readonly IMessageBussClient messageBussClient;
        private readonly IPlatformRepository repos;
        private readonly IMapper mapper;

        public PlatformsController(IPlatformRepository repos, 
                                   IMapper mapper, 
                                   ICommandDataClient commandDataClient,
                                   IMessageBussClient messageBussClient)
        {
            this.repos = repos;
            this.mapper = mapper;
            this.commandDataClient = commandDataClient;
            this.messageBussClient = messageBussClient;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(repos.GetAllPlatforms()));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var platform = repos.GetPlatformById(id);
            if (platform is null) 
                return NotFound(id);

            return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platform));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(PlatformCreateDto platform)
        {
            var createdPlatform = mapper.Map<Platform>(platform);
            repos.CreatePlatform(createdPlatform);
            // Id adds after creating
            repos.SaveChanges();

            var platformReadDto = mapper.Map<PlatformReadDto>(createdPlatform);

            try 
            {
                await commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Coult not send sync msg: {ex.Message}");
            }

            try
            {
                var platformPublishDto = mapper.Map<PlatformPublishDto>(platformReadDto);
                platformPublishDto.Event = "Platform_Published";
                messageBussClient.PublishNewPlatform(platformPublishDto);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"--> Coult not send async msg: {ex.Message}");
            }

            return Ok(createdPlatform.Id);
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
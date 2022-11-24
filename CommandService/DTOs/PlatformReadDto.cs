using System.ComponentModel.DataAnnotations;

namespace CommandService.DTOs
{
    public class PlatformReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Publisher { get; set; }

        public int Cost { get; set; }
    }
}
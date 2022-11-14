using System.Collections.Generic;

namespace CommandService.DTOs
{
    public class PlatformReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<CommandReadDto> Commands { get; set; }
    }
}
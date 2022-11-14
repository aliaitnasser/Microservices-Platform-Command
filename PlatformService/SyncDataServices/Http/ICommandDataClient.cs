using System.Threading.Tasks;
using PlatformService.DTOs;

namespace PlatformService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        // POST api/c/platforms
        Task SendPlatformToCommand(PlatformReadDto platform);
    }
}
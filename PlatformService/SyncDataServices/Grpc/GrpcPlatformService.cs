using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using static PlatformService.GrpcPlatform;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatformBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public override Task<GetAllResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new GetAllResponse();
            var platforms = _repo.GetAllPlatforms();

            foreach (var platform in platforms)
            {
                response.Platforms.Add(_mapper.Map<GrpcPlatformModel>(platform));
            }

            return Task.FromResult(response);
        }
    }
}

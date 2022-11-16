using System;
using System.Collections.Generic;
using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using PlatformService;

namespace CommandService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _config = config;
        }

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"---> Calling GRPC Service {_config["GrpcPlatform"]}");

            var channel = GrpcChannel.ForAddress(_config["GrpcPlatform"]);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.Platforms);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not call GRPC Service {_config["GrpcPlatform"]}");
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}

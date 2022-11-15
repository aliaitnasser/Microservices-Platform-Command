using System;
using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using CommandService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    Console.WriteLine("---> Platform Published Event Detected");
                    AddPlatform(message);
                    break;
                default:
                    Console.WriteLine("---> Could not determine the event type");
                    break;
            }
        }

        private void AddPlatform(string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishDto>(message);
                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);
                    if (!repo.ExternalPlatformExists(plat.ExternalId))
                    {
                        // Adding the platform to the database
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("---> Platform already exists");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"---> Could not add platform to DB: {ex.Message}");
                }
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("---> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("---> Platform_Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("---> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}

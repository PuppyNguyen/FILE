using MassTransit;
using Microsoft.Extensions.Logging;
using EA.NetDevPack.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Event = EA.NetDevPack.Messaging.Event;

namespace EA.Infra.FILE
{
    public class EventConsumer : IConsumer<Event>
    {
        private readonly ILogger<EventConsumer> _logger;

        public EventConsumer(ILogger<EventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Event> context)
        {
             
            _logger.LogInformation("EventConsumer:" + JsonConvert.SerializeObject(context.Message));
            return Task.CompletedTask;
        }
    }
    
}

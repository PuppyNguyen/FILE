using System.Threading.Tasks; 
using FluentValidation.Results;
using MediatR;
using EA.NetDevPack.Mediator;
using EA.NetDevPack.Messaging;
using EA.NetDevPack.Queries;
using EA.NetDevPack.Events;

namespace EA.Infra.FILE.Bus
{
    public sealed class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventStore _eventStore;

        public MediatorHandler(IEventStore eventStore, IMediator mediator)
        {
            _eventStore = eventStore;
            _mediator = mediator;
        }

        public async Task PublishEvent<T>(T @event) where T : Event
        {
            if (!@event.MessageType.Equals("DomainNotification"))
                _eventStore?.Save(@event);

            await _mediator.Publish(@event);
        }

        public async Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>
        {
            return await _mediator.Send(query);
            //throw new NotImplementedException();
        }

        public async Task<TResponse> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(request); 
        }

        public async Task<ValidationResult> SendCommand<T>(T command) where T : Command
        {
            return await _mediator.Send(command);
        }

    }
}
using System.Threading.Tasks;
using FluentValidation.Results;
using EA.NetDevPack.Messaging;
using EA.NetDevPack.Queries;

namespace EA.NetDevPack.Mediator
{
    public interface IMediatorHandler
    {
        Task<TResponse> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default(CancellationToken));
        Task PublishEvent<T>(T @event) where T : Event;
        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
        //Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>;
       // TResponse Send<TQuery>(TQuery query) where TQuery : IQuery<TResponse>;
    }
}
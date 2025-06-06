namespace DomainEventDispatcher.SharedKernel.Primitives
{
    using System.Threading;
    using System.Threading.Tasks;
    using DomainEventDispatcher.SharedKernel.Abstractions;

    public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        public abstract Task Handle(TDomainEvent notification, CancellationToken cancellationToken = default);

        public Task Handle(object notification, CancellationToken cancellationToken = default)
        {
            return Handle((TDomainEvent)notification, cancellationToken);
        }
    }
}

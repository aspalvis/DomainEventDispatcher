namespace DomainEventDispatcher.SharedKernel.Abstractions
{
    public interface IDomainEventHandler<TDomainEvent> : IDomainEventHandler
        where TDomainEvent : IDomainEvent
    {
        Task Handle(TDomainEvent notification, CancellationToken cancellationToken = default);
    }

    public interface IDomainEventHandler
    {
        Task Handle(object notification, CancellationToken cancellationToken = default);
    }
}

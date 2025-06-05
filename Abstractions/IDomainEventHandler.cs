namespace DomainEventDispatcher.Abstractions
{
    public interface IDomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        Task Handle(TDomainEvent notification, CancellationToken cancellationToken = default);
    }
}

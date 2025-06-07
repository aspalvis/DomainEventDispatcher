namespace DomainEventDispatcher.Domain.UserAggregate
{
    using DomainEventDispatcher.SharedKernel.Abstractions;

    public sealed class UserCreatedDomainEvent : IDomainEvent
    {
        public UserCreatedDomainEvent(User user)
        {
            User = user;
        }

        public User User { get; private set; } = default!;
    }
}

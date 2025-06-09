namespace DomainEventDispatcher.Domain.UserAggregate
{
    using ASCA.ToolKit.SharedKernel.CQRS;

    public sealed class UserCreatedDomainEvent : INotification
    {
        public UserCreatedDomainEvent(User user)
        {
            User = user;
        }

        public User User { get; private set; } = default!;
    }
}

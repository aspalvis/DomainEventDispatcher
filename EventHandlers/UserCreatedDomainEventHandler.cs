namespace DomainEventDispatcher.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using DomainEventDispatcher.Domain.UserAggregate;
    using DomainEventDispatcher.SharedKernel.Primitives;

    public class UserCreatedDomainEventHandler : DomainEventHandler<UserCreatedDomainEvent>
    {
        public override Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken = default)
        {
            notification.User.Username += "(hi from UserCreatedDomainEventHandler!)";

            return Task.CompletedTask;
        }
    }
}

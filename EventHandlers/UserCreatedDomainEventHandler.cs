namespace DomainEventDispatcher.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using ASCA.ToolKit.SharedKernel.CQRS;
    using DomainEventDispatcher.Domain.UserAggregate;

    public class UserCreatedDomainEventHandler : NotificationHandler<UserCreatedDomainEvent>
    {
        public override Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken = default)
        {
            notification.User.Username += "(hi from UserCreatedDomainEventHandler!)";

            return Task.CompletedTask;
        }
    }
}

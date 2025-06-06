namespace DomainEventDispatcher.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using DomainEventDispatcher.Data;
    using DomainEventDispatcher.Domain.PersonAggregate;
    using DomainEventDispatcher.Domain.UserAggregate;
    using DomainEventDispatcher.SharedKernel.Primitives;

    public class PersonCreatedDomainEventHandler : DomainEventHandler<PersonCreatedDomainEvent>
    {
        private readonly AppDbContext _db;

        public PersonCreatedDomainEventHandler(AppDbContext db)
        {
            _db = db;
        }

        public override Task Handle(PersonCreatedDomainEvent notification, CancellationToken cancellationToken = default)
        {
            var user = User.Create(notification.Person);

            _db.Users.Add(user);

            return Task.CompletedTask;
        }
    }
}

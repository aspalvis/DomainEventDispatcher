namespace DomainEventDispatcher.Features.Person.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using DomainEventDispatcher.Abstractions;
    using DomainEventDispatcher.Data;
    using DomainEventDispatcher.Domain.PersonAggregate;
    using DomainEventDispatcher.Domain.UserAggregate;

    public class PersonCreatedDomainEventHandler : IDomainEventHandler<PersonCreatedDomainEvent>
    {
        private readonly AppDbContext _db;

        public PersonCreatedDomainEventHandler(AppDbContext db)
        {
            _db = db;
        }

        public Task Handle(PersonCreatedDomainEvent notification, CancellationToken cancellationToken = default)
        {
            var user = User.Create(notification.Person);

            _db.Users.Add(user);

            return Task.CompletedTask;
        }
    }
}

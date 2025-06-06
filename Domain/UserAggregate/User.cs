namespace DomainEventDispatcher.Domain.UserAggregate
{
    using DomainEventDispatcher.Domain.PersonAggregate;
    using DomainEventDispatcher.SharedKernel.Primitives;

    public class User : Entity
    {
        public string Username { get; set; } = default!;

        public int PersonId { get; set; } = default!;

        public Person Person { get; set; } = default!;

        public static User Create(Person person)
        {
            var user = new User()
            {
                Person = person,
                Username = $"{person.Name}.{person.LastName}"
            };

            user.RaiseDomainEvent(new UserCreatedDomainEvent(user));

            return user;
        }
    }
}

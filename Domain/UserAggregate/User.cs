namespace DomainEventDispatcher.Domain.UserAggregate
{
    using DomainEventDispatcher.Abstractions;
    using DomainEventDispatcher.Domain.PersonAggregate;

    public class User : Entity
    {
        public string Username { get; set; } = default!;

        public int PersonId { get; set; } = default!;

        public Person Person { get; set; } = default!;

        public static User Create(Person person)
        {
            return new()
            {
                Person = person,
                Username = $"{person.Name}.{person.LastName}"
            };
        }
    }
}

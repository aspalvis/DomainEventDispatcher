namespace DomainEventDispatcher.Domain.PersonAggregate
{
    using DomainEventDispatcher.Domain.UserAggregate;
    using DomainEventDispatcher.SharedKernel.Primitives;

    public class Person : Entity
    {
        public string Name { get; set; } = default!;
        public string LastName { get; set; } = default!;

        public User? User { get; set; }

        public static Person Create(string name, string lastName)
        {
            var person = new Person()
            {
                Name = name,
                LastName = lastName
            };

            person.RaiseDomainEvent(new PersonCreatedDomainEvent(person));

            return person;
        }
    }
}

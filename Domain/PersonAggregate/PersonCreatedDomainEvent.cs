namespace DomainEventDispatcher.Domain.PersonAggregate
{
    using DomainEventDispatcher.SharedKernel.Abstractions;

    public sealed class PersonCreatedDomainEvent : IDomainEvent
    {
        public PersonCreatedDomainEvent(Person person)
        {
            Person = person;
        }

        public Person Person { get; private set; } = default!;
    }
}

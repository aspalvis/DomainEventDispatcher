namespace DomainEventDispatcher.Domain.PersonAggregate
{
    using ASCA.ToolKit.SharedKernel.CQRS;

    public sealed class PersonCreatedDomainEvent : INotification
    {
        public PersonCreatedDomainEvent(Person person)
        {
            Person = person;
        }

        public Person Person { get; private set; } = default!;
    }
}

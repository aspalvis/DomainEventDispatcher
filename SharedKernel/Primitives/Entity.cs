namespace DomainEventDispatcher.SharedKernel.Primitives
{
    using System.ComponentModel.DataAnnotations;
    using DomainEventDispatcher.SharedKernel.Abstractions;

    public class Entity
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        [Key]
        public int Id { get; set; }

        public IReadOnlyList<IDomainEvent> GetDomainEvents() => [.. _domainEvents];

        public void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public bool HasDomainEvents()
        {
            return _domainEvents.Count > 0;
        }
    }
}

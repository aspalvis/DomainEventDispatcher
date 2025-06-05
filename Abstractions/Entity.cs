namespace DomainEventDispatcher.Abstractions
{
    using System.ComponentModel.DataAnnotations;

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

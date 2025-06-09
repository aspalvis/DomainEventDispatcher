namespace DomainEventDispatcher.SharedKernel.Primitives
{
    using System.ComponentModel.DataAnnotations;
    using ASCA.ToolKit.SharedKernel.CQRS;

    public class Entity
    {
        private readonly List<INotification> _domainEvents = [];

        [Key]
        public int Id { get; set; }

        public IReadOnlyList<INotification> GetDomainEvents() => [.. _domainEvents];

        public void RaiseDomainEvent(INotification domainEvent)
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

namespace DomainEventDispatcher.Data.Interceptors
{
    using ASCA.ToolKit.SharedKernel.CQRS;
    using DomainEventDispatcher.SharedKernel.Primitives;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
    {
        private readonly INotificationSender _sender;

        public DispatchDomainEventsInterceptor(INotificationSender sender)
        {
            _sender = sender;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task DispatchDomainEventsAsync(DbContext? context, CancellationToken cancellationToken)
        {
            if (context is null)
            {
                return;
            }

            var events = new List<INotification>();

            foreach (var entry in context.ChangeTracker.Entries<Entity>())
            {
                if (entry.Entity.HasDomainEvents())
                {
                    events.AddRange(entry.Entity.GetDomainEvents());
                    entry.Entity.ClearDomainEvents();
                }
            }

            if (events.Count == 0)
            {
                return;
            }

            foreach (var domainEvent in events)
            {
                await _sender.SendNotificationAsync(domainEvent, cancellationToken);
            }

            //For cases when domain event is triggering other domain events, and change tracker will have new entries
            await DispatchDomainEventsAsync(context, cancellationToken);
        }
    }
}

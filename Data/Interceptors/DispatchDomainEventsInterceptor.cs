namespace DomainEventDispatcher.Data.Interceptors
{
    using DomainEventDispatcher.SharedKernel.Abstractions;
    using DomainEventDispatcher.SharedKernel.Primitives;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;

    public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
    {
        private readonly IServiceProvider _serviceProvider;

        public DispatchDomainEventsInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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

            var events = new List<IDomainEvent>();

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
                await Dispatch(domainEvent, cancellationToken);
            }

            //For cases when domain event is triggering other domain events, and change tracker will have new entries
            await DispatchDomainEventsAsync(context, cancellationToken);
        }

        private async Task Dispatch(IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var type = domainEvent.GetType();
            var keyedHandlers = _serviceProvider.GetKeyedServices<IDomainEventHandler>($"{type.Name}Handler");

            if (keyedHandlers is null)
            {
                return;
            }

            foreach (var handler in keyedHandlers)
            {
                await handler.Handle(domainEvent, cancellationToken);
            }
        }
    }
}

namespace DomainEventDispatcher.Data.Interceptors
{
    using System.Collections.Concurrent;
    using System.Reflection;
    using DomainEventDispatcher.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> _handlerMethodCache = [];
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

            foreach (var domainEvent in events)
            {
                await Dispatch(domainEvent, cancellationToken);
            }
        }

        private async Task Dispatch(IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var eventType = domainEvent.GetType();
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

            var handlers = _serviceProvider.GetServices(handlerType);

            if (handlers is null)
            {
                return;
            }

            var handleMethod = _handlerMethodCache.GetOrAdd(eventType, type =>
                handlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.Handle))!);

            foreach (var handler in handlers)
            {
                var task = (Task?)handleMethod.Invoke(handler, new object[] { domainEvent, cancellationToken });

                if (task != null)
                {
                    await task;
                }
            }
        }
    }
}

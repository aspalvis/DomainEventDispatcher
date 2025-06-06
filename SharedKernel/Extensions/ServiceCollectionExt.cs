namespace DomainEventDispatcher.SharedKernel.Extensions
{
    using System.Reflection;
    using DomainEventDispatcher.SharedKernel.Abstractions;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExt
    {
        public static IServiceCollection AddDomainEventHandlers(this IServiceCollection services, Assembly assembly)
        {
            var handlerInterfaceType = typeof(IDomainEventHandler<>);

            foreach (var type in assembly.GetTypes())
            {
                if (!IsValidHandlerType(type))
                    continue;

                var implementedInterface = type.GetInterfaces()
                    .FirstOrDefault(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == handlerInterfaceType);

                if (implementedInterface is null)
                    continue;

                services.AddKeyedScoped(typeof(IDomainEventHandler), type.Name, type);
            }

            return services;
        }

        private static bool IsValidHandlerType(Type type) =>
            !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any();
    }
}

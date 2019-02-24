using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SampleEvent.LightInject
{
    public class EventHandleProvider : IEventHandleProvider
    {
        private IServiceContainer ServiceContainer { get; }

        public EventHandleProvider(IServiceContainer serviceContainer, params Assembly[] assemblies)
        : this(serviceContainer, () => new PerContainerLifetime(), assemblies)
        {
        }

        public EventHandleProvider(IServiceContainer serviceContainer,
            Func<ILifetime> lifetimeFactory, params Assembly[] assemblies)
        {
            this.ServiceContainer = serviceContainer;

            this.RegisterEventHandlersFromAssemblies(serviceContainer, lifetimeFactory, assemblies);
        }

        public IEnumerable<IEventHandler<TSampleEvent>> GetEventHandlers<TSampleEvent>() where TSampleEvent : ISampleEvent
        {
            return this.ServiceContainer
                .GetAllInstances<IEventHandler<TSampleEvent>>();
        }

        public IEnumerable<Func<TSampleEvent, Task>> GetHandles<TSampleEvent>() where TSampleEvent : ISampleEvent
        {
            return this.GetEventHandlers<TSampleEvent>()
                 .Select(s => (Func<TSampleEvent, Task>)s.Handle);
        }

        private void RegisterEventHandlersFromAssemblies(IServiceContainer serviceContainer, Func<ILifetime> lifetimeFactory,
            Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                serviceContainer.RegisterAssembly(assembly, lifetimeFactory, (interfaceType, eventHandlerType) =>
                    interfaceType.IsConstructedGenericType &&
                    interfaceType.GetGenericTypeDefinition() == typeof(IEventHandler<>));
            }
        }
    }
}

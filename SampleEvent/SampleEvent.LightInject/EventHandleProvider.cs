using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SampleEvent.LightInject
{
    public class EventHandleProvider : IEventHandleProvider
    {
        private IServiceContainer ServiceContainer { get; }
        private IEnumerable<Assembly> Assemblies { get; }

        public EventHandleProvider(IServiceContainer serviceContainer, params Assembly[] assemblies)
        {
            this.ServiceContainer = serviceContainer;
            this.Assemblies = assemblies;

            RegisterEventHandlers();
        }

        public void RegisterEventHandlers()
        {
            foreach (var assembly in Assemblies)
            {
                this.ServiceContainer.RegisterAssembly(assembly, (interfaceType, eventHandlerType) =>
                    interfaceType.IsConstructedGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEventHandler<>));
            }
        }

        public IEnumerable<IEventHandler<TSampleEvent>> GetEventHandlers<TSampleEvent>() where TSampleEvent : ISampleEvent
        {
            return this.ServiceContainer
                .GetAllInstances<IEventHandler<TSampleEvent>>();
        }

        public IEnumerable<Func<TSampleEvent, Task>> GetHandles<TSampleEvent>() where TSampleEvent : ISampleEvent
        {
            return this.ServiceContainer
                 .GetAllInstances<IEventHandler<TSampleEvent>>()
                 .Select(s => (Func<TSampleEvent, Task>)s.Handle);
        }
    }
}

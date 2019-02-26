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

        public EventHandleProvider(IServiceContainer serviceContainer)
        {
            this.ServiceContainer = serviceContainer;
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
    }
}

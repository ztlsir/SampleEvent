using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleEvent
{
    public class EventHandleProvider : IEventHandleProvider
    {
        private readonly Lazy<ConcurrentDictionary<Type, ConcurrentDictionary<Type, object>>> lazyEventHandlers = new Lazy<ConcurrentDictionary<Type, ConcurrentDictionary<Type, object>>>();

        public EventHandleProvider(params object[] eventHandlers)
        {
            eventHandlers
                .Where(eventHandler => GetEventHandlerInterfaceType(eventHandler.GetType()) != null)
                .ToList()
                .ForEach(eventHandler =>
                    this.lazyEventHandlers.Value
                        .GetOrAdd(GetEventHandlerInterfaceType(eventHandler.GetType()), valueOfType => new ConcurrentDictionary<Type, object>())
                        .GetOrAdd(eventHandler.GetType(), eventHandler));
        }

        public void RegisterEventHandlers()
        {
        }

        public IEnumerable<IEventHandler<TSampleEvent>> GetEventHandlers<TSampleEvent>() where TSampleEvent : ISampleEvent
        {
            return lazyEventHandlers.Value
                .GetOrAdd(typeof(IEventHandler<TSampleEvent>), new ConcurrentDictionary<Type, object>())
                .Select(s => (IEventHandler<TSampleEvent>)s.Value);
        }

        public IEnumerable<Func<TSampleEvent, Task>> GetHandles<TSampleEvent>() where TSampleEvent : ISampleEvent
        {
            return lazyEventHandlers.Value
                .GetOrAdd(typeof(IEventHandler<TSampleEvent>), new ConcurrentDictionary<Type, object>())
                .Select<KeyValuePair<Type, object>, Func<TSampleEvent, Task>>(s => ((IEventHandler<TSampleEvent>)s.Value).Handle);
        }

        private Type GetEventHandlerInterfaceType(Type eventHandlerType)
        {
            return eventHandlerType
                .GetInterfaces()
                .FirstOrDefault(interfaceType => interfaceType.IsGenericType
                                         && interfaceType.GetGenericTypeDefinition() == typeof(IEventHandler<>));
        }
    }
}

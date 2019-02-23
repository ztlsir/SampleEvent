using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleEvent
{
    public class EventHandleProvider : BaseEventHandleProvider
    {
        private readonly Lazy<ConcurrentDictionary<Type, ConcurrentDictionary<Type, object>>> _eventHandlers = new Lazy<ConcurrentDictionary<Type, ConcurrentDictionary<Type, object>>>();

        public override void RegisterEvents()
        {
            AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(type => GetEventHandlerInterfaceType(type) != null)
                .ToList()
                .ForEach(eventHandlerType =>
                    _eventHandlers.Value
                    .GetOrAdd(GetEventHandlerInterfaceType(eventHandlerType), valueOfType => new ConcurrentDictionary<Type, object>())
                    .GetOrAdd(eventHandlerType, valueOfType => Activator.CreateInstance(eventHandlerType)));
        }

        public override IEnumerable<IEventHandler<TSampleEvent>> GetEventHandlers<TSampleEvent>()
        {
            return _eventHandlers.Value
                .GetOrAdd(typeof(IEventHandler<TSampleEvent>), new ConcurrentDictionary<Type, object>())
                .Select(s => (IEventHandler<TSampleEvent>)s.Value);
        }

        public override IEnumerable<Func<TSampleEvent, Task>> GetHandles<TSampleEvent>()
        {
            return _eventHandlers.Value
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

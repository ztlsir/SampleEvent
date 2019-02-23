using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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
                .Where(type => GetEventHandlerTypeFromInterfaces(type) != null)
                .ToList()
                .ForEach(type =>
                    _eventHandlers.Value
                    .GetOrAdd(GetEventHandlerTypeFromInterfaces(type), valueOfType => new ConcurrentDictionary<Type, object>())
                    .GetOrAdd(type, valueOfType => Activator.CreateInstance(type)));
        }

        public override IEnumerable<IEventHandler<TSampleEvent>> GetEventHandlers<TSampleEvent>()
        {
            return _eventHandlers.Value
                .GetOrAdd(typeof(IEventHandler<TSampleEvent>), new ConcurrentDictionary<Type, object>())
                .Values
                .ToList()
                .Select(s => (IEventHandler<TSampleEvent>)s);
        }

        public override IEnumerable<Action<TSampleEvent>> GetHandles<TSampleEvent>()
        {
            return _eventHandlers.Value
                .GetOrAdd(typeof(IEventHandler<TSampleEvent>), new ConcurrentDictionary<Type, object>())
                .Values
                .ToList()
                .Select<object, Action<TSampleEvent>>(s => ((IEventHandler<TSampleEvent>)s).Handle);
        }

        private Type GetEventHandlerTypeFromInterfaces(Type type)
        {
            return type
                .GetInterfaces()
                .FirstOrDefault(iType => iType.IsGenericType
                                         && iType.GetGenericTypeDefinition() == typeof(IEventHandler<>));
        }
    }
}

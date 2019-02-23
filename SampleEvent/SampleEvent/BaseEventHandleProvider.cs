using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SampleEvent
{
    public abstract class BaseEventHandleProvider : IEventHandleProvider
    {
        protected BaseEventHandleProvider()
        {
            RegisterEvents();
        }

        public abstract void RegisterEvents();
        public abstract IEnumerable<IEventHandler<TSampleEvent>> GetEventHandlers<TSampleEvent>() where TSampleEvent : ISampleEvent;
        public abstract IEnumerable<Action<TSampleEvent>> GetHandles<TSampleEvent>() where TSampleEvent : ISampleEvent;
    }
}

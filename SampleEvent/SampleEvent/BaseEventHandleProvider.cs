using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public abstract IEnumerable<Func<TSampleEvent, Task>> GetHandles<TSampleEvent>() where TSampleEvent : ISampleEvent;
    }
}

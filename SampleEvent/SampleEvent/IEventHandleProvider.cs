using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleEvent
{
    public interface IEventHandleProvider
    {
        void RegisterEventHandlers();
        IEnumerable<IEventHandler<TSampleEvent>> GetEventHandlers<TSampleEvent>() where TSampleEvent : ISampleEvent;
        IEnumerable<Func<TSampleEvent, Task>> GetHandles<TSampleEvent>() where TSampleEvent : ISampleEvent;
    }
}

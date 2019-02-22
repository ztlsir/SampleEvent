using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleEvent
{
    public interface IEventHandleProvider
    {
        void RegisterEvents();
        IEnumerable<IEventHandler<TSampleEvent>> GetEventHandlers<TSampleEvent>() where TSampleEvent : ISampleEvent;
        IEnumerable<Action<TSampleEvent>> GetHandles<TSampleEvent>() where TSampleEvent : ISampleEvent;
    }
}

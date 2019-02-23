using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SampleEvent
{
    public class NewEventHandleProvider : BaseEventHandleProvider
    {
        public override void RegisterEvents()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IEventHandler<TSampleEvent>> GetEventHandlers<TSampleEvent>()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Action<TSampleEvent>> GetHandles<TSampleEvent>()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleEvent
{
    public interface IEventHandler<TSampleEvent> where TSampleEvent : ISampleEvent
    {
        void Handle(TSampleEvent sampleEvent);
    }
}

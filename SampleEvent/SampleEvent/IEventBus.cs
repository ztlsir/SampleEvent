using System.Threading.Tasks;

namespace SampleEvent
{
    public interface IEventBus
    {
        Task Publish<TSampleEvent>(TSampleEvent sampleEvent) where TSampleEvent : ISampleEvent;
    }
}

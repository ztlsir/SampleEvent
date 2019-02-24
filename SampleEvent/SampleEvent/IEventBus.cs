using System.Threading.Tasks;

namespace SampleEvent
{
    public interface IEventBus
    {
        Task<IEventBus> Publish<TSampleEvent>(TSampleEvent sampleEvent) where TSampleEvent : ISampleEvent;
    }
}

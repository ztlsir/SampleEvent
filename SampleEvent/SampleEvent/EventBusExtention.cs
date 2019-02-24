using System.Threading.Tasks;

namespace SampleEvent
{
    public static class EventBusExtention
    {

        public static async Task<IEventBus> Publish<TSampleEvent>(this Task<IEventBus> @this, TSampleEvent sampleEvent) where TSampleEvent : ISampleEvent
        {
            return await (await @this).Publish(sampleEvent);
        }
    }
}

using System.Threading.Tasks;

namespace SampleEvent
{
    public class EventBus : IEventBus
    {
        private IEventHandleProvider EventHandleProvider { get; }

        public EventBus(IEventHandleProvider eventHandleProvider)
        {
            this.EventHandleProvider = eventHandleProvider;
        }

        public async Task Publish<TSampleEvent>(TSampleEvent sampleEvent) where TSampleEvent : ISampleEvent
        {
            foreach (var handle in this.EventHandleProvider.GetHandles<TSampleEvent>())
            {
                await handle(sampleEvent).ConfigureAwait(false);
            }
        }
    }
}

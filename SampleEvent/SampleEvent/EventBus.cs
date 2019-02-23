using System.Threading.Tasks;

namespace SampleEvent
{
    public class EventBus : IEventBus
    {
        private IEventHandleProvider EventHandleProvider { get;}

        public EventBus(IEventHandleProvider eventHandleProvider)
        {
            this.EventHandleProvider = eventHandleProvider;
        }

        public async Task Publish<TSampleEvent>(TSampleEvent sampleEvent) where TSampleEvent : ISampleEvent
        {
            var handles = this.EventHandleProvider.GetHandles<TSampleEvent>();

            foreach (var handle in handles)
            {
                await handle(sampleEvent);
            }
        }
    }
}

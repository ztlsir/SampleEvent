namespace SampleEvent
{
    public class EventBus : IEventBus
    {
        private IEventHandleProvider eventHandleProvider { get; set; }

        public EventBus(IEventHandleProvider eventHandleProvider)
        {
            this.eventHandleProvider = eventHandleProvider;
        }

        public void Publish<TSampleEvent>(TSampleEvent sampleEvent) where TSampleEvent : ISampleEvent
        {
            var handles = this.eventHandleProvider.GetHandles<TSampleEvent>();

            foreach (var handle in handles)
            {
                handle(sampleEvent);
            }
        }
    }
}

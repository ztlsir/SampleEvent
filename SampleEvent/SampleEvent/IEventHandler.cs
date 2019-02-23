using System.Threading.Tasks;

namespace SampleEvent
{
    public interface IEventHandler<in TSampleEvent> where TSampleEvent : ISampleEvent
    {
        Task Handle(TSampleEvent lostEvent);
    }
}

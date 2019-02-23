using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleEvent;
using System.Diagnostics;
using System.Threading.Tasks;
using Test.TestClass;

namespace Test
{
    [TestClass]
    public class EventHandleProviderTest
    {
        public class MsgEventHandler : IEventHandler<MsgEvent>
        {
            public async Task Handle(MsgEvent lostEvent)
            {
                await Task.Run(() => { Debug.WriteLine("MsgEventHandler：" + lostEvent.GetMessage()); });
            }
        }

        public class TalkEventHandler : IEventHandler<MsgEvent>
        {
            public async Task Handle(MsgEvent lostEvent)
            {
                await Task.Run(() => { Debug.WriteLine("TalkEventHandler：" + lostEvent.GetMessage()); });
            }
        }

        public class LostEventHandler : IEventHandler<LostEvent>
        {
            public async Task Handle(LostEvent lostEvent)
            {
                await Task.Run(() => { Debug.WriteLine("LostEventHandler：" + lostEvent.GetLostInfo()); });
            }
        }

        public class LostManEventHandler : IEventHandler<LostEvent>
        {
            public async Task Handle(LostEvent lostEvent)
            {
                await Task.Run(() => { Debug.WriteLine("LostManEventHandler：" + lostEvent.GetLostInfo()); });
            }
        }

        [TestMethod]
        public async Task Test_publish_MsgEvent()
        {
            IEventBus eventBus = new EventBus(this.GenerateEventHandleProvider());
            var msgEvent = new MsgEvent() { Message = "你好" };

            await eventBus.Publish(msgEvent);

            Assert.AreEqual(2, msgEvent.GetMessageExecuteTimes);
        }

        [TestMethod]
        public async Task Test_publish_MsgEvent_and_LostEvent()
        {
            IEventBus eventBus = new EventBus(this.GenerateEventHandleProvider());
            var msgEvent = new MsgEvent() { Message = "你好" };
            var lostEvent = new LostEvent() { LostInfo = "迷失" };

            await eventBus.Publish(msgEvent);
            await eventBus.Publish(lostEvent);

            Assert.AreEqual(2, msgEvent.GetMessageExecuteTimes);
            Assert.AreEqual(2, lostEvent.GetLostInfoExecuteTimes);
        }

        private EventHandleProvider GenerateEventHandleProvider()
        {
            return new EventHandleProvider(
                new MsgEventHandler(),
                new TalkEventHandler(),
                new LostEventHandler(),
                new LostManEventHandler());
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleEvent;
using System.Diagnostics;
using System.Threading.Tasks;
using Moq;
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
            var msgEvent = new MsgEvent() { Message = "你好" };

            IEventBus eventBus = new EventBus(this.GenerateEventHandleProvider());
            await eventBus.Publish(msgEvent);

            Assert.AreEqual(2, msgEvent.GetMessageExecuteTimes);
        }

        [TestMethod]
        public async Task Test_publish_MsgEvent_and_LostEvent()
        {
            var msgEvent = new MsgEvent() { Message = "你好" };
            var lostEvent = new LostEvent() { LostInfo = "迷失" };

            IEventBus eventBus = new EventBus(this.GenerateEventHandleProvider());
            await eventBus.Publish(msgEvent);
            await eventBus.Publish(lostEvent);

            Assert.AreEqual(2, msgEvent.GetMessageExecuteTimes);
            Assert.AreEqual(2, lostEvent.GetLostInfoExecuteTimes);
        }

        [TestMethod]
        public async Task Test_event_publish_succeed()
        {
            Mock<IEventHandler<MsgEvent>> mockMsgEventHandler = new Mock<IEventHandler<MsgEvent>>();
            var testMsgEvent = new MsgEvent() { Message = "test_msg" };

            IEventBus eventBus = new EventBus(new EventHandleProvider(mockMsgEventHandler.Object));
            await eventBus.Publish(testMsgEvent);

            mockMsgEventHandler.Verify(obj => obj.Handle(It.Is<MsgEvent>(msgEvent => msgEvent.Message == testMsgEvent.Message)), Times.Once);
        }

        [TestMethod]
        public async Task Test_multi_event_publish_succeed()
        {
            //mock IEventHandler<MsgEvent>
            Mock<IEventHandler<MsgEvent>> mockMsgEventHandler = new Mock<IEventHandler<MsgEvent>>();
            //mock IEventHandler<LostEvent>
            Mock<IEventHandler<LostEvent>> mockLostEventHandler = new Mock<IEventHandler<LostEvent>>();
            var testMsgEvent = new MsgEvent() { Message = "test_msg" };
            var testLostEvent = new LostEvent() { LostInfo = "test_LostInfo" };

            await new EventBus(new EventHandleProvider(mockMsgEventHandler.Object, mockLostEventHandler.Object))
                .Publish(testMsgEvent)
                .DoAsync(eventBus => eventBus.Publish(testLostEvent))
                .DoAsync(eventBus => eventBus.Publish(testLostEvent));

            mockMsgEventHandler.Verify(obj => obj.Handle(It.Is<MsgEvent>(msgEvent => msgEvent.Message == testMsgEvent.Message)), Times.Once);
            mockLostEventHandler.Verify(obj => obj.Handle(It.Is<LostEvent>(lostEvent => lostEvent.LostInfo == testLostEvent.LostInfo)), Times.Exactly(2));
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

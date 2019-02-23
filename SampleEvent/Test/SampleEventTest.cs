using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleEvent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class SampleEventTest
    {
        public class MsgEvent : ISampleEvent
        {
            private static readonly object LockObject = new object();

            public string Message { get; set; }

            public int GetMessageExecuteTimes { get; private set; }

            public string GetMessage()
            {
                this.GetMessageExecuteTimes++;

                return Message;
            }
        }

        public class LostEvent : ISampleEvent
        {
            private static readonly object LockObject = new object();

            public string LostInfo { get; set; }

            public int GetLostInfoExecuteTimes { get; private set; }

            public string GetLostInfo()
            {
                this.GetLostInfoExecuteTimes++;

                return LostInfo;
            }
        }

        public class MsgEventHandler : IEventHandler<MsgEvent>
        {
            public async Task Handle(MsgEvent msgEvent)
            {
                await Task.Run(() => { Debug.WriteLine("MsgEventHandler：" + msgEvent.GetMessage()); });
            }
        }

        public class TalkEventHandler : IEventHandler<MsgEvent>
        {
            public async Task Handle(MsgEvent msgEvent)
            {
                await Task.Run(() => { Debug.WriteLine("TalkEventHandler：" + msgEvent.GetMessage()); });
            }
        }

        public class LostEventHandler : IEventHandler<LostEvent>
        {
            public async Task Handle(LostEvent msgEvent)
            {
                await Task.Run(() => { Debug.WriteLine("LostEventHandler：" + msgEvent.GetLostInfo()); });
            }
        }

        public class LostManEventHandler : IEventHandler<LostEvent>
        {
            public async Task Handle(LostEvent msgEvent)
            {
                await Task.Run(() => { Debug.WriteLine("LostManEventHandler：" + msgEvent.GetLostInfo()); });
            }
        }

        [TestMethod]
        public async Task Test_publish_MsgEvent()
        {
            IEventBus eventBus = new EventBus(new EventHandleProvider());
            var msgEvent = new MsgEvent() { Message = "你好" };

            await eventBus.Publish(msgEvent);

            Assert.AreEqual(2, msgEvent.GetMessageExecuteTimes);
        }

        [TestMethod]
        public async Task Test_publish_MsgEvent_and_LostEvent()
        {
            IEventBus eventBus = new EventBus(new EventHandleProvider());
            var msgEvent = new MsgEvent() { Message = "你好" };
            var lostEvent = new LostEvent() { LostInfo = "迷失" };

            await eventBus.Publish(msgEvent);
            await eventBus.Publish(lostEvent);

            Assert.AreEqual(2, msgEvent.GetMessageExecuteTimes);
            Assert.AreEqual(2, lostEvent.GetLostInfoExecuteTimes);
        }
    }
}

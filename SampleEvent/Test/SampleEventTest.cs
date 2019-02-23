using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleEvent;

namespace Test
{
    [TestClass]
    public class SampleEventTest
    {
        public class MsgEvent : ISampleEvent
        {
            public string Msg { get; set; }
        }

        public class LostEvent : ISampleEvent
        {
            public string LostInfo { get; set; }
        }

        public class MsgEventHandler : IEventHandler<MsgEvent>
        {
            public void Handle(MsgEvent msgEvent)
            {
                Debug.WriteLine("MsgEventHandler：" + msgEvent.Msg);
            }
        }

        public class TalkEventHandler : IEventHandler<MsgEvent>
        {
            public void Handle(MsgEvent msgEvent)
            {
                Debug.WriteLine("TalkEventHandler：" + msgEvent.Msg);
            }
        }

        public class LostEventHandler : IEventHandler<LostEvent>
        {
            public void Handle(LostEvent msgEvent)
            {
                Debug.WriteLine("LostEventHandler：" + msgEvent.LostInfo);
            }
        }

        public class LostManEventHandler : IEventHandler<LostEvent>
        {
            public void Handle(LostEvent msgEvent)
            {
                Debug.WriteLine("LostManEventHandler：" + msgEvent.LostInfo);
            }
        }

        [TestMethod]
        public void Test_EventHandleProvider()
        {
            IEventBus eventBus = new EventBus(new EventHandleProvider());

            eventBus.Publish(new MsgEvent() { Msg = "你好" });

            eventBus.Publish(new LostEvent() { LostInfo = "迷失" });
        }
    }
}

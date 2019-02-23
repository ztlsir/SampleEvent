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

        public class MsgEventHandler : IEventHandler<MsgEvent>
        {
            public void Handle(MsgEvent msgEvent)
            {
                Debug.Write("MsgEventHandler：" + msgEvent.Msg);
            }
        }

        public class TalkEventHandler : IEventHandler<MsgEvent>
        {
            public void Handle(MsgEvent msgEvent)
            {
                Debug.Write("TalkEventHandler：" + msgEvent.Msg);
            }
        }

        [TestMethod]
        public void Test_EventHandleProvider()
        {
            IEventBus eventBus = new EventBus(new EventHandleProvider());

            eventBus.Publish(new MsgEvent() { Msg = "你好" });
        }
    }
}

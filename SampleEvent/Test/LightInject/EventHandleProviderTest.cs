using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleEvent;
using System.Diagnostics;
using System.Threading.Tasks;
using Moq;
using SampleEvent.LightInject;
using Test.TestClass;
using EventHandleProvider = SampleEvent.LightInject.EventHandleProvider;

namespace Test.LightInject
{
    [TestClass]
    public class EventHandleProviderTest
    {
        public class MsgEventHandler : IEventHandler<MsgEvent>
        {
            private IDoSomething DoSomething { get; }

            public MsgEventHandler(IDoSomething doSomething)
            {
                this.DoSomething = doSomething;
            }

            public async Task Handle(MsgEvent lostEvent)
            {
                await Task.Run(() =>
                {
                    DoSomething.JustDoIt(lostEvent.Message);
                    Debug.WriteLine("MsgEventHandler：" + lostEvent.GetMessage());
                });
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
            private IDoSomething DoSomething { get; }

            public LostEventHandler(IDoSomething doSomething)
            {
                this.DoSomething = doSomething;
            }

            public async Task Handle(LostEvent lostEvent)
            {
                await Task.Run(() =>
                {
                    DoSomething.JustDoIt(lostEvent.LostInfo);
                    Debug.WriteLine("LostEventHandler：" + lostEvent.GetLostInfo());
                });
            }
        }

        public class LostManEventHandler : IEventHandler<LostEvent>
        {
            private IDoSomething DoSomething { get; }

            public LostManEventHandler(IDoSomething doSomething)
            {
                this.DoSomething = doSomething;
            }

            public async Task Handle(LostEvent lostEvent)
            {
                await Task.Run(() =>
                {
                    DoSomething.JustDoIt(lostEvent.LostInfo);
                    Debug.WriteLine("LostManEventHandler：" + lostEvent.GetLostInfo());
                });
            }
        }

        [TestMethod]
        public async Task Test_publish_MsgEvent()
        {
            var container = new ServiceContainer();
            Mock<IDoSomething> mockDoSomething = new Mock<IDoSomething>();
            container.Register<IDoSomething>(factory => mockDoSomething.Object);
            var msgEvent = new MsgEvent() { Message = "你好" };
            SampleEventIOCFactory.RegisterEventHandlersFromAssemblies(container, () => new PerContainerLifetime(),
                this.GetType().Assembly);

            IEventBus eventBus = new EventBus(this.GenerateEventHandleProvider(container));
            await eventBus.Publish(msgEvent);

            mockDoSomething.Verify(obj => obj.JustDoIt(It.Is<string>(text => text == msgEvent.Message)), Times.Once);
        }

        [TestMethod]
        public async Task Test_publish_MsgEvent_and_LostEvent()
        {
            var container = new ServiceContainer();
            Mock<IDoSomething> mockDoSomething = new Mock<IDoSomething>();
            container.Register<IDoSomething>(factory => mockDoSomething.Object);
            var msgEvent = new MsgEvent() { Message = "你好" };
            var lostEvent = new LostEvent() { LostInfo = "迷失" };
            SampleEventIOCFactory.RegisterEventHandlersFromAssemblies(container, () => new PerContainerLifetime(),
                this.GetType().Assembly);

            IEventBus eventBus = new EventBus(this.GenerateEventHandleProvider(container));
            await eventBus.Publish(msgEvent);
            await eventBus.Publish(lostEvent);

            mockDoSomething.Verify(obj => obj.JustDoIt(It.Is<string>(text => text == msgEvent.Message)), Times.Once);
            mockDoSomething.Verify(obj => obj.JustDoIt(It.Is<string>(text => text == lostEvent.LostInfo)), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Test_event_publish_succeed()
        {
            var container = new ServiceContainer();
            container.Register<IDoSomething, DoSomething>();
            //mock and register IEventHandler<MsgEvent>
            Mock<IEventHandler<MsgEvent>> mockDoSomething = new Mock<IEventHandler<MsgEvent>>();
            container.Register<IEventHandler<MsgEvent>>(factory => mockDoSomething.Object);
            var testMsgEvent = new MsgEvent() { Message = "test_msg" };

            IEventBus eventBus = new EventBus(this.GenerateEventHandleProvider(container));
            await eventBus.Publish(testMsgEvent);

            mockDoSomething.Verify(obj => obj.Handle(It.Is<MsgEvent>(msgEvent => msgEvent.Message == testMsgEvent.Message)), Times.Once);
        }

        [TestMethod]
        public async Task Test_multi_event_publish_succeed()
        {
            var container = new ServiceContainer();
            container.Register<IDoSomething, DoSomething>();
            //mock and register IEventHandler<MsgEvent>
            Mock<IEventHandler<MsgEvent>> mockMsgEventHandler = new Mock<IEventHandler<MsgEvent>>();
            container.Register<IEventHandler<MsgEvent>>(factory => mockMsgEventHandler.Object);
            //mock and register IEventHandler<LostEvent>
            Mock<IEventHandler<LostEvent>> mockLostEventHandler = new Mock<IEventHandler<LostEvent>>();
            container.Register<IEventHandler<LostEvent>>(factory => mockLostEventHandler.Object);
            var testMsgEvent = new MsgEvent() { Message = "test_msg" };
            var testLostEvent = new LostEvent() { LostInfo = "test_LostInfo" };

            IEventBus eventBus = new EventBus(this.GenerateEventHandleProvider(container));
            await eventBus.Publish(testMsgEvent);
            await eventBus.Publish(testLostEvent);
            await eventBus.Publish(testLostEvent);

            mockMsgEventHandler.Verify(obj => obj.Handle(It.Is<MsgEvent>(msgEvent => msgEvent.Message == testMsgEvent.Message)), Times.Once);
            mockLostEventHandler.Verify(obj => obj.Handle(It.Is<LostEvent>(lostEvent => lostEvent.LostInfo == testLostEvent.LostInfo)), Times.Exactly(2));
        }

        private EventHandleProvider GenerateEventHandleProvider(ServiceContainer container = null)
        {
            if (container == null)
            {
                container = new ServiceContainer();
            }

            return new EventHandleProvider(container);
        }
    }
}

using SampleEvent;

namespace Test.TestClass
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
}

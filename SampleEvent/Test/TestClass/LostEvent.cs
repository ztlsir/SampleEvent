using SampleEvent;

namespace Test.TestClass
{
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
}

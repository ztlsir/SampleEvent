namespace Test.TestClass
{
    public class DoSomething : IDoSomething
    {
        public string JustDoIt(string text)
        {
            return string.Format("I got some info :{0}", text);
        }
    }
}

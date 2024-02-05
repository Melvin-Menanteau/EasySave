namespace EasySave
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Program program = new Program();
            string result = program.hello();
            Assert.Equal("Hello", result);
        }
    }
}
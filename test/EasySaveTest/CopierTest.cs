using EasySave;
using Xunit;

namespace EasySaveTest
{
    public class CopierTest
    {
        [Fact]
        public void Test1()
        {
            Copier copier = new Copier();
            string result = copier.Hello();

            Assert.Equal("Hello", result);
        }
    }
}

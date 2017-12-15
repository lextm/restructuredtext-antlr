using System.IO;
using ReStructuredText;
using Xunit;

namespace Tests
{
    public class MyClass
    {
        [Fact]
        public void Test()
        {
            var content = "I test";
            var path = Path.GetTempFileName();
            File.WriteAllText(path, content);

            restructuredtextParser.ParseDocument(path);
            Assert.False(true);
        }
    }
}

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
            var content = "I test\n";
            var path = Path.GetTempFileName();
            File.WriteAllText(path, content);

            var document = restructuredtextParser.ParseDocument(path);
            Assert.Equal(1, document.Paragraphs.Count);
            Assert.Equal("I test\n", document.Paragraphs[0].Text);
        }
    }
}

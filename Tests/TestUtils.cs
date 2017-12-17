using System.IO;

namespace ReStructuredText.Tests
{
    public static class TestUtils
    {
        public static Document Test(string fileName)
        {
            var path = Path.Combine("Input", fileName);
            var result = ReStructuredTextParser.ParseDocument(path);
            return result;
        }
    }
}

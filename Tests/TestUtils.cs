using System.IO;

namespace ReStructuredText.Tests
{
    public static class TestUtils
    {
        public static Document Test(string content)
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path, content);

            var result = ReStructuredTextParser.ParseDocument(path);
            File.Delete(path);
            return result;
        }
    }
}

using Xunit;

namespace Lextm.ReStructuredText.Tests
{
    public class FullTest
    {
        [Fact]
        public void TestLine()
        {
            var document = TestUtils.Test("full");

            Assert.Null(document.Find(1, 0));

            Assert.Equal(ElementType.Section, document.Find(2, 0).TypeCode);
            Assert.Equal(ElementType.Section, document.Find(3, 0).TypeCode);
            Assert.Equal(ElementType.Section, document.Find(4, 0).TypeCode);
            Assert.Equal(ElementType.Section, document.Find(5, 0).TypeCode);
            Assert.Equal(ElementType.Paragraph, document.Find(6, 0).TypeCode);
            Assert.Equal(ElementType.Paragraph, document.Find(7, 0).TypeCode);
            Assert.Equal(ElementType.Section, document.Find(8, 0).TypeCode);
            Assert.Equal(ElementType.Paragraph, document.Find(9, 0).TypeCode);
            Assert.Equal(ElementType.ListItem, document.Find(9, 0).Parent.TypeCode);
            Assert.Equal(ElementType.Paragraph, document.Find(10, 0).TypeCode);
            Assert.Equal(ElementType.ListItem, document.Find(10, 0).Parent.TypeCode);

            Assert.Null(document.Find(11, 0));
        }
    }
}

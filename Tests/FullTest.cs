using Xunit;

namespace Lextm.ReStructuredText.Tests
{
    public class FullTest
    {
        [Fact]
        public void LineNumbers()
        {
            var document = TestUtils.Test("full_linenumbers");

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

        [Fact]
        public void LineStart()
        {
            var document = TestUtils.Test("full_linestart");

            IElement item1 = document.Find(1, 0);
            Assert.Equal(ElementType.Paragraph, item1.TypeCode);
            Assert.Equal(ElementType.ListItem, item1.Parent.TypeCode);
            Assert.StartsWith(":doc:", item1.TextAreas[0].Content.Text);

            IElement item2 = document.Find(2, 0);
            Assert.Equal(ElementType.Paragraph, item2.TypeCode);
            Assert.Equal(ElementType.ListItem, item2.Parent.TypeCode);
            Assert.StartsWith(":doc:", item2.TextAreas[0].Content.Text);

            IElement item3 = document.Find(3, 0);
            Assert.Equal(ElementType.Paragraph, item3.TypeCode);
            Assert.Equal(ElementType.ListItem, item3.Parent.TypeCode);
            Assert.StartsWith(":doc:", item3.TextAreas[0].Content.Text);

            IElement item4 = document.Find(4, 0);
            Assert.Equal(ElementType.Paragraph, item4.TypeCode);
            Assert.Equal(ElementType.ListItem, item4.Parent.TypeCode);
            Assert.StartsWith(":doc:", item4.TextAreas[0].Content.Text);

            IElement item5 = document.Find(5, 0);
            Assert.Equal(ElementType.Paragraph, item5.TypeCode);
            Assert.Equal(ElementType.ListItem, item5.Parent.TypeCode);
            Assert.StartsWith(":doc:", item5.TextAreas[0].Content.Text);

            IElement item6 = document.Find(6, 0);
            Assert.Equal(ElementType.Paragraph, item6.TypeCode);
            Assert.Equal(ElementType.ListItem, item6.Parent.TypeCode);
            Assert.StartsWith("doc", ((InterpretedText)item6.TextAreas[0]).RoleName);

            IElement item7 = document.Find(7, 0);
            Assert.Equal(ElementType.Paragraph, item7.TypeCode);
            Assert.Equal(ElementType.ListItem, item7.Parent.TypeCode);
            Assert.StartsWith("doc", ((InterpretedText)item7.TextAreas[0]).RoleName);

            IElement item8 = document.Find(8, 0);
            Assert.Equal(ElementType.Paragraph, item8.TypeCode);
            Assert.Equal(ElementType.ListItem, item8.Parent.TypeCode);
            Assert.StartsWith("doc", ((InterpretedText)item8.TextAreas[0]).RoleName);

            IElement item9 = document.Find(9, 0);
            Assert.Equal(ElementType.Paragraph, item9.TypeCode);
            Assert.Equal(ElementType.ListItem, item9.Parent.TypeCode);
            Assert.StartsWith("doc", ((InterpretedText)item9.TextAreas[0]).RoleName);
        }
    }
}

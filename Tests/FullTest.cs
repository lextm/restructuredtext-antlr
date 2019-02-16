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
            Assert.False(document.TriggerDocumentList(0, 0));

            Assert.Equal(ElementType.Section, document.Find(2, 0).TypeCode);
            Assert.False(document.TriggerDocumentList(1, 0));
            Assert.Equal(ElementType.Section, document.Find(3, 0).TypeCode);
            Assert.False(document.TriggerDocumentList(2, 0));
            Assert.Equal(ElementType.Section, document.Find(4, 0).TypeCode);
            Assert.False(document.TriggerDocumentList(3, 0));
            Assert.Equal(ElementType.Section, document.Find(5, 0).TypeCode);
            Assert.False(document.TriggerDocumentList(4, 0));
            Assert.Equal(ElementType.Paragraph, document.Find(6, 0).TypeCode);
            Assert.False(document.TriggerDocumentList(5, 0));
            Assert.Equal(ElementType.Paragraph, document.Find(7, 0).TypeCode);
            Assert.False(document.TriggerDocumentList(6, 0));
            Assert.Equal(ElementType.Section, document.Find(8, 0).TypeCode);
            Assert.False(document.TriggerDocumentList(7, 0));
            IElement element9 = document.Find(9, 0);
            Assert.Equal(ElementType.Paragraph, element9.TypeCode);
            Assert.True(document.TriggerDocumentList(8, 0));
            Assert.Equal(ElementType.ListItem, element9.Parent.TypeCode);
            IElement element10 = document.Find(10, 0);
            Assert.Equal(ElementType.Paragraph, element10.TypeCode);
            Assert.True(document.TriggerDocumentList(9, 0));
            Assert.Equal(ElementType.ListItem, element10.Parent.TypeCode);

            Assert.Equal(ElementType.Section, document.Find(11, 0).TypeCode);
            Assert.False(document.TriggerDocumentList(10, 0));

            Assert.Equal(ElementType.Paragraph, document.Find(12, 0).TypeCode);
            Assert.True(document.TriggerDocumentList(11, 0));

            Assert.Null(document.Find(13, 0));
            Assert.False(document.TriggerDocumentList(12, 0));
        }

        [Fact]
        public void LineStart()
        {
            var document = TestUtils.Test("full_linestart");

            IElement item1 = document.Find(1, 0);
            Assert.Equal(ElementType.Paragraph, item1.TypeCode);
            Assert.Equal(ElementType.ListItem, item1.Parent.TypeCode);
            Assert.StartsWith(":doc:", item1.TextAreas[0].Content.Text);
            Assert.True(document.TriggerDocumentList(0, 0));

            IElement item2 = document.Find(2, 0);
            Assert.Equal(ElementType.Paragraph, item2.TypeCode);
            Assert.Equal(ElementType.ListItem, item2.Parent.TypeCode);
            Assert.StartsWith(":doc:", item2.TextAreas[0].Content.Text);
            Assert.True(document.TriggerDocumentList(1, 0));

            IElement item3 = document.Find(3, 0);
            Assert.Equal(ElementType.Paragraph, item3.TypeCode);
            Assert.Equal(ElementType.ListItem, item3.Parent.TypeCode);
            Assert.StartsWith(":doc:", item3.TextAreas[0].Content.Text);
            Assert.True(document.TriggerDocumentList(2, 0));

            IElement item4 = document.Find(4, 0);
            Assert.Equal(ElementType.Paragraph, item4.TypeCode);
            Assert.Equal(ElementType.ListItem, item4.Parent.TypeCode);
            Assert.StartsWith(":doc:", item4.TextAreas[0].Content.Text);
            Assert.True(document.TriggerDocumentList(3, 0));

            IElement item5 = document.Find(5, 0);
            Assert.Equal(ElementType.Paragraph, item5.TypeCode);
            Assert.Equal(ElementType.ListItem, item5.Parent.TypeCode);
            Assert.StartsWith(":doc:", item5.TextAreas[0].Content.Text);
            Assert.True(document.TriggerDocumentList(4, 0));

            IElement item6 = document.Find(6, 0);
            Assert.Equal(ElementType.Paragraph, item6.TypeCode);
            Assert.Equal(ElementType.ListItem, item6.Parent.TypeCode);
            Assert.StartsWith("doc", ((InterpretedText)item6.TextAreas[0]).RoleName);
            Assert.True(document.TriggerDocumentList(5, 0));

            IElement item7 = document.Find(7, 0);
            Assert.Equal(ElementType.Paragraph, item7.TypeCode);
            Assert.Equal(ElementType.ListItem, item7.Parent.TypeCode);
            Assert.StartsWith("doc", ((InterpretedText)item7.TextAreas[0]).RoleName);
            Assert.True(document.TriggerDocumentList(6, 0));

            IElement item8 = document.Find(8, 0);
            Assert.Equal(ElementType.Paragraph, item8.TypeCode);
            Assert.Equal(ElementType.ListItem, item8.Parent.TypeCode);
            Assert.StartsWith("doc", ((InterpretedText)item8.TextAreas[0]).RoleName);
            Assert.True(document.TriggerDocumentList(7, 0));

            IElement item9 = document.Find(9, 0);
            Assert.Equal(ElementType.Paragraph, item9.TypeCode);
            Assert.Equal(ElementType.ListItem, item9.Parent.TypeCode);
            Assert.StartsWith("doc", ((InterpretedText)item9.TextAreas[0]).RoleName);
            Assert.True(document.TriggerDocumentList(8, 0));
        }
    }
}

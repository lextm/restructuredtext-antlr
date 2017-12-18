using Xunit;

namespace ReStructuredText.Tests
{
    public class InlineMarkupLiteralTest
    {
        [Fact]
        public void Simple()
        {
            var document = TestUtils.Test("inlinemarkupliteral_simple");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(2, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[0].ElementType);
            var literal = (Literal) paragraph.TextAreas[0];
            Assert.Equal("literal", literal.Content.Text);
            var text = paragraph.TextAreas[1];
            Assert.Equal("\n", text.Content.Text);
        }
        
        [Fact]
        public void BackSlash()
        {
            var document = TestUtils.Test("inlinemarkupliteral_backslash");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(2, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[0].ElementType);
            var literal = (Literal) paragraph.TextAreas[0];
            Assert.Equal("\\\\literal", literal.Content.Text);
            var text = paragraph.TextAreas[1];
            Assert.Equal("\n", text.Content.Text);
        }
        
        [Fact]
        public void BackSlashInside()
        {
            var document = TestUtils.Test("inlinemarkupliteral_backslash_inside");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(2, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[0].ElementType);
            var literal = (Literal) paragraph.TextAreas[0];
            Assert.Equal("lite\\\\ral", literal.Content.Text);
            var text = paragraph.TextAreas[1];
            Assert.Equal("\n", text.Content.Text);
        }
        
        [Fact]
        public void BackSlashEnd()
        {
            var document = TestUtils.Test("inlinemarkupliteral_backslash_end");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(2, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[0].ElementType);
            var literal = (Literal) paragraph.TextAreas[0];
            Assert.Equal("literal\\\\", literal.Content.Text);
            var text = paragraph.TextAreas[1];
            Assert.Equal("\n", text.Content.Text);
        }
        
        [Fact]
        public void Apostrophe()
        {
            var document = TestUtils.Test("inlinemarkupliteral_apostrophe");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(5, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[1].ElementType);
            var literal = (Literal) paragraph.TextAreas[1];
            Assert.Equal("literal", literal.Content.Text);
            var text = paragraph.TextAreas[2];
            Assert.Equal(" and l\u2019", text.Content.Text);
        }
    }
}
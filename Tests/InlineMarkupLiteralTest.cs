using Xunit;

namespace Lextm.ReStructuredText.Tests
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
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[0].TypeCode);
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
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[0].TypeCode);
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
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[0].TypeCode);
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
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[0].TypeCode);
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
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[1].TypeCode);
            var literal = (Literal) paragraph.TextAreas[1];
            Assert.Equal("literal", literal.Content.Text);
            var text = paragraph.TextAreas[2];
            Assert.Equal(" and l\u2019", text.Content.Text);
        }
        
        [Fact]
        public void Quoted()
        {
            var document = TestUtils.Test("inlinemarkupliteral_quoted");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(13, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.Text, paragraph.TextAreas[0].TypeCode);
            var text = (TextArea) paragraph.TextAreas[0];
            Assert.Equal("quoted '", text.Content.Text);
            
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[1].TypeCode);
            var literal = (Literal) paragraph.TextAreas[1];
            Assert.Equal("literal", literal.Content.Text);
            
            Assert.Equal("', quoted \"", paragraph.TextAreas[2].Content.Text);
            Assert.Equal("\"\n", paragraph.TextAreas[4].Content.Text);
            Assert.Equal("quoted \u2018", paragraph.TextAreas[5].Content.Text);
            Assert.Equal("\u2019, quoted \u201c", paragraph.TextAreas[7].Content.Text);
            Assert.Equal("\u201d,\n", paragraph.TextAreas[9].Content.Text);
            Assert.Equal("quoted \xab", paragraph.TextAreas[10].Content.Text);
            Assert.Equal("\xbb\n", paragraph.TextAreas[12].Content.Text);
        }
        
        [Fact]
        public void Quote()
        {
            var document = TestUtils.Test("inlinemarkupliteral_quote");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(10, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[0].TypeCode);
            var text = (Literal) paragraph.TextAreas[0];
            Assert.Equal("'literal'", text.Content.Text);
            
            Assert.Equal(ElementType.Text, paragraph.TextAreas[1].TypeCode);
            var literal = (TextArea) paragraph.TextAreas[1];
            Assert.Equal(" with quotes, ", literal.Content.Text);
            
            Assert.Equal("\"literal\"", paragraph.TextAreas[2].Content.Text);
            Assert.Equal(" with quotes,\n", paragraph.TextAreas[3].Content.Text);
            Assert.Equal("\u2018literal\u2019", paragraph.TextAreas[4].Content.Text);
            Assert.Equal("\u201cliteral\u201d", paragraph.TextAreas[6].Content.Text);
            Assert.Equal("\xabliteral\xbb", paragraph.TextAreas[8].Content.Text);
        }
        
        [Fact]
        public void InterpretedText()
        {
            var document = TestUtils.Test("inlinemarkupliteral_interpretedtext");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(3, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[1].TypeCode);
            var text = (Literal) paragraph.TextAreas[1];
            Assert.Equal("`interpreted text`", text.Content.Text);
        }
        
        [Fact]
        public void EndsWithBackslash()
        {
            var document = TestUtils.Test("inlinemarkupliteral_endswithbackslash");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(3, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.Literal, paragraph.TextAreas[1].TypeCode);
            var text = (Literal) paragraph.TextAreas[1];
            Assert.Equal("list", text.Content.Text);
            
            Assert.Equal("\\s use square bracket syntax.\n", paragraph.TextAreas[2].Content.Text);
        }
    }
}
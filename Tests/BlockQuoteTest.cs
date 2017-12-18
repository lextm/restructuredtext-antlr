using Xunit;

namespace ReStructuredText.Tests
{
    public class BlockQuoteTest
    {
        [Fact]
        public void Simple()
        {
            var document = TestUtils.Test("blockquote_simple");
            Assert.Equal(2, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Line 1.\n", paragraph.TextAreas[0].Content.Text);
            Assert.Equal("Line 2.\n", paragraph.TextAreas[1].Content.Text);
            BlockQuote element = (BlockQuote)document.Elements[1];
            Assert.True(element.TypeCode == ElementType.BlockQuote);
            Paragraph paragraph3 = (Paragraph)element.Elements[0];
            Assert.Equal("Indented.\n", paragraph3.TextAreas[0].Content.Text);
        }

        [Fact]
        public void Nested()
        {
            var document = TestUtils.Test("blockquote_nested");
            Assert.Equal(2, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Line 1.\n", paragraph.TextAreas[0].Content.Text);
            Assert.Equal("Line 2.\n", paragraph.TextAreas[1].Content.Text);
            BlockQuote element = (BlockQuote)document.Elements[1];
            Assert.True(element.TypeCode == ElementType.BlockQuote);
            Paragraph paragraph2 = (Paragraph)element.Elements[0];
            Assert.Equal("Indented 1.\n", paragraph2.TextAreas[0].Content.Text);
            BlockQuote blockQuote = (BlockQuote)element.Elements[1];
            Assert.Equal(ElementType.BlockQuote, blockQuote.TypeCode);
            Paragraph paragraph3 = (Paragraph)blockQuote.Elements[0];
            Assert.Equal("Indented 2.\n", paragraph3.TextAreas[0].Content.Text);
        }

        [Fact]
        public void Nested2()
        {
            var document = TestUtils.Test("blockquote_nested2");
            Assert.Equal(3, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Here is a paragraph.\n", paragraph.TextAreas[0].Content.Text);
            BlockQuote element = (BlockQuote)document.Elements[1];
            Assert.True(element.TypeCode == ElementType.BlockQuote);
            BlockQuote block2 = (BlockQuote)element.Elements[0];
            Assert.Equal("Indent 8 spaces.\n", block2.TextAreas[0].Content.Text);
            Paragraph paragraph3 = (Paragraph)element.Elements[1];
            Assert.Equal(ElementType.Paragraph, paragraph3.TypeCode);
            Assert.Equal("Indent 4 spaces.\n", paragraph3.TextAreas[0].Content.Text);
            Assert.Equal("Is this correct? Should it generate a warning?\n", document.Elements[2].TextAreas[0].Content.Text);
            Assert.Equal("Yes, it is correct, no warning necessary.\n", document.Elements[2].TextAreas[1].Content.Text);
        } 
    }
}

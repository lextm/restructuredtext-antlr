using Xunit;

namespace ReStructuredText.Tests
{
    public class BlockQuoteTest
    {
        [Fact]
        public void Simple()
        {
            var document = TestUtils.Test("\nLine 1.\nLine 2.\n\n   Indented.\n");
            Assert.Equal(2, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Line 1.\n", paragraph.Lines[0].Text.Content);
            Assert.Equal("Line 2.\n", paragraph.Lines[1].Text.Content);
            BlockQuote element = (BlockQuote)document.Elements[1];
            Assert.True(element.TypeCode == ElementType.BlockQuote);
            Paragraph paragraph3 = (Paragraph)element.Content[0];
            Assert.Equal("Indented.\n", paragraph3.Lines[0].Text.Content);
        }

        [Fact]
        public void Nested()
        {
            var document = TestUtils.Test("\nLine 1.\nLine 2.\n\n   Indented 1.\n\n      Indented 2.\n");
            Assert.Equal(2, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Line 1.\n", paragraph.Lines[0].Text.Content);
            Assert.Equal("Line 2.\n", paragraph.Lines[1].Text.Content);
            BlockQuote element = (BlockQuote)document.Elements[1];
            Assert.True(element.TypeCode == ElementType.BlockQuote);
            Paragraph paragraph2 = (Paragraph)element.Content[0];
            Assert.Equal("Indented 1.\n", paragraph2.Lines[0].Text.Content);
            BlockQuote blockQuote = (BlockQuote)element.Content[1];
            Assert.Equal(ElementType.BlockQuote, blockQuote.TypeCode);
            Paragraph paragraph3 = (Paragraph)blockQuote.Content[0];
            Assert.Equal("Indented 2.\n", paragraph3.Lines[0].Text.Content);
        }

        [Fact]
        public void Nested2()
        {
            var document = TestUtils.Test("\nHere is a paragraph.\n\n        Indent 8 spaces.\n\n    Indent 4 spaces.\n\nIs this correct? Should it generate a warning?\nYes, it is correct, no warning necessary.\n");
            Assert.Equal(3, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Here is a paragraph.\n", paragraph.Lines[0].Text.Content);
            BlockQuote element = (BlockQuote)document.Elements[1];
            Assert.True(element.TypeCode == ElementType.BlockQuote);
            BlockQuote block2 = (BlockQuote)element.Content[0];
            Assert.Equal("Indent 8 spaces.\n", block2.Lines[0].Text.Content);
            Paragraph paragraph3 = (Paragraph)element.Content[1];
            Assert.Equal(ElementType.Paragraph, paragraph3.TypeCode);
            Assert.Equal("Indent 4 spaces.\n", paragraph3.Lines[0].Text.Content);
            Assert.Equal("Is this correct? Should it generate a warning?\n", document.Elements[2].Lines[0].Text.Content);
            Assert.Equal("Yes, it is correct, no warning necessary.\n", document.Elements[2].Lines[1].Text.Content);
        } 
    }
}

using System.Linq;
using Xunit;

namespace ReStructuredText.Tests
{
    public class ParagraphTest
    {
        [Fact]
        public void Empty()
        {
            var document = TestUtils.Test("\n");
            Assert.Equal(0, document.Elements.Count);
        }

        [Fact]
        public void SingleLine()
        {
            // TODO: see how to resolve this.
            var document = TestUtils.Test("A paragraph.");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal("A paragraph.<missing LineBreak>", document.Elements[0].Lines[0].Text.Content);
        }

        [Fact]
        public void SingleLineWithNewLine()
        {
            var document = TestUtils.Test("A paragraph.\n");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal("A paragraph.\n", document.Elements[0].Lines[0].Text.Content);
        }

        [Fact]
        public void MultipleLines()
        {
            var document = TestUtils.Test("\nA paragraph.\n");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal("A paragraph.\n", document.Elements[0].Lines[0].Text.Content);
        }

        [Fact]
        public void MultipleSingleLines()
        {
            var document = TestUtils.Test("\nParagraph 1.\n\nParagraph 2.\n");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal("Paragraph 1.\n", document.Elements[0].Lines[0].Text.Content);
            Assert.Equal("Paragraph 2.\n", document.Elements[1].Lines[0].Text.Content);
        }

        [Fact]
        public void MultilineParagraph()
        {
            var document = TestUtils.Test("\nLine 1.\nLine 2.\nLine 3.\n");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal("Line 1.\n", document.Elements[0].Lines[0].Text.Content);
            Assert.Equal("Line 2.\n", document.Elements[0].Lines[1].Text.Content);
            Assert.Equal("Line 3.\n", document.Elements[0].Lines[2].Text.Content);
        }

        [Fact]
        public void MultilineParagraphs()
        {
            var document = TestUtils.Test("\nParagraph 1, Line 1.\nLine 2.\nLine 3.\n\nParagraph 2, Line 1.\nLine 2.\nLine 3.\n");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal("Paragraph 1, Line 1.\n", document.Elements[0].Lines[0].Text.Content);
            Assert.Equal("Line 2.\n", document.Elements[0].Lines[1].Text.Content);
            Assert.Equal("Line 3.\n", document.Elements[0].Lines[2].Text.Content);
            Assert.Equal("Paragraph 2, Line 1.\n", document.Elements[1].Lines[0].Text.Content);
            Assert.Equal("Line 2.\n", document.Elements[1].Lines[1].Text.Content);
            Assert.Equal("Line 3.\n", document.Elements[1].Lines[2].Text.Content);
        }
        
        [Fact]
        public void SimpleParagraphs()
        {
            var document = TestUtils.Test("\nA. Einstein was a really\nsmart dude.\n");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal("A. Einstein was a really\n", document.Elements[0].Lines[0].Text.Content);
            Assert.Equal("smart dude.\n", document.Elements[0].Lines[1].Text.Content);
        }
    }
}

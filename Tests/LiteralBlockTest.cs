using Xunit;

namespace Lextm.ReStructuredText.Tests
{
    public class LiteralBlockTest
    {
        [Fact]
        public void Minimized()
        {
            var document = TestUtils.Test("literalblock_minimized");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal("A paragraph", document.Elements[0].TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.LiteralBlock, document.Elements[1].TypeCode);
            Assert.Equal("A literal block.", document.Elements[1].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void MinimizedAndSpace()
        {
            var document = TestUtils.Test("literalblock_minimized_space");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal("A paragraph with a space after the colons", document.Elements[0].TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.LiteralBlock, document.Elements[1].TypeCode);
            Assert.Equal("A literal block.", document.Elements[1].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Two()
        {
            var document = TestUtils.Test("literalblock_two");
            Assert.Equal(5, document.Elements.Count);
            Assert.Equal("A paragraph", document.Elements[0].TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.LiteralBlock, document.Elements[1].TypeCode);
            Assert.Equal("A literal block.", document.Elements[1].TextAreas[0].Content.Text);
            
            Assert.Equal("Another paragraph", document.Elements[2].TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.LiteralBlock, document.Elements[3].TypeCode);
            Assert.Equal("Another literal block.", document.Elements[3].TextAreas[0].Content.Text);
            Assert.Equal("With two blank lines following.", document.Elements[3].TextAreas[1].Content.Text);
            
            Assert.Equal("A final paragraph.\n", document.Elements[4].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Multiline()
        {
            var document = TestUtils.Test("literalblock_multiline");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal("A paragraph\n", document.Elements[0].TextAreas[0].Content.Text);
            Assert.Equal("on more than\n", document.Elements[0].TextAreas[1].Content.Text);
            Assert.Equal("one line", document.Elements[0].TextAreas[2].Content.Text);
            
            Assert.Equal(ElementType.LiteralBlock, document.Elements[1].TypeCode);
            Assert.Equal("A literal block.", document.Elements[1].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Partial()
        {
            var document = TestUtils.Test("literalblock_partial");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal("A paragraph: ", document.Elements[0].TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.LiteralBlock, document.Elements[1].TypeCode);
            Assert.Equal("A literal block.", document.Elements[1].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Expanded()
        {
            var document = TestUtils.Test("literalblock_expanded");
            Assert.Equal(2, document.Elements.Count);
            // TODO: should end with \n
            // Assert.Equal("A paragraph:\n", document.Elements[0].TextAreas[0].Text.Content);
            Assert.Equal("A paragraph:\n\n", document.Elements[0].TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.LiteralBlock, document.Elements[1].TypeCode);
            Assert.Equal("A literal block.", document.Elements[1].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Wonky()
        {
            var document = TestUtils.Test("literalblock_wonky");
            Assert.Equal(2, document.Elements.Count);
            // TODO: should end with \n
            // Assert.Equal("A paragraph:\n", document.Elements[0].TextAreas[0].Text.Content);
            Assert.Equal("A paragraph", document.Elements[0].TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.LiteralBlock, document.Elements[1].TypeCode);
            Assert.Equal("  A wonky literal block.", document.Elements[1].TextAreas[0].Content.Text);
            Assert.Equal("Literal line 2.", document.Elements[1].TextAreas[1].Content.Text);
            Assert.Equal("  Literal line 3.", document.Elements[1].TextAreas[2].Content.Text);
        }
        
        [Fact]
        public void Quoted()
        {
            var document = TestUtils.Test("literalblock_quoted");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal("A paragraph", document.Elements[0].TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.LiteralBlock, document.Elements[1].TypeCode);
            Assert.Equal("> A literal block.", document.Elements[1].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void QuotedTwoBlankLine()
        {
            var document = TestUtils.Test("literalblock_quoted_twoblanklines");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal("A paragraph", document.Elements[0].TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.LiteralBlock, document.Elements[1].TypeCode);
            Assert.Equal("> A literal block.", document.Elements[1].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void QuotedMultiline()
        {
            var document = TestUtils.Test("literalblock_quoted_multiline");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal("A paragraph", document.Elements[0].TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.LiteralBlock, document.Elements[1].TypeCode);
            Assert.Equal("> A literal block.", document.Elements[1].TextAreas[0].Content.Text);
            Assert.Equal("> Line 2.", document.Elements[1].TextAreas[1].Content.Text);
        }
    }
}
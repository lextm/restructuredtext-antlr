using Xunit;

namespace ReStructuredText.Tests
{
    public class LineBlockTest
    {
        [Fact]
        public void Simple()
        {
            var document = TestUtils.Test("lineblock_simple");
            Assert.Equal(1, document.Elements.Count);
            var block = (LineBlock) document.Elements[0];
            Assert.Equal("This is a line block.", block.TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Multiple()
        {
            var document = TestUtils.Test("lineblock_multiple");
            Assert.Equal(3, document.Elements.Count);
            var block = (LineBlock) document.Elements[0];
            Assert.Equal("This is a line block.", block.TextAreas[0].Content.Text);
            var line = (Line) block.Elements[1];
            Assert.Equal("Line breaks are ", line.TextAreas[0].Content.Text);
            Assert.Equal("preserved", line.TextAreas[1].Content.Text);
            Assert.Equal(".", line.TextAreas[2].Content.Text);
            
            Assert.Equal("This is a second line block.", document.Elements[1].TextAreas[0].Content.Text);
            Assert.Equal("This is a third.", document.Elements[2].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void InitialIndentation()
        {
            var document = TestUtils.Test("lineblock_indentation");
            
            Assert.Equal(1, document.Elements.Count);
            var block = (LineBlock) document.Elements[0];
            var block1 = (Line) block.Elements[0];
            Assert.Equal("In line blocks,", block1.TextAreas[0].Content.Text);
            var child = (LineBlock) block.Elements[1];
            Assert.Equal(ElementType.Text, child.TextAreas[0].TypeCode);
            Assert.Equal("Initial indentation is ", child.TextAreas[0].Content.Text);
            Assert.Equal(ElementType.Emphasis, child.TextAreas[1].TypeCode);
            Assert.Equal("also", child.TextAreas[1].Content.Text);
            Assert.Equal(ElementType.Text, child.TextAreas[2].TypeCode);
            Assert.Equal(" preserved.", child.TextAreas[2].Content.Text);
        }
    }
}
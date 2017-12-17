using Xunit;

namespace ReStructuredText.Tests
{
    public class LineBlockTest
    {
        [Fact]
        public void Simple()
        {
            var document = TestUtils.Test("\n| This is a line block.\n");
            Assert.Equal(1, document.Elements.Count);
            var block = (LineBlock) document.Elements[0];
            Assert.Equal("This is a line block.", block.Lines[0].Text.Content);
        }
        
        [Fact]
        public void Multiple()
        {
            var document = TestUtils.Test("\n| This is a line block.\n| Line breaks are *preserved*.\n\n| This is a second line block.\n\n| This is a third.\n");
            Assert.Equal(3, document.Elements.Count);
            var block = (LineBlock) document.Elements[0];
            Assert.Equal("This is a line block.", block.Lines[0].Text.Content);
            Assert.Equal("Line breaks are *preserved*.", block.Lines[1].Text.Content);
            
            Assert.Equal("This is a second line block.", document.Elements[1].Lines[0].Text.Content);
            Assert.Equal("This is a third.", document.Elements[2].Lines[0].Text.Content);
        }
        
        [Fact]
        public void InitialIndentation()
        {
            var document = TestUtils.Test("\n| In line blocks,\n|     Initial indentation is also *preserved*.\n");
            Assert.Equal(1, document.Elements.Count);
            var block = (LineBlock) document.Elements[0];
            Assert.Equal("In line blocks,", block.Lines[0].Text.Content);
            var block1 = (LineBlock) block.Content[0];
            // TODO:
            Assert.Equal("Initial indentation is also *preserved*.", block1.Lines[0].Text.Content);
        }
    }
}
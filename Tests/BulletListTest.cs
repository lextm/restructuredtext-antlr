using Xunit;

namespace ReStructuredText.Tests
{
    public class BulletListTest
    {
        [Fact]
        public void Single()
        {
            var document = TestUtils.Test("\n- item\n");
            Assert.Equal(1, document.Elements.Count);

            var list = (BulletList) document.Elements[0];
            Assert.Equal(1, list.Items.Count);
            Assert.Equal("item\n", list.Items[0].Elements[0].Lines[0].Text.Content);
        }
        
        [Fact]
        public void Multiple()
        {
            var document = TestUtils.Test("\n* item 1\n\n* item 2\n");
            Assert.Equal(1, document.Elements.Count);

            var list = (BulletList) document.Elements[0];
            Assert.Equal(2, list.Items.Count);
            Assert.Equal('*', list.Start);
            Assert.Equal("item 1\n", list.Items[0].Elements[0].Lines[0].Text.Content);
            Assert.Equal("item 2\n", list.Items[1].Elements[0].Lines[0].Text.Content);
        }
        
        [Fact]
        public void MultipleNoBlankLine()
        {
            var document = TestUtils.Test("\nNo blank line between:\n\n+ item 1\n+ item 2\n");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal("No blank line between:\n", document.Elements[0].Lines[0].Text.Content);

            var list = (BulletList) document.Elements[1];
            Assert.Equal(2, list.Items.Count);
            Assert.Equal('+', list.Start);
            Assert.Equal("item 1\n", list.Items[0].Elements[0].Lines[0].Text.Content);
            Assert.Equal("item 2\n", list.Items[1].Elements[0].Lines[0].Text.Content);
        }

        [Fact]
        public void MultiLineBody()
        {
            var document = TestUtils.Test("\n- item 1, para 1.\n\n  item 1, para 2.\n\n- item 2\n");
            Assert.Equal(1, document.Elements.Count);

            var list = (BulletList)document.Elements[0];
            Assert.Equal(2, list.Items.Count);

            var item = list.Items[0];
            Assert.Equal(2, item.Elements.Count);
            
            Assert.Equal("item 1, para 1.\n", item.Elements[0].Lines[0].Text.Content);
            Assert.Equal("item 1, para 2.\n", item.Elements[1].Lines[0].Text.Content);
            
            Assert.Equal("item 2\n", list.Items[1].Elements[0].Lines[0].Text.Content);
        }
        
        [Fact]
        public void MultiLineBodyWithNoBlankLine()
        {
            var document = TestUtils.Test("\n- item 1, para 1.\n  item 1, para 2.\n- item 2\n");
            Assert.Equal(1, document.Elements.Count);

            var list = (BulletList)document.Elements[0];
            Assert.Equal(2, list.Items.Count);

            var item = list.Items[0];
            Assert.Equal(1, item.Elements.Count);
            
            Assert.Equal("item 1, para 1.\n", item.Elements[0].Lines[0].Text.Content);
            Assert.Equal("item 1, para 2.\n", item.Elements[0].Lines[1].Text.Content);
            
            Assert.Equal("item 2\n", list.Items[1].Elements[0].Lines[0].Text.Content);
        }
        
        [Fact]
        public void DifferentMarker()
        {
            var document = TestUtils.Test("\nDifferent bullets:\n\n- item 1\n\n+ item 2\n\n* item 3\n- item 4\n");
            Assert.Equal(5, document.Elements.Count);

            Assert.Equal("item 1\n", ((BulletList)document.Elements[1]).Items[0].Elements[0].Lines[0].Text.Content);
            Assert.Equal("item 2\n", ((BulletList)document.Elements[2]).Items[0].Elements[0].Lines[0].Text.Content);
            Assert.Equal("item 3\n", ((BulletList)document.Elements[3]).Items[0].Elements[0].Lines[0].Text.Content);
            Assert.Equal("item 4\n", ((BulletList)document.Elements[4]).Items[0].Elements[0].Lines[0].Text.Content);
        }
        
        [Fact]
        public void EmptyItemAbove()
        {
            var document = TestUtils.Test("\n-\n\nempty item above\n");
            // TODO: Read the spec to know if this is supported.
            Assert.Equal(2, document.Elements.Count);
        }

        [Fact]
        public void Unicode()
        {
            var document =
                TestUtils.Test(
                    "\nUnicode bullets:\n\n\u2022 BULLET\n\n\u2023 TRIANGULAR BULLET\n\n\u2043 HYPHEN BULLET\n\n");
            // TODO: read the spec to confirm the characters.
            Assert.Equal(4, document.Elements.Count);
        }

        [Fact]
        public void BeginWithNewLine()
        {
            var document = TestUtils.Test("\n-\n   1\n-\n  2\n-\n 3\n");
            // TODO: read the spec to confirm this is supported.
            Assert.Equal(1, document.Elements.Count);
        }
    }
}
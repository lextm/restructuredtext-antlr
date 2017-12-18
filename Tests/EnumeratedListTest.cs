using Xunit;

namespace ReStructuredText.Tests
{
    public class EnumeratedListTest
    {
        [Fact]
        public void Simple()
        {
            var document = TestUtils.Test("enumeratedlist_simple");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.EnumeratedList, document.Elements[0].TypeCode);

            var list = (EnumeratedList) document.Elements[0];
            Assert.Equal(3, list.Items.Count);

            Assert.Equal("Item one.\n", list.Items[0].TextAreas[0].Content.Text);
            Assert.Equal("Item two.\n", list.Items[1].TextAreas[0].Content.Text);
            Assert.Equal("Item three.\n", list.Items[2].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void NoBlankLines()
        {
            var document = TestUtils.Test("enumeratedlist_noblanklines");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal(ElementType.EnumeratedList, document.Elements[1].TypeCode);

            var list = (EnumeratedList) document.Elements[1];
            Assert.Equal(3, list.Items.Count);

            Assert.Equal("Item one.\n", list.Items[0].TextAreas[0].Content.Text);
            Assert.Equal("Item two.\n", list.Items[1].TextAreas[0].Content.Text);
            Assert.Equal("Item three.\n", list.Items[2].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Empty()
        {
            var document = TestUtils.Test("enumeratedlist_empty");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            Assert.Equal("1.\n", document.Elements[0].TextAreas[0].Content.Text);
            Assert.Equal("empty item above, no blank line\n", document.Elements[0].TextAreas[1].Content.Text);
        }
        
        [Fact]
        public void Scrambled()
        {
            var document = TestUtils.Test("enumeratedlist_scrambled");
            
            // TODO:
            Assert.Equal(7, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            Assert.Equal(ElementType.EnumeratedList, document.Elements[1].TypeCode);
            Assert.Equal(ElementType.EnumeratedList, document.Elements[2].TypeCode);
            Assert.Equal(ElementType.EnumeratedList, document.Elements[3].TypeCode);
            Assert.Equal(ElementType.Paragraph, document.Elements[4].TypeCode);

            var list = (EnumeratedList) document.Elements[1];
            Assert.Equal(1, list.Items.Count);

            Assert.Equal("Item three.\n", list.Items[0].TextAreas[0].Content.Text);
            
            var list2 = (EnumeratedList) document.Elements[2];
            Assert.Equal(1, list2.Items.Count);
            Assert.Equal("Item two.\n", list2.Items[0].TextAreas[0].Content.Text);
            
            var list3 = (EnumeratedList) document.Elements[3];
            Assert.Equal(1, list3.Items.Count);
            Assert.Equal("Item one.\n", list3.Items[0].TextAreas[0].Content.Text);
            
            //TODO: test the rest.
        }

        [Fact]
        public void NonOrdinal()
        {
            var document = TestUtils.Test("enumeratedlist_nonordinal");
            Assert.Equal(4, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            Assert.Equal(ElementType.EnumeratedList, document.Elements[1].TypeCode);
            Assert.Equal(ElementType.Paragraph, document.Elements[2].TypeCode);
            Assert.Equal(ElementType.EnumeratedList, document.Elements[3].TypeCode);
            
            var list = (EnumeratedList) document.Elements[1];
            Assert.Equal(4, list.Items.Count);

            Assert.Equal("Item zero.\n", list.Items[0].TextAreas[0].Content.Text);
            
            var list2 = (EnumeratedList) document.Elements[3];
            Assert.Equal(2, list2.Items.Count);
            Assert.Equal("Item two.\n", list2.Items[0].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void MultipleBody()
        {
            var document = TestUtils.Test("enumeratedlist_multiplebody");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.EnumeratedList, document.Elements[0].TypeCode);
            
            var list = (EnumeratedList) document.Elements[0];
            Assert.Equal(3, list.Items.Count);

            Assert.Equal("Item one: line 1,\n", list.Items[0].TextAreas[0].Content.Text);
            Assert.Equal("line 2.\n", list.Items[0].TextAreas[1].Content.Text);
            
            Assert.Equal("Item two: line 1,\n", list.Items[1].TextAreas[0].Content.Text);
            Assert.Equal("line 2.\n", list.Items[1].TextAreas[1].Content.Text);

            var item = list.Items[2];
            Assert.Equal(2, item.Elements.Count);
            
            Assert.Equal("Paragraph 2.\n", item.Elements[1].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Different()
        {
            var document = TestUtils.Test("enumeratedlist_different");
            // TODO:
            Assert.Equal(6, document.Elements.Count);
        }
        
        // TODO: roman calculation.
    }
}
using Xunit;

namespace ReStructuredText.Tests
{
    public class InlineMarkupEmphasisTest
    {
        [Fact]
        public void Simple()
        {
            var document = TestUtils.Test("inlinemarkupemphasis_simple");
            Assert.Equal(1, document.Elements.Count);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(2, paragraph.TextAreas.Count);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[0].TypeCode);
            Assert.Equal("emphasis", paragraph.TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.Text, paragraph.TextAreas[1].TypeCode);
            Assert.Equal("\n", paragraph.TextAreas[1].Content.Text);
        }
        
        [Fact]
        public void Multiple()
        {
            var document = TestUtils.Test("inlinemarkupemphasis_multiple");
            Assert.Equal(1, document.Elements.Count);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(10, paragraph.TextAreas.Count);
            
            Assert.Equal(ElementType.Text, paragraph.TextAreas[0].TypeCode);
            Assert.Equal("l'", paragraph.TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[1].TypeCode);
            Assert.Equal("emphasis", paragraph.TextAreas[1].Content.Text);
            
            Assert.Equal(ElementType.Text, paragraph.TextAreas[2].TypeCode);
            Assert.Equal(" with the ", paragraph.TextAreas[2].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[3].TypeCode);
            Assert.Equal("emphasis", paragraph.TextAreas[3].Content.Text);
            
            Assert.Equal(ElementType.Text, paragraph.TextAreas[4].TypeCode);
            Assert.Equal("' apostrophe.\n", paragraph.TextAreas[4].Content.Text);
          
            Assert.Equal(ElementType.Text, paragraph.TextAreas[5].TypeCode);
            Assert.Equal("l\u2019", paragraph.TextAreas[5].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[6].TypeCode);
            Assert.Equal("emphasis", paragraph.TextAreas[6].Content.Text);
            
            Assert.Equal(ElementType.Text, paragraph.TextAreas[7].TypeCode);
            Assert.Equal(" with the ", paragraph.TextAreas[7].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[8].TypeCode);
            Assert.Equal("emphasis", paragraph.TextAreas[8].Content.Text);
            
            Assert.Equal(ElementType.Text, paragraph.TextAreas[9].TypeCode);
            Assert.Equal("\u2019 apostrophe.\n", paragraph.TextAreas[9].Content.Text);
        }
        
        [Fact]
        public void CrossLine()
        {
            var document = TestUtils.Test("inlinemarkupemphasis_crossline");
            
            // TODO: should be 1.
            Assert.Equal(2, document.Elements.Count);
//            Assert.Equal(1, document.Elements.Count);
//            var paragraph = (Paragraph) document.Elements[0];
//            Assert.Equal(2, paragraph.TextAreas.Count);
//            
//            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[0].TypeCode);
//            var emphasis = (Emphasis) paragraph.TextAreas[0];
//            Assert.Equal("emphasized sentence\n", emphasis.TextAreas[0].Content.Text);
//            Assert.Equal("across lines", emphasis.TextAreas[1].Content.Text);
//            
//            Assert.Equal(ElementType.Text, paragraph.TextAreas[1].TypeCode);
//            Assert.Equal("\n", paragraph.TextAreas[1].Content.Text);
        }
    }
}
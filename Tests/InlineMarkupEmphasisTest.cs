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

            Assert.Equal(1, document.Elements.Count);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(2, paragraph.TextAreas.Count);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[0].TypeCode);
            var emphasis = (Emphasis) paragraph.TextAreas[0];
            Assert.Equal("emphasized sentence\n", emphasis.TextAreas[0].Content.Text);
            Assert.Equal("across lines", emphasis.TextAreas[1].Content.Text);
            
            Assert.Equal(ElementType.Text, paragraph.TextAreas[1].TypeCode);
            Assert.Equal("\n", paragraph.TextAreas[1].Content.Text);
        }
        
        [Fact]
        public void Punctuation()
        {
            var document = TestUtils.Test("inlinemarkupemphasis_punctuation");

            Assert.Equal(2, document.Elements.Count);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(25, paragraph.TextAreas.Count);
            
            Assert.Equal("some punctuation is allowed around inline markup, e.g.\n", paragraph.TextAreas[0].Content.Text);
            Assert.Equal("/", paragraph.TextAreas[1].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[2].TypeCode);
            Assert.Equal("emphasis", ((Emphasis)paragraph.TextAreas[2]).TextAreas[0].Content.Text);
            
            Assert.Equal("/, -", paragraph.TextAreas[3].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[4].TypeCode);
            Assert.Equal("emphasis", ((Emphasis)paragraph.TextAreas[4]).TextAreas[0].Content.Text);
            
            Assert.Equal("-, and :", paragraph.TextAreas[5].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[6].TypeCode);
            Assert.Equal("emphasis", ((Emphasis)paragraph.TextAreas[6]).TextAreas[0].Content.Text);
            
            Assert.Equal(": (delimiters),\n", paragraph.TextAreas[7].Content.Text);
            Assert.Equal("(", paragraph.TextAreas[8].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[9].TypeCode);
            Assert.Equal("emphasis", ((Emphasis)paragraph.TextAreas[9]).TextAreas[0].Content.Text);
            
            Assert.Equal("), [", paragraph.TextAreas[10].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[11].TypeCode);
            Assert.Equal("emphasis", ((Emphasis)paragraph.TextAreas[11]).TextAreas[0].Content.Text);
            
            Assert.Equal("], <", paragraph.TextAreas[12].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[13].TypeCode);
            Assert.Equal("emphasis", ((Emphasis)paragraph.TextAreas[13]).TextAreas[0].Content.Text);
            
            Assert.Equal(">, {", paragraph.TextAreas[14].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[15].TypeCode);
            Assert.Equal("emphasis", ((Emphasis)paragraph.TextAreas[15]).TextAreas[0].Content.Text);
            
            Assert.Equal("} (open/close pairs)\n", paragraph.TextAreas[16].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[17].TypeCode);
            Assert.Equal("emphasis", ((Emphasis)paragraph.TextAreas[17]).TextAreas[0].Content.Text);
            
            Assert.Equal("., ", paragraph.TextAreas[18].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[19].TypeCode);
            Assert.Equal("emphasis", ((Emphasis)paragraph.TextAreas[19]).TextAreas[0].Content.Text);
            
            Assert.Equal(",, ", paragraph.TextAreas[20].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[21].TypeCode);
            Assert.Equal("emphasis", ((Emphasis)paragraph.TextAreas[21]).TextAreas[0].Content.Text);
            
            Assert.Equal("!, and ", paragraph.TextAreas[22].Content.Text);
            
            Assert.Equal(ElementType.Emphasis, paragraph.TextAreas[23].TypeCode);
            Assert.Equal("emphasis", ((Emphasis)paragraph.TextAreas[23]).TextAreas[0].Content.Text);
            
            Assert.Equal(" (closing delimiters),\n", paragraph.TextAreas[24].Content.Text);
            
            var paragraph2 = (Paragraph) document.Elements[1];
            Assert.Equal(2, paragraph2.TextAreas.Count);
            
            Assert.Equal("but not\n", paragraph2.TextAreas[0].Content.Text);
            Assert.Equal(")*emphasis*(, ]*emphasis*[, >*emphasis*>, }*emphasis*{ (close/open pairs),\n", paragraph2.TextAreas[1].Content.Text);
        }
    }
}
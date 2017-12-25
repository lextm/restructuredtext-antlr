using Xunit;

namespace Lextm.ReStructuredText.Tests
{
    public class InlineMarkupStrongEmphasisTest
    {
        [Fact]
        public void Apostrophe()
        {
            var document = TestUtils.Test("inlinemarkupstrongemphasis_apostrophe");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(5, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.Strong, paragraph.TextAreas[1].TypeCode);
            var literal = (Strong) paragraph.TextAreas[1];
            Assert.Equal("strong", literal.Content.Text);
            var text = paragraph.TextAreas[2];
            Assert.Equal(" and l\u2019", text.Content.Text);
        }
        
        [Fact]
        public void Quoted()
        {
            var document = TestUtils.Test("inlinemarkupstrongemphasis_quoted");
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal("quoted '", paragraph.TextAreas[0].Content.Text);
            Assert.Equal("', quoted \"", paragraph.TextAreas[2].Content.Text);
            Assert.Equal("\",\n", paragraph.TextAreas[4].Content.Text);
            Assert.Equal("quoted \u2018", paragraph.TextAreas[5].Content.Text);
            Assert.Equal("\u2019, quoted \u201c", paragraph.TextAreas[7].Content.Text);
            Assert.Equal("\u201d,\n", paragraph.TextAreas[9].Content.Text);
            Assert.Equal("quoted \xab", paragraph.TextAreas[10].Content.Text);
            Assert.Equal("\xbb\n", paragraph.TextAreas[12].Content.Text);
            Assert.Equal(13, paragraph.TextAreas.Count);
        }

        [Fact]
        public void NotStrong()
        {
            var document = TestUtils.Test("inlinemarkupstrongemphasis_notstrong");
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal("(", paragraph.TextAreas[0].Content.Text);
            Assert.Equal(") but not (**) or '(** ' or x**2 or \\**kwargs or **\n", paragraph.TextAreas[2].Content.Text);
            Assert.Equal(3, paragraph.TextAreas.Count);
        }

        [Fact]
        public void Asterisk()
        {
            var document = TestUtils.Test("inlinemarkupstrongemphasis_asterisk");
            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal("Strong asterisk: ", paragraph.TextAreas[0].Content.Text);
            Assert.Equal("*", paragraph.TextAreas[1].Content.Text);
            Assert.Equal("\n", paragraph.TextAreas[2].Content.Text);
            Assert.Equal(3, paragraph.TextAreas.Count);
            
            var paragraph2 = (Paragraph) document.Elements[1];
            Assert.Equal("Strong double asterisk: ", paragraph2.TextAreas[0].Content.Text);
            Assert.Equal("**", paragraph2.TextAreas[1].Content.Text);
            Assert.Equal("\n", paragraph2.TextAreas[2].Content.Text);
            Assert.Equal(3, paragraph.TextAreas.Count);
            
            Assert.Equal(2, document.Elements.Count);
        }
    }
}
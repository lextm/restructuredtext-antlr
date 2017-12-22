using Antlr4.Runtime.Atn;
using Xunit;

namespace ReStructuredText.Tests
{
    public class InlineMarkupStrongEmphasisTest
    {
        //[Fact]
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
            Assert.Equal("\xbb\n", paragraph.TextAreas[11].Content.Text);
            Assert.Equal(12, paragraph.TextAreas.Count);
        }
    }
}
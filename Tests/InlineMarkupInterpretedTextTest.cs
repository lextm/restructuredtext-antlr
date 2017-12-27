using Xunit;

namespace Lextm.ReStructuredText.Tests
{
    public class InlineMarkupInterpretedTextTest
    {
        [Fact]
        public void Simple()
        {
            var document = TestUtils.Test("inlinemarkupinterpretedtext_simple");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);

            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(2, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.InterpretedText, paragraph.TextAreas[0].TypeCode);
            Assert.Equal("interpreted", paragraph.TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Titled()
        {
            var document = TestUtils.Test("inlinemarkupinterpretedtext_titled");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);

            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(2, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.InterpretedText, paragraph.TextAreas[0].TypeCode);
            var interpreted = (InterpretedText) paragraph.TextAreas[0];
            Assert.Equal("title", interpreted.RoleName);
            Assert.Equal("interpreted", paragraph.TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Multiple()
        {
            var document = TestUtils.Test("inlinemarkupinterpretedtext_multiple");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);

            var paragraph = (Paragraph) document.Elements[0];
            Assert.Equal(4, paragraph.TextAreas.Count);
            Assert.Equal(ElementType.InterpretedText, paragraph.TextAreas[0].TypeCode);
            var foo = (InterpretedText) paragraph.TextAreas[0];
            Assert.Equal("foo", paragraph.TextAreas[0].Content.Text);

            Assert.Equal(ElementType.Text, paragraph.TextAreas[1].TypeCode);
            Assert.Equal(" ", paragraph.TextAreas[1].Content.Text);
            
            Assert.Equal(ElementType.InterpretedText, paragraph.TextAreas[2].TypeCode);
            var bar = (InterpretedText) paragraph.TextAreas[2];
            Assert.Equal("x", bar.RoleName);
            Assert.Equal("bar", paragraph.TextAreas[2].Content.Text);
            
            Assert.Equal(ElementType.Text, paragraph.TextAreas[3].TypeCode);
            Assert.Equal(" :x:y\n", paragraph.TextAreas[3].Content.Text);
        }
    }
}
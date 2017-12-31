using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Lextm.ReStructuredText.Tests
{
    public class BlockQuoteTest
    {
        [Fact]
        public void Simple()
        {
            var document = TestUtils.Test("blockquote_simple");
            Assert.Equal(2, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Line 1.\n", paragraph.TextAreas[0].Content.Text);
            Assert.Equal("Line 2.\n", paragraph.TextAreas[1].Content.Text);
            BlockQuote element = (BlockQuote)document.Elements[1];
            Assert.True(element.TypeCode == ElementType.BlockQuote);
            Paragraph paragraph3 = (Paragraph)element.Elements[0];
            Assert.Equal("Indented.\n", paragraph3.TextAreas[0].Content.Text);
        }

        [Fact]
        public void Nested()
        {
            var document = TestUtils.Test("blockquote_nested");
            Assert.Equal(2, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Line 1.\n", paragraph.TextAreas[0].Content.Text);
            Assert.Equal("Line 2.\n", paragraph.TextAreas[1].Content.Text);
            BlockQuote block = (BlockQuote)document.Elements[1];
            Assert.True(block.TypeCode == ElementType.BlockQuote);
            Paragraph paragraph2 = (Paragraph)block.Elements[0];
            Assert.Equal("Indented 1.\n", paragraph2.TextAreas[0].Content.Text);
            BlockQuote blockQuote = (BlockQuote)block.Elements[1];
            Assert.Equal(ElementType.BlockQuote, blockQuote.TypeCode);
            Paragraph paragraph3 = (Paragraph)blockQuote.Elements[0];
            Assert.Equal("Indented 2.\n", paragraph3.TextAreas[0].Content.Text);
        }

        [Fact]
        public void Nested2()
        {
            var document = TestUtils.Test("blockquote_nested2");
            Assert.Equal(3, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Here is a paragraph.\n", paragraph.TextAreas[0].Content.Text);
            BlockQuote element = (BlockQuote)document.Elements[1];
            Assert.True(element.TypeCode == ElementType.BlockQuote);
            BlockQuote block2 = (BlockQuote)element.Elements[0];
            Assert.Equal("Indent 8 spaces.\n", block2.TextAreas[0].Content.Text);
            Paragraph paragraph3 = (Paragraph)element.Elements[1];
            Assert.Equal(ElementType.Paragraph, paragraph3.TypeCode);
            Assert.Equal("Indent 4 spaces.\n", paragraph3.TextAreas[0].Content.Text);
            Assert.Equal("Is this correct? Should it generate a warning?\n", document.Elements[2].TextAreas[0].Content.Text);
            Assert.Equal("Yes, it is correct, no warning necessary.\n", document.Elements[2].TextAreas[1].Content.Text);
        }

        [Fact]
        public void Attribution()
        {
            var document = TestUtils.Test("blockquote_attribution");
            Assert.Equal(4, document.Elements.Count);
            {
                Paragraph paragraph = (Paragraph)document.Elements[0];
                Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
                BlockQuote element = (BlockQuote)document.Elements[1];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote.\n", paragraph2.TextAreas[0].Content.Text);
                Attribution attribution = element.Attribution;
                Assert.Equal("Attribution\n", attribution.TextAreas[0].Content.Text);
            }
            {
                Paragraph paragraph = (Paragraph)document.Elements[2];
                Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
                BlockQuote element = (BlockQuote)document.Elements[3];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote.\n", paragraph2.TextAreas[0].Content.Text);
                Attribution attribution = element.Attribution;
                Assert.Equal("Attribution\n", attribution.TextAreas[0].Content.Text);
            }
        }

        [Fact]
        public void AttributionEmDash()
        {
            var document = TestUtils.Test("blockquote_attribution_emdash");
            Assert.Equal(4, document.Elements.Count);
            {
                Paragraph paragraph = (Paragraph)document.Elements[0];
                Assert.Equal("Alternative: true em-dash.\n", paragraph.TextAreas[0].Content.Text);
                BlockQuote element = (BlockQuote)document.Elements[1];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote.\n", paragraph2.TextAreas[0].Content.Text);
                Attribution attribution = element.Attribution;
                Assert.Equal("Attribution\n", attribution.TextAreas[0].Content.Text);
            }
            {
                Paragraph paragraph = (Paragraph)document.Elements[2];
                Assert.Equal("Alternative: three hyphens.\n", paragraph.TextAreas[0].Content.Text);
                BlockQuote element = (BlockQuote)document.Elements[3];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote.\n", paragraph2.TextAreas[0].Content.Text);
                Attribution attribution = element.Attribution;
                Assert.Equal("Attribution\n", attribution.TextAreas[0].Content.Text);
            }
        }

        [Fact]
        public void AttributionMultiline()
        {
            var document = TestUtils.Test("blockquote_attribution_multiline");
            Assert.Equal(5, document.Elements.Count);
            {
                Paragraph paragraph = (Paragraph)document.Elements[0];
                Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
                BlockQuote element = (BlockQuote)document.Elements[1];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote.\n", paragraph2.TextAreas[0].Content.Text);
                Attribution attribution = element.Attribution;
                Assert.Equal("Attribution line one\n", attribution.TextAreas[0].Content.Text);
                Assert.Equal("and line two\n", attribution.TextAreas[1].Content.Text);
            }
            {
                Paragraph paragraph = (Paragraph)document.Elements[2];
                Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
                BlockQuote element = (BlockQuote)document.Elements[3];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote.\n", paragraph2.TextAreas[0].Content.Text);
                Attribution attribution = element.Attribution;
                Assert.Equal("Attribution line one\n", attribution.TextAreas[0].Content.Text);
                Assert.Equal("and line two\n", attribution.TextAreas[1].Content.Text);
            }
            {
                Paragraph paragraph = (Paragraph)document.Elements[4];
                Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
            }
        }

        [Fact]
        public void AttributionSimple()
        {
            var document = TestUtils.Test("blockquote_attribution_simple");
            Assert.Equal(3, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
            {
                BlockQuote element = (BlockQuote)document.Elements[1];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote 1.\n", paragraph2.TextAreas[0].Content.Text);
                Attribution attribution = element.Attribution;
                Assert.Equal("Attribution 1\n", attribution.TextAreas[0].Content.Text);
            }
            {
                BlockQuote element = (BlockQuote)document.Elements[2];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote 2.\n", paragraph2.TextAreas[0].Content.Text);
                Attribution attribution = element.Attribution;
                Assert.Equal("Attribution 2\n", attribution.TextAreas[0].Content.Text);
            }
        }

        [Fact]
        public void AttributionSimple2()
        {
            var document = TestUtils.Test("blockquote_attribution_simple2");
            Assert.Equal(3, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
            {
                BlockQuote element = (BlockQuote)document.Elements[1];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote 1.\n", paragraph2.TextAreas[0].Content.Text);
                Attribution attribution = element.Attribution;
                Assert.Equal("Attribution 1\n", attribution.TextAreas[0].Content.Text);
            }
            {
                BlockQuote element = (BlockQuote)document.Elements[2];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote 2.\n", paragraph2.TextAreas[0].Content.Text);
                Assert.Null(element.Attribution);
            }
        }

        [Fact]
        public void AttributionComment()
        {
            var document = TestUtils.Test("blockquote_comment");
            Assert.Equal(5, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Unindented paragraph.\n", paragraph.TextAreas[0].Content.Text);
            {
                BlockQuote element = (BlockQuote)document.Elements[1];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote 1.\n", paragraph2.TextAreas[0].Content.Text);
                Attribution attribution = element.Attribution;
                Assert.Equal("Attribution 1\n", attribution.TextAreas[0].Content.Text);
            }
            {
                BlockQuote element = (BlockQuote)document.Elements[2];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote 2.\n", paragraph2.TextAreas[0].Content.Text);
                Assert.Null(element.Attribution);
            }
            var comment = (Comment)document.Elements[3];
            Assert.Equal(0, comment.TextAreas.Count);
            {
                BlockQuote element = (BlockQuote)document.Elements[4];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote 3.\n", paragraph2.TextAreas[0].Content.Text);
                Assert.Null(element.Attribution);
            }
        }

        [Fact]
        public void DefinitionList()
        {
            var document = TestUtils.Test("blockquote_definitionlist");
            Assert.Equal(6, document.Elements.Count);
            {
                Paragraph paragraph = (Paragraph)document.Elements[0];
                Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
                BlockQuote element = (BlockQuote)document.Elements[1];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("-- Not an attribution\n", paragraph2.TextAreas[0].Content.Text);
                Assert.Null(element.Attribution);
            }
            {
                Paragraph paragraph = (Paragraph)document.Elements[2];
                Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
                BlockQuote element = (BlockQuote)document.Elements[3];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote.\n", paragraph2.TextAreas[0].Content.Text);
                Assert.Null(element.Attribution);
                Paragraph paragraph3 = (Paragraph)element.Elements[1];
                Assert.Equal("\\-- Not an attribution\n", paragraph3.TextAreas[0].Content.Text);
            }
            {
                Paragraph paragraph = (Paragraph)document.Elements[4];
                Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
                BlockQuote element = (BlockQuote)document.Elements[5];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("Block quote.\n", paragraph2.TextAreas[0].Content.Text);
                Assert.Null(element.Attribution);
                DefinitionList list = (DefinitionList) element.Elements[1];
                var item1 = list.Items[0];
                Assert.Equal("-- Not an attribution line one", item1.Term.TextAreas[0].Content.Text);
                var list2 = (DefinitionList)item1.Definition.Elements[0];
                var item2 = list2.Items[0];
                Assert.Equal("and line two", item2.Term.TextAreas[0].Content.Text);
                Assert.Equal("and line three\n", item2.Definition.Elements[0].TextAreas[0].Content.Text);
            }
        }

        [Fact]
        public void AttributionComplicated()
        {
            var document = TestUtils.Test("blockquote_attribution_complicated");
            Assert.Equal(3, document.Elements.Count);
            Paragraph paragraph = (Paragraph)document.Elements[0];
            Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
            {
                BlockQuote element = (BlockQuote)document.Elements[1];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("-- Not a valid attribution\n", paragraph2.TextAreas[0].Content.Text);
                Paragraph paragraph3 = (Paragraph)element.Elements[1];
                Assert.Equal("Block quote 1.\n", paragraph3.TextAreas[0].Content.Text);
                Attribution attribution = element.Attribution;
                Assert.Equal("Attribution 1\n", attribution.TextAreas[0].Content.Text);
            }
            {
                BlockQuote element = (BlockQuote)document.Elements[2];
                Paragraph paragraph2 = (Paragraph)element.Elements[0];
                Assert.Equal("--Invalid attribution\n", paragraph2.TextAreas[0].Content.Text);
                Paragraph paragraph3 = (Paragraph)element.Elements[1];
                Assert.Equal("Block quote 2.\n", paragraph3.TextAreas[0].Content.Text);
                Attribution attribution = element.Attribution;
                Assert.Equal("Attribution 2\n", attribution.TextAreas[0].Content.Text);
            }
        }
    }
}

using System;
using System.Runtime.Remoting;
using Xunit;

namespace ReStructuredText.Tests
{
    public class SectionTest
    {
        [Fact]
        public void SectionWithParagraph()
        {
            var document = TestUtils.Test("section_paragraph");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title", section.Title[0].Content.Text);
            Assert.Equal(1, section.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section.Elements[0].TypeCode);
            var paragraph = (Paragraph)section.Elements[0];
            Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
        }

        [Fact]
        public void SectionWithParagraphNoBlankLine()
        {
            var document = TestUtils.Test("section_paragraph_noblankline");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title", section.Title[0].Content.Text);
            Assert.Equal(1, section.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section.Elements[0].TypeCode);
            var paragraph = (Paragraph)section.Elements[0];
            Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
        }

        [Fact]
        public void SectionSurroundedByParagraph()
        {
            var document = TestUtils.Test("section_paragraph_surrounded");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            Assert.Equal(ElementType.Section, document.Elements[1].TypeCode);
            var section = (Section)document.Elements[1];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title", section.Title[0].Content.Text);
            Assert.Equal(1, section.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section.Elements[0].TypeCode);
            var paragraph = (Paragraph)section.Elements[0];
            Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
        }

        [Fact]
        public void SectionWithOverlineTitle()
        {
            var document = TestUtils.Test("section_overline");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title", section.Title[0].Content.Text);
            Assert.Equal(1, section.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section.Elements[0].TypeCode);
            var paragraph = (Paragraph)section.Elements[0];
            Assert.Equal("Test overline title.\n", paragraph.TextAreas[0].Content.Text);
        }

        [Fact]
        public void SectionWithOverlineTitleWithInset()
        {
            var document = TestUtils.Test("section_overline_inset");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title", section.Title[0].Content.Text);
            Assert.Equal(1, section.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section.Elements[0].TypeCode);
            var paragraph = (Paragraph)section.Elements[0];
            Assert.Equal("Test overline title.\n", paragraph.TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Nested()
        {
            var document = TestUtils.Test("section_nested");
            Assert.Equal(2, document.Elements.Count);
            
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title 1", section.Title[0].Content.Text);
            Assert.Equal(2, section.Elements.Count);
            
            Assert.Equal(ElementType.Paragraph, section.Elements[0].TypeCode);
            var paragraph = (Paragraph)section.Elements[0];
            Assert.Equal("Paragraph 1.\n", paragraph.TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.Section, section.Elements[1].TypeCode);
            var section1 = (Section) section.Elements[1];
            Assert.Equal(2, section1.Level);
            Assert.Equal("Title 2", section1.Title[0].Content.Text);
            Assert.Equal(1, section1.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section1.Elements[0].TypeCode);
            Assert.Equal("Paragraph 2.\n", section1. Elements[0].TextAreas[0].Content.Text);

            Assert.Equal(ElementType.Section, document.Elements[1].TypeCode);
            var section2 = (Section)document.Elements[1];
            Assert.Equal(1, section2.Level);
            Assert.Equal("Title 3", section2.Title[0].Content.Text);
            Assert.Equal(2, section2.Elements.Count);

            Assert.Equal(ElementType.Paragraph, section2.Elements[0].TypeCode);
            var paragraph1 = (Paragraph)section2.Elements[0];
            Assert.Equal("Paragraph 3.\n", paragraph1.TextAreas[0].Content.Text);

            Assert.Equal(ElementType.Section, section2.Elements[1].TypeCode);
            var section3 = (Section)section2.Elements[1];
            Assert.Equal(2, section3.Level);
            Assert.Equal("Title 4", section3.Title[0].Content.Text);
            Assert.Equal(1, section3.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section3.Elements[0].TypeCode);
            Assert.Equal("Paragraph 4.\n", section3.Elements[0].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void NestedWithOverlines()
        {
            var document = TestUtils.Test("section_nested_overline");
            Assert.Equal(3, document.Elements.Count);
            
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            Assert.Equal("Test return to existing, highest-level section (Title 3, with overlines).\n", document.Elements[0].TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.Section, document.Elements[1].TypeCode);
            var section = (Section)document.Elements[1];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title 1", section.Title[0].Content.Text);
            Assert.Equal(2, section.Elements.Count);
            
            Assert.Equal(ElementType.Paragraph, section.Elements[0].TypeCode);
            var paragraph = (Paragraph)section.Elements[0];
            Assert.Equal("Paragraph 1.\n", paragraph.TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.Section, section.Elements[1].TypeCode);
            var section1 = (Section) section.Elements[1];
            Assert.Equal(2, section1.Level);
            Assert.Equal("Title 2", section1.Title[0].Content.Text);
            Assert.Equal(1, section1.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section1.Elements[0].TypeCode);
            Assert.Equal("Paragraph 2.\n", section1. Elements[0].TextAreas[0].Content.Text);

            Assert.Equal(ElementType.Section, document.Elements[2].TypeCode);
            var section2 = (Section)document.Elements[2];
            Assert.Equal(1, section2.Level);
            Assert.Equal("Title 3", section2.Title[0].Content.Text);
            Assert.Equal(2, section2.Elements.Count);

            Assert.Equal(ElementType.Paragraph, section2.Elements[0].TypeCode);
            var paragraph1 = (Paragraph)section2.Elements[0];
            Assert.Equal("Paragraph 3.\n", paragraph1.TextAreas[0].Content.Text);

            Assert.Equal(ElementType.Section, section2.Elements[1].TypeCode);
            var section3 = (Section)section2.Elements[1];
            Assert.Equal(2, section3.Level);
            Assert.Equal("Title 4", section3.Title[0].Content.Text);
            Assert.Equal(1, section3.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section3.Elements[0].TypeCode);
            Assert.Equal("Paragraph 4.\n", section3.Elements[0].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void NestedWithOverlinesLevel3()
        {
            var document = TestUtils.Test("section_nested_overline_level3");
            Assert.Equal(1, document.Elements.Count);
            
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title 1", section.Title[0].Content.Text);
            Assert.Equal(3, section.Elements.Count);
            
            Assert.Equal(ElementType.Paragraph, section.Elements[0].TypeCode);
            var paragraph = (Paragraph)section.Elements[0];
            Assert.Equal("Paragraph 1.\n", paragraph.TextAreas[0].Content.Text);
            
            Assert.Equal(ElementType.Section, section.Elements[1].TypeCode);
            var section1 = (Section) section.Elements[1];
            Assert.Equal(2, section1.Level);
            Assert.Equal("Title 2", section1.Title[0].Content.Text);
            Assert.Equal(2, section1.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section1.Elements[0].TypeCode);
            Assert.Equal("Paragraph 2.\n", section1. Elements[0].TextAreas[0].Content.Text);

            Assert.Equal(ElementType.Section, section1.Elements[1].TypeCode);
            var section2 = (Section)section1.Elements[1];
            Assert.Equal(3, section2.Level);
            Assert.Equal("Title 3", section2.Title[0].Content.Text);
            Assert.Equal(1, section2.Elements.Count);

            Assert.Equal(ElementType.Paragraph, section2.Elements[0].TypeCode);
            var paragraph1 = (Paragraph)section2.Elements[0];
            Assert.Equal("Paragraph 3.\n", paragraph1.TextAreas[0].Content.Text);

            Assert.Equal(ElementType.Section, section.Elements[2].TypeCode);
            var section3 = (Section)section.Elements[2];
            Assert.Equal(2, section3.Level);
            Assert.Equal("Title 4", section3.Title[0].Content.Text);
            Assert.Equal(1, section3.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section3.Elements[0].TypeCode);
            Assert.Equal("Paragraph 4.\n", section3.Elements[0].TextAreas[0].Content.Text);
        }

        [Fact]
        public void InlineMarkup()
        {
            var document =
                TestUtils.Test("section_inlinemarkup");
            Assert.Equal(1, document.Elements.Count);
            var section = (Section) document.Elements[0];

            // TODO: should be 4.
            // Assert.Equal(4, section.Title.Count);
            Assert.Equal(5, section.Title.Count);
//            Assert.Equal("Title containing ", section.Title[0].Content.Text);
//            Assert.Equal("inline", section.Title[1].Content.Text);
//            Assert.Equal(" ", section.Title[2].Content.Text);
//            Assert.Equal("markup", section.Title[3].Content.Text);
        }
        
        [Fact]
        public void NumberedTitle()
        {
            var document = TestUtils.Test("section_numbered");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("1. Numbered Title", section.Title[0].Content.Text);
            Assert.Equal(1, section.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section.Elements[0].TypeCode);
            var paragraph = (Paragraph)section.Elements[0];
            Assert.Equal("Paragraph.\n", paragraph.TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Short()
        {
            var document = TestUtils.Test("section_short");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("ABC", section.Title[0].Content.Text);
            Assert.Equal(1, section.Elements.Count);
            Assert.Equal(ElementType.Paragraph, section.Elements[0].TypeCode);
            var paragraph = (Paragraph)section.Elements[0];
            Assert.Equal("Short title.\n", paragraph.TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void BlockquoteLike()
        {
            var document = TestUtils.Test("section_blockquote");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            Assert.Equal(ElementType.BlockQuote, document.Elements[1].TypeCode);
            var block = (BlockQuote)document.Elements[1];
            Assert.Equal(1, block.Level);
            Assert.Equal("ABC\n", block.TextAreas[0].Content.Text);
            Assert.Equal("==\n", block.TextAreas[1].Content.Text);
            Assert.Equal("Underline too short.\n", block.Elements[1].TextAreas[0].Content.Text);
        }
        
        [Fact]
        public void Empty()
        {
            var document = TestUtils.Test("section_empty");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Empty Section", section.Title[0].Content.Text);
            Assert.Equal(0, section.Elements.Count);
        }

        [Fact]
        public void ShortTitles()
        {
            var document = TestUtils.Test(
                "section_shorts");
            Assert.Equal(2, document.Elements.Count);
            var section = (Section)document.Elements[0];
            Assert.Equal("One", section.Title[0].Content.Text);
            var section2 = (Section) document.Elements[1];
            Assert.Equal("Two", section2.Title[0].Content.Text);
        }
    }
}

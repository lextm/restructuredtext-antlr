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
            var document = TestUtils.Test("\nTitle\n=====\n\nParagraph.\n");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title", section.Title);
            Assert.Equal(1, section.Content.Count);
            Assert.Equal(ElementType.Paragraph, section.Content[0].TypeCode);
            var paragraph = (Paragraph)section.Content[0];
            Assert.Equal("Paragraph.\n", paragraph.Lines[0].Text.Content);
        }

        [Fact]
        public void SectionWithParagraphNoBlankLine()
        {
            var document = TestUtils.Test("\nTitle\n=====\nParagraph.\n");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title", section.Title);
            Assert.Equal(1, section.Content.Count);
            Assert.Equal(ElementType.Paragraph, section.Content[0].TypeCode);
            var paragraph = (Paragraph)section.Content[0];
            Assert.Equal("Paragraph.\n", paragraph.Lines[0].Text.Content);
        }

        [Fact]
        public void SectionSurroundedByParagraph()
        {
            var document = TestUtils.Test("\nParagraph.\n\nTitle\n=====\nParagraph.\n");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            Assert.Equal(ElementType.Section, document.Elements[1].TypeCode);
            var section = (Section)document.Elements[1];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title", section.Title);
            Assert.Equal(1, section.Content.Count);
            Assert.Equal(ElementType.Paragraph, section.Content[0].TypeCode);
            var paragraph = (Paragraph)section.Content[0];
            Assert.Equal("Paragraph.\n", paragraph.Lines[0].Text.Content);
        }

        [Fact]
        public void SectionWithOverlineTitle()
        {
            var document = TestUtils.Test("\n=====\nTitle\n=====\n\nTest overline title.\n");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title", section.Title);
            Assert.Equal(1, section.Content.Count);
            Assert.Equal(ElementType.Paragraph, section.Content[0].TypeCode);
            var paragraph = (Paragraph)section.Content[0];
            Assert.Equal("Test overline title.\n", paragraph.Lines[0].Text.Content);
        }

        [Fact]
        public void SectionWithOverlineTitleWithInset()
        {
            var document = TestUtils.Test("\n=======\n Title \n=======\n\nTest overline title.\n");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title", section.Title);
            Assert.Equal(1, section.Content.Count);
            Assert.Equal(ElementType.Paragraph, section.Content[0].TypeCode);
            var paragraph = (Paragraph)section.Content[0];
            Assert.Equal("Test overline title.\n", paragraph.Lines[0].Text.Content);
        }
        
        [Fact]
        public void Nested()
        {
            var document = TestUtils.Test("\nTitle 1\n=======\nParagraph 1.\n\nTitle 2\n-------\nParagraph 2.\n\nTitle 3\n=======\nParagraph 3.\n\nTitle 4\n-------\nParagraph 4.\n");
            Assert.Equal(2, document.Elements.Count);
            
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title 1", section.Title);
            Assert.Equal(2, section.Content.Count);
            
            Assert.Equal(ElementType.Paragraph, section.Content[0].TypeCode);
            var paragraph = (Paragraph)section.Content[0];
            Assert.Equal("Paragraph 1.\n", paragraph.Lines[0].Text.Content);
            
            Assert.Equal(ElementType.Section, section.Content[1].TypeCode);
            var section1 = (Section) section.Content[1];
            Assert.Equal(2, section1.Level);
            Assert.Equal("Title 2", section1.Title);
            Assert.Equal(1, section1.Content.Count);
            Assert.Equal(ElementType.Paragraph, section1.Content[0].TypeCode);
            Assert.Equal("Paragraph 2.\n", section1. Content[0].Lines[0].Text.Content);

            Assert.Equal(ElementType.Section, document.Elements[1].TypeCode);
            var section2 = (Section)document.Elements[1];
            Assert.Equal(1, section2.Level);
            Assert.Equal("Title 3", section2.Title);
            Assert.Equal(2, section2.Content.Count);

            Assert.Equal(ElementType.Paragraph, section2.Content[0].TypeCode);
            var paragraph1 = (Paragraph)section2.Content[0];
            Assert.Equal("Paragraph 3.\n", paragraph1.Lines[0].Text.Content);

            Assert.Equal(ElementType.Section, section2.Content[1].TypeCode);
            var section3 = (Section)section2.Content[1];
            Assert.Equal(2, section3.Level);
            Assert.Equal("Title 4", section3.Title);
            Assert.Equal(1, section3.Content.Count);
            Assert.Equal(ElementType.Paragraph, section3.Content[0].TypeCode);
            Assert.Equal("Paragraph 4.\n", section3.Content[0].Lines[0].Text.Content);
        }
        
        [Fact]
        public void NestedWithOverlines()
        {
            var document = TestUtils.Test("\nTest return to existing, highest-level section (Title 3, with overlines).\n\n=======\nTitle 1\n=======\nParagraph 1.\n\n-------\nTitle 2\n-------\nParagraph 2.\n\n=======\nTitle 3\n=======\n\nParagraph 3.\n\n-------\nTitle 4\n-------\nParagraph 4.\n");
            Assert.Equal(3, document.Elements.Count);
            
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            Assert.Equal("Test return to existing, highest-level section (Title 3, with overlines).\n", document.Elements[0].Lines[0].Text.Content);
            
            Assert.Equal(ElementType.Section, document.Elements[1].TypeCode);
            var section = (Section)document.Elements[1];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title 1", section.Title);
            Assert.Equal(2, section.Content.Count);
            
            Assert.Equal(ElementType.Paragraph, section.Content[0].TypeCode);
            var paragraph = (Paragraph)section.Content[0];
            Assert.Equal("Paragraph 1.\n", paragraph.Lines[0].Text.Content);
            
            Assert.Equal(ElementType.Section, section.Content[1].TypeCode);
            var section1 = (Section) section.Content[1];
            Assert.Equal(2, section1.Level);
            Assert.Equal("Title 2", section1.Title);
            Assert.Equal(1, section1.Content.Count);
            Assert.Equal(ElementType.Paragraph, section1.Content[0].TypeCode);
            Assert.Equal("Paragraph 2.\n", section1. Content[0].Lines[0].Text.Content);

            Assert.Equal(ElementType.Section, document.Elements[2].TypeCode);
            var section2 = (Section)document.Elements[2];
            Assert.Equal(1, section2.Level);
            Assert.Equal("Title 3", section2.Title);
            Assert.Equal(2, section2.Content.Count);

            Assert.Equal(ElementType.Paragraph, section2.Content[0].TypeCode);
            var paragraph1 = (Paragraph)section2.Content[0];
            Assert.Equal("Paragraph 3.\n", paragraph1.Lines[0].Text.Content);

            Assert.Equal(ElementType.Section, section2.Content[1].TypeCode);
            var section3 = (Section)section2.Content[1];
            Assert.Equal(2, section3.Level);
            Assert.Equal("Title 4", section3.Title);
            Assert.Equal(1, section3.Content.Count);
            Assert.Equal(ElementType.Paragraph, section3.Content[0].TypeCode);
            Assert.Equal("Paragraph 4.\n", section3.Content[0].Lines[0].Text.Content);
        }
        
        [Fact]
        public void NestedWithOverlinesLevel3()
        {
            var document = TestUtils.Test("\nTitle 1\n=======\nParagraph 1.\n\nTitle 2\n-------\nParagraph 2.\n\nTitle 3\n+++++++\nParagraph 3.\n\nTitle 4\n-------\nParagraph 4.\n");
            Assert.Equal(1, document.Elements.Count);
            
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Title 1", section.Title);
            Assert.Equal(3, section.Content.Count);
            
            Assert.Equal(ElementType.Paragraph, section.Content[0].TypeCode);
            var paragraph = (Paragraph)section.Content[0];
            Assert.Equal("Paragraph 1.\n", paragraph.Lines[0].Text.Content);
            
            Assert.Equal(ElementType.Section, section.Content[1].TypeCode);
            var section1 = (Section) section.Content[1];
            Assert.Equal(2, section1.Level);
            Assert.Equal("Title 2", section1.Title);
            Assert.Equal(2, section1.Content.Count);
            Assert.Equal(ElementType.Paragraph, section1.Content[0].TypeCode);
            Assert.Equal("Paragraph 2.\n", section1. Content[0].Lines[0].Text.Content);

            Assert.Equal(ElementType.Section, section1.Content[1].TypeCode);
            var section2 = (Section)section1.Content[1];
            Assert.Equal(3, section2.Level);
            Assert.Equal("Title 3", section2.Title);
            Assert.Equal(1, section2.Content.Count);

            Assert.Equal(ElementType.Paragraph, section2.Content[0].TypeCode);
            var paragraph1 = (Paragraph)section2.Content[0];
            Assert.Equal("Paragraph 3.\n", paragraph1.Lines[0].Text.Content);

            Assert.Equal(ElementType.Section, section.Content[2].TypeCode);
            var section3 = (Section)section.Content[2];
            Assert.Equal(2, section3.Level);
            Assert.Equal("Title 4", section3.Title);
            Assert.Equal(1, section3.Content.Count);
            Assert.Equal(ElementType.Paragraph, section3.Content[0].TypeCode);
            Assert.Equal("Paragraph 4.\n", section3.Content[0].Lines[0].Text.Content);
        }

        [Fact]
        public void InlineMarkup()
        {
            var document =
                TestUtils.Test("\nTitle containing *inline* **markup**\n=======================\n\nParagraph.\n");
            Assert.Equal(1, document.Elements.Count);
            var section = (Section) document.Elements[0];
            // TODO:
            Assert.Equal("Title containing *inline* **markup**", section.Title);
        }
        
        [Fact]
        public void NumberedTitle()
        {
            var document = TestUtils.Test("\n1. Numbered Title\n=================\n\nParagraph.\n");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("1. Numbered Title", section.Title);
            Assert.Equal(1, section.Content.Count);
            Assert.Equal(ElementType.Paragraph, section.Content[0].TypeCode);
            var paragraph = (Paragraph)section.Content[0];
            Assert.Equal("Paragraph.\n", paragraph.Lines[0].Text.Content);
        }
        
        [Fact]
        public void Short()
        {
            var document = TestUtils.Test("\nABC\n===\n\nShort title.\n");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("ABC", section.Title);
            Assert.Equal(1, section.Content.Count);
            Assert.Equal(ElementType.Paragraph, section.Content[0].TypeCode);
            var paragraph = (Paragraph)section.Content[0];
            Assert.Equal("Short title.\n", paragraph.Lines[0].Text.Content);
        }
        
        [Fact]
        public void BlockquoteLike()
        {
            var document = TestUtils.Test("\nParagraph\n\n    ABC\n    ==\n\n    Underline too short.\n");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal(ElementType.Paragraph, document.Elements[0].TypeCode);
            Assert.Equal(ElementType.BlockQuote, document.Elements[1].TypeCode);
            var block = (BlockQuote)document.Elements[1];
            Assert.Equal(1, block.Level);
            Assert.Equal("ABC\n", block.Lines[0].Text.Content);
            Assert.Equal("==\n", block.Lines[1].Text.Content);
            Assert.Equal("Underline too short.\n", block.Content[1].Lines[0].Text.Content);
        }
        
        [Fact]
        public void Empty()
        {
            var document = TestUtils.Test("\nEmpty Section\n==============\n");
            Assert.Equal(1, document.Elements.Count);
            Assert.Equal(ElementType.Section, document.Elements[0].TypeCode);
            var section = (Section)document.Elements[0];
            Assert.Equal(1, section.Level);
            Assert.Equal("Empty Section", section.Title);
            Assert.Equal(0, section.Content.Count);
        }

        [Fact]
        public void ShortTitles()
        {
            var document = TestUtils.Test(
                "\n===\nOne\n===\n\nThe bubble-up parser strategy conflicts with short titles\n(<= 3 char-long over- & underlines).\n\n===\nTwo\n===\n\nThe parser currently contains a work-round kludge.\nWithout it, the parser ends up in an infinite loop.\n");
            Assert.Equal(2, document.Elements.Count);
            var section = (Section)document.Elements[0];
            Assert.Equal("One", section.Title);
            var section2 = (Section) document.Elements[1];
            Assert.Equal("Two", section2.Title);
        }
    }
}

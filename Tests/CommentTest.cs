﻿using Xunit;

namespace ReStructuredText.Tests
{
    public class CommentTest
    {
        [Fact]
        public void CommentAndParagraph()
        {
            var document = TestUtils.Test("\n.. A comment\n\nParagraph.\n");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal("A comment\n", document.Elements[0].Lines[0].Text.Content);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("Paragraph.\n", document.Elements[1].Lines[0].Text.Content);
            Assert.False(document.Elements[1].TypeCode == ElementType.Comment);
        }

        [Fact]
        public void TwoLinesCommentAndParagraph()
        {
            var document = TestUtils.Test("\n.. A comment\n   block.\n\nParagraph.\n");
            Assert.Equal(2, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("A comment\n", document.Elements[0].Lines[0].Text.Content);
            Assert.Equal("block.\n", document.Elements[0].Lines[1].Text.Content);
            Assert.Equal("Paragraph.\n", document.Elements[1].Lines[0].Text.Content);
        }

        [Fact]
        public void MultipleLinesComment()
        {
            var document = TestUtils.Test("\n..\n   A comment consisting of multiple lines\n   starting on the line after the\n   explicit markup start.\n");
            Assert.Single(document.Elements);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("A comment consisting of multiple lines\n", document.Elements[0].Lines[0].Text.Content);
            Assert.Equal("starting on the line after the\n", document.Elements[0].Lines[1].Text.Content);
            Assert.Equal("explicit markup start.\n", document.Elements[0].Lines[2].Text.Content);
        }

        [Fact]
        public void MultipleCommentsAndParagraph()
        {
            var document = TestUtils.Test("\n.. A comment.\n.. Another.\n\nParagraph.\n");
            Assert.Equal(3, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("A comment.\n", document.Elements[0].Lines[0].Text.Content);
            Assert.True(document.Elements[1].TypeCode == ElementType.Comment);
            Assert.Equal("Another.\n", document.Elements[1].Lines[0].Text.Content);
            Assert.Equal("Paragraph.\n", document.Elements[2].Lines[0].Text.Content);
        }

        [Fact]
        public void CommentWithDoubleColonsAndParagraph()
        {
            var document = TestUtils.Test("\n.. A comment::\n\nParagraph.\n");
            Assert.Equal(2, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("A comment::\n", document.Elements[0].Lines[0].Text.Content);
            Assert.Equal("Paragraph.\n", document.Elements[1].Lines[0].Text.Content);
        }

        [Fact]
        public void CommentSimilarToDirective()
        {
            var document = TestUtils.Test("\n..\n   A comment::\n\nParagraph.\n");
            Assert.Equal(2, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("A comment::\n", document.Elements[0].Lines[0].Text.Content);
            Assert.Equal("Paragraph.\n", document.Elements[1].Lines[0].Text.Content);
        }

        [Fact]
        public void CommentSimilarToHyperlinkTarget()
        {
            var document = TestUtils.Test("\n..\n   _comment: http://example.org\n\nParagraph.\n");
            Assert.Equal(2, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("_comment: http://example.org\n", document.Elements[0].Lines[0].Text.Content);
            Assert.Equal("Paragraph.\n", document.Elements[1].Lines[0].Text.Content);
        }

        [Fact]
        public void CommentSimilarToCitation()
        {
            var document = TestUtils.Test("\n..\n  [comment] Not a citation.\n\nParagraph.\n");
            Assert.Equal(2, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("[comment] Not a citation.\n", document.Elements[0].Lines[0].Text.Content);
            Assert.Equal("Paragraph.\n", document.Elements[1].Lines[0].Text.Content);
        }

        [Fact]
        public void CommentSimilarToSubstitutionDefinition()
        {
            var document = TestUtils.Test("\n..\n  |comment| image:: bogus.png\n\nParagraph.\n");
            Assert.Equal(2, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("|comment| image:: bogus.png\n", document.Elements[0].Lines[0].Text.Content);
            Assert.Equal("Paragraph.\n", document.Elements[1].Lines[0].Text.Content);
        }

        [Fact]
        public void CommentFollowedByEmptyComment()
        {
            var document = TestUtils.Test("\n.. Next is an empty comment, which serves to end this comment and\n   prevents the following block quote being swallowed up.\n\n..\n\n   A block quote.\n");
            Assert.Equal(3, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("Next is an empty comment, which serves to end this comment and\n", document.Elements[0].Lines[0].Text.Content);
            Assert.Equal("prevents the following block quote being swallowed up.\n", document.Elements[0].Lines[1].Text.Content);
            Assert.True(document.Elements[1].TypeCode == ElementType.Comment);
            Assert.Equal(0, document.Elements[1].Lines.Count);
            Assert.Equal("A block quote.\n", document.Elements[2].Lines[0].Text.Content);
            Assert.True(document.Elements[2].TypeCode == ElementType.BlockQuote);
        }

        // TODO: 'a comment in definition list item', and more from
        // https://github.com/seikichi/restructured/blob/master/test/parser/CommentTest.js
    }
}
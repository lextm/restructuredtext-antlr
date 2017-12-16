using Xunit;

namespace Tests
{
    public class CommentTest
    {
        [Fact]
        public void CommentAndParagraph()
        {
            var document = TestUtils.Test("\n.. A comment\n\nParagraph.\n");
            Assert.Equal(2, document.Paragraphs.Count);
            Assert.Equal("A comment\n", document.Paragraphs[0].Lines[0].Text.Content);
            Assert.True(document.Paragraphs[0].IsComment);
            Assert.Equal("Paragraph.\n", document.Paragraphs[1].Lines[0].Text.Content);
            Assert.False(document.Paragraphs[1].IsComment);
        }
        
        [Fact]
        public void TwoLinesCommentAndParagraph()
        {
            var document = TestUtils.Test("\n.. A comment\n   block.\n\nParagraph.\n");
            Assert.Equal(2, document.Paragraphs.Count);
            Assert.True(document.Paragraphs[0].IsComment);
            Assert.Equal("A comment\n", document.Paragraphs[0].Lines[0].Text.Content);
            Assert.Equal("block.\n", document.Paragraphs[0].Lines[1].Text.Content);
            Assert.Equal("Paragraph.\n", document.Paragraphs[1].Lines[0].Text.Content);
        }

        [Fact]
        public void MultipleLinesComment()
        {
            var document = TestUtils.Test("\n..\n   A comment consisting of multiple lines\n   starting on the line after the\n   explicit markup start.\n");
            Assert.Single(document.Paragraphs);
            Assert.True(document.Paragraphs[0].IsComment);
            Assert.Equal("A comment consisting of multiple lines\n", document.Paragraphs[0].Lines[0].Text.Content);
            Assert.Equal("starting on the line after the\n", document.Paragraphs[0].Lines[1].Text.Content);
            Assert.Equal("explicit markup start.\n", document.Paragraphs[0].Lines[2].Text.Content);
        }

        [Fact]
        public void MultipleCommentsAndParagraph()
        {
            var document = TestUtils.Test("\n.. A comment.\n.. Another.\n\nParagraph.\n");
            Assert.Equal(3, document.Paragraphs.Count);
            Assert.True(document.Paragraphs[0].IsComment);
            Assert.Equal("A comment.\n", document.Paragraphs[0].Lines[0].Text.Content);
            Assert.True(document.Paragraphs[1].IsComment);
            Assert.Equal("Another.\n", document.Paragraphs[1].Lines[0].Text.Content);
            Assert.Equal("Paragraph.\n", document.Paragraphs[2].Lines[0].Text.Content);
        }

        [Fact]
        public void CommentWithDoubleColonsAndParagraph()
        {
            var document = TestUtils.Test("\n.. A comment::\n\nParagraph.\n");
            Assert.Equal(2, document.Paragraphs.Count);
            Assert.True(document.Paragraphs[0].IsComment);
            Assert.Equal("A comment::\n", document.Paragraphs[0].Lines[0].Text.Content);
            Assert.Equal("Paragraph.\n", document.Paragraphs[1].Lines[0].Text.Content);
        }

        [Fact]
        public void CommentSimilarToDirective()
        {
            var document = TestUtils.Test("\n..\n   A comment::\n\nParagraph.\n");
            Assert.Equal(2, document.Paragraphs.Count);
            Assert.True(document.Paragraphs[0].IsComment);
            Assert.Equal("A comment::\n", document.Paragraphs[0].Lines[0].Text.Content);
            Assert.Equal("Paragraph.\n", document.Paragraphs[1].Lines[0].Text.Content);
        }

        [Fact]
        public void CommentSimilarToHyperlinkTarget()
        {
            var document = TestUtils.Test("\n..\n   _comment: http://example.org\n\nParagraph.\n");
            Assert.Equal(2, document.Paragraphs.Count);
            Assert.True(document.Paragraphs[0].IsComment);
            Assert.Equal("_comment: http://example.org\n", document.Paragraphs[0].Lines[0].Text.Content);
            Assert.Equal("Paragraph.\n", document.Paragraphs[1].Lines[0].Text.Content);
        }

        [Fact]
        public void CommentSimilarToCitation()
        {
            var document = TestUtils.Test("\n..\n  [comment] Not a citation.\n\nParagraph.\n");
            Assert.Equal(2, document.Paragraphs.Count);
            Assert.True(document.Paragraphs[0].IsComment);
            Assert.Equal("[comment] Not a citation.\n", document.Paragraphs[0].Lines[0].Text.Content);
            Assert.Equal("Paragraph.\n", document.Paragraphs[1].Lines[0].Text.Content);
        }

        [Fact]
        public void CommentSimilarToSubstitutionDefinition()
        {
            var document = TestUtils.Test("\n..\n  |comment| image:: bogus.png\n\nParagraph.\n");
            Assert.Equal(2, document.Paragraphs.Count);
            Assert.True(document.Paragraphs[0].IsComment);
            Assert.Equal("|comment| image:: bogus.png\n", document.Paragraphs[0].Lines[0].Text.Content);
            Assert.Equal("Paragraph.\n", document.Paragraphs[1].Lines[0].Text.Content);
        }

        [Fact]
        public void CommentFollowedByEmptyComment()
        {
            var document = TestUtils.Test("\n.. Next is an empty comment, which serves to end this comment and\n   prevents the following block quote being swallowed up.\n\n..\n\n   A block quote.\n");
            Assert.Equal(3, document.Paragraphs.Count);
            Assert.True(document.Paragraphs[0].IsComment);
            Assert.Equal("Next is an empty comment, which serves to end this comment and\n", document.Paragraphs[0].Lines[0].Text.Content);
            Assert.Equal("prevents the following block quote being swallowed up.\n", document.Paragraphs[0].Lines[1].Text.Content);
            Assert.True(document.Paragraphs[1].IsComment);
            Assert.Equal(0, document.Paragraphs[1].Lines.Count);
            Assert.Equal("A block quote.\n", document.Paragraphs[2].Lines[0].Text.Content);
            Assert.True(document.Paragraphs[2].IsBlockQuote);
        }

        // TODO: 'a comment in definition list item', and more from
        // https://github.com/seikichi/restructured/blob/master/test/parser/CommentTest.js
    }
}
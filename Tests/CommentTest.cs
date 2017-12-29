using Xunit;

namespace Lextm.ReStructuredText.Tests
{
    public class CommentTest
    {
        [Fact]
        public void CommentAndParagraph()
        {
            var document = TestUtils.Test("comment_commentandparagraph");
            Assert.Equal(2, document.Elements.Count);
            Assert.Equal("A comment\n", document.Elements[0].TextAreas[0].Content.Text);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("Paragraph.\n", document.Elements[1].TextAreas[0].Content.Text);
            Assert.False(document.Elements[1].TypeCode == ElementType.Comment);
        }

        [Fact]
        public void TwoLinesCommentAndParagraph()
        {
            var document = TestUtils.Test("comment_twolinescommentandparagraph");
            Assert.Equal(2, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("A comment\n", document.Elements[0].TextAreas[0].Content.Text);
            Assert.Equal("block.\n", document.Elements[0].TextAreas[1].Content.Text);
            Assert.Equal("Paragraph.\n", document.Elements[1].TextAreas[0].Content.Text);
        }

        [Fact]
        public void MultipleLinesComment()
        {
            var document = TestUtils.Test("comment_multiplelines");
            Assert.Single(document.Elements);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("A comment consisting of multiple lines\n", document.Elements[0].TextAreas[0].Content.Text);
            Assert.Equal("starting on the line after the\n", document.Elements[0].TextAreas[1].Content.Text);
            Assert.Equal("explicit markup start.\n", document.Elements[0].TextAreas[2].Content.Text);
        }

        [Fact]
        public void MultipleCommentsAndParagraph()
        {
            var document = TestUtils.Test("comment_multiple");
            Assert.Equal(3, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("A comment.\n", document.Elements[0].TextAreas[0].Content.Text);
            Assert.True(document.Elements[1].TypeCode == ElementType.Comment);
            Assert.Equal("Another.\n", document.Elements[1].TextAreas[0].Content.Text);
            Assert.Equal("Paragraph.\n", document.Elements[2].TextAreas[0].Content.Text);
        }

        [Fact]
        public void CommentWithDoubleColonsAndParagraph()
        {
            var document = TestUtils.Test("comment_doublecolons");
            Assert.Equal(2, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("A comment::\n", document.Elements[0].TextAreas[0].Content.Text);
            Assert.Equal("Paragraph.\n", document.Elements[1].TextAreas[0].Content.Text);
        }

        [Fact]
        public void CommentSimilarToDirective()
        {
            var document = TestUtils.Test("comment_directive");
            Assert.Equal(2, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("A comment::\n", document.Elements[0].TextAreas[0].Content.Text);
            Assert.Equal("Paragraph.\n", document.Elements[1].TextAreas[0].Content.Text);
        }

        [Fact]
        public void CommentSimilarToHyperlinkTarget()
        {
            var document = TestUtils.Test("comment_hyperlinktarget");
            Assert.Equal(2, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("_comment: http://example.org\n", document.Elements[0].TextAreas[0].Content.Text);
            Assert.Equal("Paragraph.\n", document.Elements[1].TextAreas[0].Content.Text);
        }

        [Fact]
        public void CommentSimilarToCitation()
        {
            var document = TestUtils.Test("comment_citation");
            Assert.Equal(2, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("[comment] Not a citation.\n", document.Elements[0].TextAreas[0].Content.Text);
            Assert.Equal("Paragraph.\n", document.Elements[1].TextAreas[0].Content.Text);
        }

        [Fact]
        public void CommentSimilarToSubstitutionDefinition()
        {
            var document = TestUtils.Test("comment_substitution");
            Assert.Equal(2, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("|comment| image:: bogus.png\n", document.Elements[0].TextAreas[0].Content.Text);
            Assert.Equal("Paragraph.\n", document.Elements[1].TextAreas[0].Content.Text);
        }

        [Fact]
        public void CommentFollowedByEmptyComment()
        {
            var document = TestUtils.Test("comment_followedbyempty");
            Assert.Equal(3, document.Elements.Count);
            Assert.True(document.Elements[0].TypeCode == ElementType.Comment);
            Assert.Equal("Next is an empty comment, which serves to end this comment and\n", document.Elements[0].TextAreas[0].Content.Text);
            Assert.Equal("prevents the following block quote being swallowed up.\n", document.Elements[0].TextAreas[1].Content.Text);
            Assert.True(document.Elements[1].TypeCode == ElementType.Comment);
            Assert.Equal(0, document.Elements[1].TextAreas.Count);
            Assert.Equal("A block quote.\n", document.Elements[2].TextAreas[0].Content.Text);
            Assert.True(document.Elements[2].TypeCode == ElementType.BlockQuote);
        }

        [Fact]
        public void DefinitionList()
        {
            var document = TestUtils.Test("comment_definitionlist");
            Assert.Equal(1, document.Elements.Count);
            var list = (DefinitionList)document.Elements[0];
            {
                var item = list.Items[0];
                Assert.Equal("term 1", item.Term.TextAreas[0].Content.Text);
                Assert.Equal(2, item.Definition.Elements.Count);
                var paragraph = (Paragraph)item.Definition.Elements[0];
                Assert.Equal("definition 1\n", paragraph.TextAreas[0].Content.Text);
                var comment = (Comment)item.Definition.Elements[1];
                Assert.Equal("a comment\n", comment.TextAreas[0].Content.Text);
            }
            {
                var item = list.Items[1];
                Assert.Equal("term 2", item.Term.TextAreas[0].Content.Text);
                Assert.Equal(1, item.Definition.Elements.Count);
                var paragraph = (Paragraph)item.Definition.Elements[0];
                Assert.Equal("definition 2\n", paragraph.TextAreas[0].Content.Text);
            }
        }
        
        [Fact]
        public void DefinitionListComment()
        {
            var document = TestUtils.Test("comment_definitionlist_comment");
            Assert.Equal(3, document.Elements.Count);
            {
                var list = (DefinitionList)document.Elements[0];
                var item = list.Items[0];
                Assert.Equal("term 1", item.Term.TextAreas[0].Content.Text);
                Assert.Equal(1, item.Definition.Elements.Count);
                var paragraph = (Paragraph)item.Definition.Elements[0];
                Assert.Equal("definition 1\n", paragraph.TextAreas[0].Content.Text);
            }
            var comment = (Comment)document.Elements[1];
            Assert.Equal("a comment\n", comment.TextAreas[0].Content.Text);
            {
                var list = (DefinitionList)document.Elements[2];
                var item = list.Items[0];
                Assert.Equal("term 2", item.Term.TextAreas[0].Content.Text);
                Assert.Equal(1, item.Definition.Elements.Count);
                var paragraph = (Paragraph)item.Definition.Elements[0];
                Assert.Equal("definition 2\n", paragraph.TextAreas[0].Content.Text);
            }
        }

        [Fact]
        public void BulletList()
        {
            var document = TestUtils.Test("comment_bulletlist");
            Assert.Equal(1, document.Elements.Count);
            var list = (BulletList)document.Elements[0];

            var item = list.Items[0];
            Assert.Equal('+', item.Start);
            {
                var paragraph = (Paragraph)item.Elements[0];
                Assert.Equal("bullet paragraph 1\n", paragraph.TextAreas[0].Content.Text);
            }
            {
                var paragraph = (Paragraph)item.Elements[1];
                Assert.Equal("bullet paragraph 2\n", paragraph.TextAreas[0].Content.Text);
            }

            var comment = (Comment)item.Elements[2];
            Assert.Equal("comment between bullet paragraphs 2 and 3\n", comment.TextAreas[0].Content.Text);

            {
                var paragraph = (Paragraph)item.Elements[3];
                Assert.Equal("bullet paragraph 3\n", paragraph.TextAreas[0].Content.Text);
            }
        }

        [Fact]
        public void BulletListComment()
        {
            var document = TestUtils.Test("comment_bulletlist_comment");
            Assert.Equal(1, document.Elements.Count);
            var list = (BulletList)document.Elements[0];

            var item = list.Items[0];
            Assert.Equal('+', item.Start);
            {
                var paragraph = (Paragraph)item.Elements[0];
                Assert.Equal("bullet paragraph 1\n", paragraph.TextAreas[0].Content.Text);
            }

            var comment = (Comment)item.Elements[1];
            Assert.Equal("comment between bullet paragraphs 1 (leader) and 2\n", comment.TextAreas[0].Content.Text);

            {
                var paragraph = (Paragraph)item.Elements[2];
                Assert.Equal("bullet paragraph 2\n", paragraph.TextAreas[0].Content.Text);
            }
        }

        [Fact]
        public void BulletListTrailing()
        {
            var document = TestUtils.Test("comment_bulletlist_trailing");
            Assert.Equal(1, document.Elements.Count);
            var list = (BulletList)document.Elements[0];

            var item = list.Items[0];
            Assert.Equal('+', item.Start);
            {
                var paragraph = (Paragraph)item.Elements[0];
                Assert.Equal("bullet\n", paragraph.TextAreas[0].Content.Text);
            }

            var comment = (Comment)item.Elements[1];
            Assert.Equal("trailing comment\n", comment.TextAreas[0].Content.Text);
        }
    }
}

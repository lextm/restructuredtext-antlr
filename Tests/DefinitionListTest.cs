using Xunit;

namespace Lextm.ReStructuredText.Tests
{
    public class DefinitionListTest
    {
        [Fact]
        public void Simple()
        {
            var document = TestUtils.Test("definitionlist_simple");
            Assert.Equal(1, document.Elements.Count);
            DefinitionList list = (DefinitionList)document.Elements[0];
            Assert.Equal(1, list.Items.Count);
            DefinitionListItem item = list.Items[0];
            var term = item.Term;
            Assert.Equal(1, term.TextAreas.Count);
            Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
            Assert.Equal("term", term.TextAreas[0].Content.Text);
            var definition = item.Definition;
            Assert.Equal(1, definition.Elements.Count);
            var paragraph = (Paragraph)definition.Elements[0];
            Assert.Equal("definition\n", paragraph.TextAreas[0].Content.Text);
        }

        [Fact]
        public void Paragraph()
        {
            var document = TestUtils.Test("definitionlist_paragraph");
            Assert.Equal(2, document.Elements.Count);
            DefinitionList list = (DefinitionList)document.Elements[0];
            Assert.Equal(1, list.Items.Count);
            DefinitionListItem item = list.Items[0];
            var term = item.Term;
            Assert.Equal(1, term.TextAreas.Count);
            Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
            Assert.Equal("term", term.TextAreas[0].Content.Text);
            var definition = item.Definition;
            Assert.Equal(1, definition.Elements.Count);
            var paragraph = (Paragraph)definition.Elements[0];
            Assert.Equal("definition\n", paragraph.TextAreas[0].Content.Text);

            var paragraph2 = (Paragraph)document.Elements[1];
            Assert.Equal("paragraph\n", paragraph2.TextAreas[0].Content.Text);
        }

        [Fact]
        public void Two()
        {
            var document = TestUtils.Test("definitionlist_two");
            Assert.Equal(1, document.Elements.Count);
            DefinitionList list = (DefinitionList)document.Elements[0];
            Assert.Equal(2, list.Items.Count);
            {
                DefinitionListItem item = list.Items[0];
                var term = item.Term;
                Assert.Equal(1, term.TextAreas.Count);
                Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
                Assert.Equal("term 1", term.TextAreas[0].Content.Text);
                var definition = item.Definition;
                Assert.Equal(1, definition.Elements.Count);
                var paragraph = (Paragraph)definition.Elements[0];
                Assert.Equal("definition 1\n", paragraph.TextAreas[0].Content.Text);
            }
            {
                DefinitionListItem item = list.Items[1];
                var term = item.Term;
                Assert.Equal(1, term.TextAreas.Count);
                Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
                Assert.Equal("term 2", term.TextAreas[0].Content.Text);
                var definition = item.Definition;
                Assert.Equal(1, definition.Elements.Count);
                var paragraph = (Paragraph)definition.Elements[0];
                Assert.Equal("definition 2\n", paragraph.TextAreas[0].Content.Text);
            }
        }

        [Fact]
        public void TwoNoBlankLine()
        {
            var document = TestUtils.Test("definitionlist_two_noblankline");
            Assert.Equal(1, document.Elements.Count);
            DefinitionList list = (DefinitionList)document.Elements[0];
            Assert.Equal(2, list.Items.Count);
            {
                DefinitionListItem item = list.Items[0];
                var term = item.Term;
                Assert.Equal(1, term.TextAreas.Count);
                Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
                Assert.Equal("term 1", term.TextAreas[0].Content.Text);
                var definition = item.Definition;
                Assert.Equal(1, definition.Elements.Count);
                var paragraph = (Paragraph)definition.Elements[0];
                Assert.Equal("definition 1 (no blank line below)\n", paragraph.TextAreas[0].Content.Text);
            }
            {
                DefinitionListItem item = list.Items[1];
                var term = item.Term;
                Assert.Equal(1, term.TextAreas.Count);
                Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
                Assert.Equal("term 2", term.TextAreas[0].Content.Text);
                var definition = item.Definition;
                Assert.Equal(1, definition.Elements.Count);
                var paragraph = (Paragraph)definition.Elements[0];
                Assert.Equal("definition 2\n", paragraph.TextAreas[0].Content.Text);
            }
        }

        [Fact]
        public void Nested()
        {
            var document = TestUtils.Test("definitionlist_nested");
            Assert.Equal(2, document.Elements.Count);
            DefinitionList list = (DefinitionList)document.Elements[0];
            Assert.Equal(2, list.Items.Count);
            {
                DefinitionListItem item = list.Items[0];
                var term = item.Term;
                Assert.Equal(1, term.TextAreas.Count);
                Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
                Assert.Equal("term 1", term.TextAreas[0].Content.Text);
                var definition = item.Definition;
                Assert.Equal(2, definition.Elements.Count);
                var paragraph = (Paragraph)definition.Elements[0];
                Assert.Equal("definition 1\n", paragraph.TextAreas[0].Content.Text);
            }
            {
                DefinitionListItem item = list.Items[1];
                var term = item.Term;
                Assert.Equal(1, term.TextAreas.Count);
                Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
                Assert.Equal("term 2", term.TextAreas[0].Content.Text);
                var definition = item.Definition;
                Assert.Equal(1, definition.Elements.Count);
                var paragraph = (Paragraph)definition.Elements[0];
                Assert.Equal("definition 2\n", paragraph.TextAreas[0].Content.Text);
            }

            var paragraph2 = (Paragraph)document.Elements[1];
            Assert.Equal("paragraph\n", paragraph2.TextAreas[0].Content.Text);
        }

        [Fact]
        public void Classifier()
        {
            var document = TestUtils.Test("definitionlist_classifier");
            Assert.Equal(1, document.Elements.Count);
            DefinitionList list = (DefinitionList)document.Elements[0];
            Assert.Equal(1, list.Items.Count);
            DefinitionListItem item = list.Items[0];
            var term = item.Term;
            Assert.Equal(1, term.TextAreas.Count);
            Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
            Assert.Equal("Term", term.TextAreas[0].Content.Text);
            var classifier = item.Classifiers[0];
            Assert.Equal(1, classifier.TextAreas.Count);
            Assert.Equal(ElementType.Text, classifier.TextAreas[0].TypeCode);
            Assert.Equal("classifier", classifier.TextAreas[0].Content.Text);
            var definition = item.Definition;
            Assert.Equal(1, definition.Elements.Count);
            var paragraph = (Paragraph)definition.Elements[0];
            Assert.Equal("The ' : ' indicates a classifier in\n", paragraph.TextAreas[0].Content.Text);
            Assert.Equal("definition list item terms only.\n", paragraph.TextAreas[1].Content.Text);
        }

        [Fact]
        public void WithoutClassifier()
        {
            var document = TestUtils.Test("definitionlist_classifier_without");
            Assert.Equal(1, document.Elements.Count);
            DefinitionList list = (DefinitionList)document.Elements[0];
            Assert.Equal(3, list.Items.Count);
            {
                DefinitionListItem item = list.Items[0];
                var term = item.Term;
                Assert.Equal(1, term.TextAreas.Count);
                Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
                Assert.Equal("Term: not a classifier", term.TextAreas[0].Content.Text);
                Assert.Equal(0, item.Classifiers.Count);
                var definition = item.Definition;
                Assert.Equal(1, definition.Elements.Count);
                var paragraph = (Paragraph)definition.Elements[0];
                Assert.Equal("Because there\'s no space before the colon.\n", paragraph.TextAreas[0].Content.Text);
            }

            {
                DefinitionListItem item = list.Items[1];
                var term = item.Term;
                Assert.Equal(1, term.TextAreas.Count);
                Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
                Assert.Equal("Term :not a classifier", term.TextAreas[0].Content.Text);
                Assert.Equal(0, item.Classifiers.Count);
                var definition = item.Definition;
                Assert.Equal(1, definition.Elements.Count);
                var paragraph = (Paragraph)definition.Elements[0];
                Assert.Equal("Because there\'s no space after the colon.\n", paragraph.TextAreas[0].Content.Text);
            }

            {
                DefinitionListItem item = list.Items[2];
                var term = item.Term;
                Assert.Equal(1, term.TextAreas.Count);
                Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
                Assert.Equal("Term \\: not a classifier", term.TextAreas[0].Content.Text);
                Assert.Equal(0, item.Classifiers.Count);
                var definition = item.Definition;
                Assert.Equal(1, definition.Elements.Count);
                var paragraph = (Paragraph)definition.Elements[0];
                Assert.Equal("Because the colon is escaped.\n", paragraph.TextAreas[0].Content.Text);
            }
        }

        [Fact]
        public void ClassifierLiteral()
        {
            var document = TestUtils.Test("definitionlist_classifier_literal");
            Assert.Equal(1, document.Elements.Count);
            DefinitionList list = (DefinitionList)document.Elements[0];
            Assert.Equal(1, list.Items.Count);
            DefinitionListItem item = list.Items[0];
            var term = item.Term;
            Assert.Equal(1, term.TextAreas.Count);
            Assert.Equal(ElementType.Literal, term.TextAreas[0].TypeCode);
            Assert.Equal("Term : not a classifier", term.TextAreas[0].Content.Text);
            Assert.Equal(0, item.Classifiers.Count);
            var definition = item.Definition;
            Assert.Equal(1, definition.Elements.Count);
            var paragraph = (Paragraph)definition.Elements[0];
            Assert.Equal("Because the ' : ' is inside an inline literal.\n", paragraph.TextAreas[0].Content.Text);
        }

        [Fact]
        public void ClassifierMultiple()
        {
            var document = TestUtils.Test("definitionlist_classifier_multiple");
            Assert.Equal(1, document.Elements.Count);
            DefinitionList list = (DefinitionList)document.Elements[0];
            Assert.Equal(1, list.Items.Count);
            DefinitionListItem item = list.Items[0];
            var term = item.Term;
            Assert.Equal(1, term.TextAreas.Count);
            Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
            Assert.Equal("Term", term.TextAreas[0].Content.Text);
            Assert.Equal(2, item.Classifiers.Count);
            Assert.Equal("classifier one", item.Classifiers[0].TextAreas[0].Content.Text);
            Assert.Equal("classifier two", item.Classifiers[1].TextAreas[0].Content.Text);
            var definition = item.Definition;
            Assert.Equal(1, definition.Elements.Count);
            var paragraph = (Paragraph)definition.Elements[0];
            Assert.Equal("Definition\n", paragraph.TextAreas[0].Content.Text);
        }

        [Fact]
        public void ClassifierMultipleInlineMarkup()
        {
            var document = TestUtils.Test("definitionlist_classifier_multiple_inlinemarkup");
            Assert.Equal(1, document.Elements.Count);
            DefinitionList list = (DefinitionList)document.Elements[0];
            Assert.Equal(1, list.Items.Count);
            DefinitionListItem item = list.Items[0];
            var term = item.Term;
            Assert.Equal(1, term.TextAreas.Count);
            Assert.Equal(ElementType.Text, term.TextAreas[0].TypeCode);
            Assert.Equal("Term", term.TextAreas[0].Content.Text);
            Assert.Equal(2, item.Classifiers.Count);
            Assert.Equal(ElementType.Strong, item.Classifiers[0].TextAreas[0].TypeCode);
            Assert.Equal("classifier one", item.Classifiers[0].TextAreas[0].Content.Text);
            Assert.Equal("classifier two", item.Classifiers[1].TextAreas[0].Content.Text);
            var definition = item.Definition;
            Assert.Equal(1, definition.Elements.Count);
            var paragraph = (Paragraph)definition.Elements[0];
            Assert.Equal("Definition\n", paragraph.TextAreas[0].Content.Text);
        }
    }
}

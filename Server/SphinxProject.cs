using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Server;

namespace Lextm.ReStructuredText.LanguageServer
{
    public class SphinxProject
    {
        private string _root;

        public void Refresh(ReStructuredTextSettings sessionSettings)
        {
            // TODO: check conf.py for include files.
             var setting = sessionSettings.ConfPath;
             var workspaceRoot = sessionSettings.WorkspaceRoot;
             _root = setting.Replace("${workspaceRoot}", workspaceRoot);
        }

        public void RefreshDocument(TextDocument doc)
        {
//            var key = doc.Uri.ToString();
//            if (!Files.ContainsKey(key))
//            {
//                var path = key;
//                Files.Add(key, path);
//            }
        }

        public CompletionList GetCompletionList(DocumentState textDocument, Position position)
        {
            var document = textDocument.LintedDocument;
            var element = document.Find(position.Line + 1, position.Character);
            if (element == null || element.TypeCode != ElementType.Paragraph)
            {
                return new CompletionList();
            }

            if (element.Parent == null || element.Parent.TypeCode != ElementType.ListItem)
            {
                return new CompletionList();
            }

            var text = element.TextAreas[0];
            if (text is InterpretedText interpreted)
            {
                if (interpreted.RoleName != "doc")
                {
                    return new CompletionList();
                }
            }
            else if (!text.Content.Text.StartsWith(":doc:", StringComparison.Ordinal))
            {
                return new CompletionList();
            }

            var Files = new Dictionary<string, string>();
            foreach (string file in Directory.EnumerateFiles(
                _root, "*.rst", SearchOption.AllDirectories))
            {
                Files.Add(GetPath(file), file);
            }

            foreach (string file in Directory.EnumerateFiles(
                _root, "*.rest", SearchOption.AllDirectories))
            {
                Files.Add(GetPath(file), file);
            }

            return new CompletionList(Files.Select(_ =>
                new CompletionItem(_.Key, CompletionItemKind.Text, _.Value, null)), true);
        }

        private string GetPath(string file)
        {
            var part = file.Substring(_root.Length);
            return part.TrimStart('\\', '/').Replace('\\', '/').Replace(".rst", null).Replace(".rest", null);
        }
    }
}
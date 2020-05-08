using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Server;

namespace Lextm.ReStructuredText.LanguageServer
{
    public class SphinxProject
    {
        public void RefreshDocument(TextDocument doc)
        {
//            var key = doc.Uri.ToString();
//            if (!Files.ContainsKey(key))
//            {
//                var path = key;
//                Files.Add(key, path);
//            }
        }

        public CompletionList GetCompletionList(DocumentState textDocument, Position position, CompletionContext context)
        {
            var document = textDocument.LintedDocument;
            if (context.TriggerKind != CompletionTriggerKind.TriggerCharacter
                || context.TriggerCharacter != '/'
                || !document.TriggerDocumentList(position.Line, position.Character))
            {
                return new CompletionList();
            }

            var Files = new Dictionary<string, string>();
            foreach (string file in Directory.EnumerateFiles(
                WorkspaceRoot, "*.rst", SearchOption.AllDirectories))
            {
                Files.Add(GetPath(file), file);
            }

            foreach (string file in Directory.EnumerateFiles(
                WorkspaceRoot, "*.rest", SearchOption.AllDirectories))
            {
                Files.Add(GetPath(file), file);
            }

            bool incomplete = Files.Count > 50;
            return new CompletionList(Files.Select(_ =>
                new CompletionItem(_.Key, CompletionItemKind.File, _.Value, null)).Take(50), incomplete);
        }

        private string GetPath(string file)
        {
            var part = file.Substring(WorkspaceRoot.Length);
            return part.TrimStart('\\', '/').Replace('\\', '/').Replace(".rst", null).Replace(".rest", null);
        }

        public string WorkspaceRoot { get; set; }
    }
}
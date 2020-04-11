﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Server;

namespace Lextm.ReStructuredText.LanguageServer
{
    public class SphinxProject
    {
        string _root;

        public void Refresh(ReStructuredTextSettings sessionSettings)
        {
            // TODO: check conf.py for include files.
             var setting = sessionSettings.ConfPath;
             _root = setting.Replace("${workspaceFolder}", WorkspaceRoot);
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
            if (!document.TriggerDocumentList(position.Line, position.Character))
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

        public string WorkspaceRoot { get; set; }
    }
}
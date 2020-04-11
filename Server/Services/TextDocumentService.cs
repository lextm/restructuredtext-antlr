using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JsonRpc.Contracts;
using LanguageServer.VsCode;
using LanguageServer.VsCode.Contracts;

namespace Lextm.ReStructuredText.LanguageServer.Services
{
    [JsonRpcScope(MethodPrefix = "textDocument/")]
    public class TextDocumentService : LanguageServiceBase
    {
        [JsonRpcMethod]
        public async Task<Hover> Hover(TextDocumentIdentifier textDocument, Position position, CancellationToken ct)
        {
            // Note that Hover is cancellable.
            await Task.Delay(1000, ct);
            return new Hover {Contents = "Test _hover_ @" + position + "\n\n" + textDocument};
        }

        [JsonRpcMethod]
        public SignatureHelp SignatureHelp(TextDocumentIdentifier textDocument, Position position)
        {
            return new SignatureHelp(new List<SignatureInformation>
            {
                new SignatureInformation("**Function1**", "Documentation1"),
                new SignatureInformation("**Function2** <strong>test</strong>", "Documentation2"),
            });
        }

        [JsonRpcMethod(IsNotification = true)]
        public void DidOpen(TextDocumentItem textDocument)
        {
            var doc = Session.AddOrUpdateDocument(textDocument);
            doc.RequestAnalysis();
        }

        [JsonRpcMethod(IsNotification = true)]
        public void DidChange(TextDocumentIdentifier textDocument,
            ICollection<TextDocumentContentChangeEvent> contentChanges)
        {
            Session.DocumentStates[textDocument.Uri].NotifyChanges(contentChanges);
        }

        [JsonRpcMethod(IsNotification = true)]
        public void WillSave(TextDocumentIdentifier textDocument, TextDocumentSaveReason reason)
        {
        }

        [JsonRpcMethod(IsNotification = true)]
        public async Task DidClose(TextDocumentIdentifier textDocument)
        {
            Session.RemoveDocument(textDocument.Uri);
            if (textDocument.Uri.IsUntitled())
            {
                await Session.Client.Document.PublishDiagnostics(textDocument.Uri, new Diagnostic[0]);
            }
        }


        [JsonRpcMethod]
        public CompletionList Completion(TextDocumentIdentifier textDocument, Position position, CompletionContext context)
        {
            var doc = Session.DocumentStates[textDocument.Uri];
            doc.AnalyzeAsync();
            return 
                Session.Project.GetCompletionList(doc, position, context); //position, Session.PageInfoStore));
        }
    }
}
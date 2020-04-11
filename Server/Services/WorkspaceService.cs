using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JsonRpc.Contracts;
using LanguageServer.VsCode.Contracts;
namespace Lextm.ReStructuredText.LanguageServer.Services
{
    [JsonRpcScope(MethodPrefix = "workspace/")]
    public class WorkspaceService : LanguageServiceBase
    {
        [JsonRpcMethod(IsNotification = true)]
        public async Task DidChangeConfiguration(SettingsRoot settings)
        {
            Session.Settings = settings.ReStructuredText;
            Session.Project.Refresh(Session.Settings);
            foreach (var doc in Session.DocumentStates.Values)
            {
                //Session.Project.RefreshDocument(doc.Document);
//                var diag = Session.DiagnosticProvider.LintDocument(doc.Document, Session.Settings.LanguageServer.MaxNumberOfProblems);
//                await Client.Document.PublishDiagnostics(doc.Document.Uri, diag);
            }
        }

        [JsonRpcMethod(IsNotification = true)]
        public async Task DidChangeWatchedFiles(ICollection<FileEvent> changes)
        {
            foreach (var change in changes)
            {
                if (!change.Uri.IsFile) continue;
                var localPath = change.Uri.AbsolutePath;
                if (string.Equals(Path.GetExtension(localPath), ".rst") || string.Equals(Path.GetExtension(localPath), ".rest"))
                {
                    // If the file has been removed, we will clear the lint result about it.
                    // Note that pass null to PublishDiagnostics may mess up the client.
                    if (change.Type == FileChangeType.Deleted)
                    {
                        await Session.Client.Document.PublishDiagnostics(change.Uri, new Diagnostic[0]);
                    }
                }
            }
        }
    }
}

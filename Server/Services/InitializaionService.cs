using System;
using JsonRpc.Standard;
using JsonRpc.Standard.Contracts;
using JsonRpc.Standard.Server;
using LanguageServer.VsCode.Contracts;
using Newtonsoft.Json.Linq;

namespace Lextm.ReStructuredText.LanguageServer.Services
{
    public class InitializaionService : LanguageServiceBase
    {
        [JsonRpcMethod(AllowExtensionData = true)]
        public InitializeResult Initialize(int processId, Uri rootUri, ClientCapabilities capabilities,
            JToken initializationOptions = null, string trace = null)
        {
            // inform the language server client of server capabilities.
            return new InitializeResult(new ServerCapabilities
            {
                //HoverProvider = true,
                //SignatureHelpProvider = new SignatureHelpOptions(new[] {'(', ')'}),
                CompletionProvider = new CompletionOptions(true, new[]{'/'}),
                TextDocumentSync = new TextDocumentSyncOptions
                {
                    OpenClose = true,
                    WillSave = true,
                    Change = TextDocumentSyncKind.Incremental
                }
            });
        }

        [JsonRpcMethod(IsNotification = true)]
        public void Initialized()
        {
        }

        [JsonRpcMethod]
        public void Shutdown()
        {

        }

        [JsonRpcMethod(IsNotification = true)]
        public void Exit()
        {
            Session.StopServer();
        }

        [JsonRpcMethod("$/cancelRequest", IsNotification = true)]
        public void CancelRequest(MessageId id)
        {
            RequestContext.Features.Get<IRequestCancellationFeature>()?.TryCancel(id);
        }
    }
}
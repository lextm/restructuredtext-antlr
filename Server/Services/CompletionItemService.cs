using JsonRpc.Standard.Contracts;
using LanguageServer.VsCode.Contracts;

namespace Lextm.ReStructuredText.LanguageServer.Services
{
    [JsonRpcScope(MethodPrefix = "completionItem/")]
    public class CompletionItemService : LanguageServiceBase
    {
        // The request is sent from the client to the server to resolve additional information
        // for a given completion item.
        [JsonRpcMethod(AllowExtensionData = true)]
        public CompletionItem Resolve()
        {
            var item = RequestContext.Request.Parameters.ToObject<CompletionItem>(Utility.CamelCaseJsonSerializer);
            // Add a pair of square brackets around the inserted text.
            item.InsertText = $"{item.Label}";
            return item;
        }
    }
}
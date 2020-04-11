using JsonRpc.Server;

namespace Lextm.ReStructuredText.LanguageServer.Services
{
    public class LanguageServiceBase : JsonRpcService
    {
        protected SessionState Session => RequestContext.Features.Get<SessionState>();
    }
}
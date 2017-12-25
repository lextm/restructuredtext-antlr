using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lextm.ReStructuredText.LanguageServer
{
    internal static class Utility
    {
        public static readonly JsonSerializer CamelCaseJsonSerializer = new JsonSerializer
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
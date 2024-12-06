using Microsoft.Extensions.DependencyInjection;
using SearchSphere.Common.Services;
using SearchSphere.Integrations.DocumentIntelligenceClient.Interfaces;
using SearchSphere.Integrations.DocumentIntelligenceClient.Options;

namespace SearchSphere.Integrations.DocumentIntelligenceClient
{
    public static class DepInj
    {
        public static void RegisterDocumentIntelligenceClient(
            this IServiceCollection service,
            Action<DocumentIntelligenceOptions> configureOptions)
        {
            service.ConfigureServiceOptions<DocumentIntelligenceOptions>((_, opt) => configureOptions(opt));
            service.AddTransient<IDocumentIntelligenceClient, DocumentIntelligenceClient>();
        }
    }
}
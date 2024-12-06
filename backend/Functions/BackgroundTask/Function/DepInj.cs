using Microsoft.Extensions.DependencyInjection;
using SearchSphere.Functions.BackgroundTask.Service;
using SearchSphere.Functions.BackgroundTask.Service.Interfaces;
using SearchSphere.Integrations.CosmosDbClient;
using SearchSphere.Integrations.CosmosDbClient.Options;
using SearchSphere.Integrations.DocumentIntelligenceClient;
using SearchSphere.Integrations.DocumentIntelligenceClient.Options;

namespace SearchSphere.Functions.BackgroundTask
{
    public static class DepInj
    {
        public static void RegisterServices(
            this IServiceCollection services,
            Action<CosmosDbClientOptions> configureCosmos, 
            Action<DocumentIntelligenceOptions> configureDocumentIntelligenceOptions)
        {
            //
            services.RegisterCosmosDbClient(configureCosmos);
            services.RegisterDocumentIntelligenceClient(configureDocumentIntelligenceOptions);
            //
            services.AddTransient<ICosmosService, CosmosService>();
            services.AddTransient<IExtractTextService, ExtractTextService>();
        }
    }
}
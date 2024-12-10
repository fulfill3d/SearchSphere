using Microsoft.Extensions.DependencyInjection;
using SearchSphere.Common.Services;
using SearchSphere.Functions.BackgroundTask.Service;
using SearchSphere.Functions.BackgroundTask.Service.Interfaces;
using SearchSphere.Functions.BackgroundTask.Service.Options;
using SearchSphere.Integrations.AzureOpenAiClient;
using SearchSphere.Integrations.AzureOpenAiClient.Options;
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
            Action<DocumentIntelligenceOptions> configureDocumentIntelligenceOptions, 
            Action<AzureOpenAiOptions> configureAzureOpenAiOptions,
            Action<SlidingWindowOptions> configureSlidingWindowOptions)
        {
            //
            services.ConfigureServiceOptions<SlidingWindowOptions>((_, opt) => configureSlidingWindowOptions(opt));
            //
            services.RegisterCosmosDbClient(configureCosmos);
            services.RegisterDocumentIntelligenceClient(configureDocumentIntelligenceOptions);
            services.RegisterAzureOpenAiClient(configureAzureOpenAiOptions);
            //
            services.AddTransient<IAzureOpenAiService, AzureOpenAiService>();
            services.AddTransient<ICosmosService, CosmosService>();
            services.AddTransient<IDocumentIntelligenceService, DocumentIntelligenceService>();
            services.AddTransient<IExtractTextService, ExtractTextService>();
        }
    }
}
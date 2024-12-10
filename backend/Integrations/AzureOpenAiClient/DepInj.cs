using Microsoft.Extensions.DependencyInjection;
using SearchSphere.Common.Services;
using SearchSphere.Integrations.AzureOpenAiClient.Interfaces;
using SearchSphere.Integrations.AzureOpenAiClient.Options;

namespace SearchSphere.Integrations.AzureOpenAiClient
{
    public static class DepInj
    {
        public static void RegisterAzureOpenAiClient(
            this IServiceCollection service,
            Action<AzureOpenAiOptions> configureOptions)
        {
            service.ConfigureServiceOptions<AzureOpenAiOptions>((_, opt) => configureOptions(opt));
            service.AddTransient<IAzureOpenAiClient, AzureOpenAiClient>();
        }
    }
}
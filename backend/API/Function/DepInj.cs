using Microsoft.Extensions.DependencyInjection;
using SearchSphere.API.Service;
using SearchSphere.API.Service.Interfaces;
using SearchSphere.API.Service.Options;
using SearchSphere.Common.Services;
using SearchSphere.Integrations.BlobClient.Options;
using SearchSphere.Integrations.CosmosDbClient.Options;

namespace SearchSphere.API
{
    public static class DepInj
    {
        public static void RegisterServices(
            this IServiceCollection services, 
            Action<CosmosDbClientOptions> configureCosmos, 
            Action<BlobClientConfiguration> configureBlob, Action<BlobServiceOptions> configureOpt)
        {
            //
            services.ConfigureServiceOptions<CosmosDbClientOptions>((_, options) => configureCosmos(options));
            services.ConfigureServiceOptions<BlobClientConfiguration>((_, options) => configureBlob(options));
            services.ConfigureServiceOptions<BlobServiceOptions>((_, options) => configureOpt(options));
            //
            services.AddTransient<ICosmosService, CosmosService>();
            services.AddTransient<IBlobService, BlobService>();
            services.AddTransient<IFileService, FileService>();
        }
    }
}
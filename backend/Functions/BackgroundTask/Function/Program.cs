using Azure.Identity;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SearchSphere.Functions.BackgroundTask;

var builder = FunctionsApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

// Configure Azure App Configuration and Key Vault
var token = new DefaultAzureCredential();
var appConfigUrl = builder.Configuration["AppConfigUrl"] ?? string.Empty;

configuration.AddAzureAppConfiguration(config =>
{
    config.Connect(new Uri(appConfigUrl), token);
    config.ConfigureKeyVault(kv => kv.SetCredential(token));
});

services.RegisterServices(options =>
{
    options.EndpointUri = configuration["Fulfill3dCosmosEndpointUri"] ?? string.Empty;
    options.PrimaryKey = configuration["Fulfill3dCosmosPrimaryKey"] ?? string.Empty;
    options.DatabaseId = configuration["SearchSphere_CosmosDbDatabaseId"] ?? string.Empty;
    options.ContainerId = configuration["SearchSphere_CosmosDbContainerId"] ?? string.Empty;
},conf =>
{
    conf.Endpoint = configuration["SearchSphere_DocumentIntelligenceEndpoint"] ?? string.Empty;
    conf.ApiKey = configuration["SearchSphere_DocumentIntelligenceApiKey"] ?? string.Empty;
},conf =>
{
    conf.Endpoint = configuration["SearchSphere_AzureOpenAiEndpoint"] ?? string.Empty;
    conf.ApiKey = configuration["SearchSphere_AzureOpenAiApiKey"] ?? string.Empty;
    conf.DeploymentName = configuration["SearchSphere_AzureOpenAiDeploymentName"] ?? string.Empty;
},conf =>
{
    conf.WindowSize = int.Parse(configuration["SearchSphere_SlidingWindowSize"] ?? string.Empty);
    conf.StepSize = int.Parse(configuration["SearchSphere_SlidingStepSize"] ?? string.Empty);
});

builder.Build().Run();
using Azure.Identity;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SearchSphere.API;

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

// Register services
services.RegisterServices(options =>
{
    options.EndpointUri = builder.Configuration["Fulfill3dCosmosEndpointUri"] ?? string.Empty;
    options.PrimaryKey = builder.Configuration["Fulfill3dCosmosPrimaryKey"] ?? string.Empty;
    options.DatabaseId = builder.Configuration["SearchSphere_CosmosDbDatabaseId"] ?? string.Empty;
    options.ContainerId = builder.Configuration["SearchSphere_CosmosDbContainerId"] ?? string.Empty;
}, options =>
{
    options.ConnectionString = builder.Configuration["BlobConnectionString"] ?? string.Empty;
}, options =>
{
    options.BlobContainerName = builder.Configuration["SearchSphere_BlobContainerName"] ?? string.Empty;
});

// Build and run the application
var app = builder.Build();
app.Run();
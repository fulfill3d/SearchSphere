using SearchSphere.Functions.BackgroundTask.Data;
using SearchSphere.Functions.BackgroundTask.Service.Interfaces;
using SearchSphere.Integrations.CosmosDbClient.Interfaces;

namespace SearchSphere.Functions.BackgroundTask.Service
{
    public class CosmosService(ICosmosDbClient cosmosClient): ICosmosService
    {
        private const string PartitionKey = "text-content";
        public async Task<bool> SaveTextContent(TextContent content)
        {
            await cosmosClient.AddItemAsync(content, PartitionKey);
            return true;
        }
    }
}
using SearchSphere.API.Data;
using SearchSphere.API.Service.Interfaces;
using SearchSphere.Integrations.CosmosDbClient.Interfaces;

namespace SearchSphere.API.Service
{
    public class CosmosService(ICosmosDbClient cosmosClient): ICosmosService
    {
        private const string PartitionKey = "document-metadata";
        public async Task<bool> SaveDocumentMetadata(DocumentMetadata metadata)
        {
            await cosmosClient.AddItemAsync(metadata, PartitionKey);
            return true;
        }
    }
}
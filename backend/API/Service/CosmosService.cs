using Microsoft.Azure.Cosmos;
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

        public async Task<IEnumerable<DocumentMetadata>> GetDocumentMetadata(string oid)
        {
            var query = $"SELECT * FROM c WHERE c.oid = '{oid}'";
            return await cosmosClient.QueryItemsAsync<DocumentMetadata>(query);
        }
    }
}
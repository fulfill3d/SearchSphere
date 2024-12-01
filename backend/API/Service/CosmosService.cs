using SearchSphere.API.Data;
using SearchSphere.API.Service.Interfaces;
using SearchSphere.Integrations.CosmosDbClient.Interfaces;

namespace SearchSphere.API.Service
{
    public class CosmosService(ICosmosDbClient cosmosClient): ICosmosService
    {
        private const string PartitionKey = "document-metadata";
        public async Task<bool> SaveDocumentMetadata(string oid, string blobUrl, string fileName)
        {
            var data = new DocumentMetadata
            {
                Name = fileName,
                Url = blobUrl,
                OID = oid,
                UUID = Guid.NewGuid().ToString(),
                PartitionKey = PartitionKey
            };
            await cosmosClient.AddItemAsync(data, PartitionKey);
            return true;
        }
    }
}
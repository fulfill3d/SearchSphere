using SearchSphere.API.Data;
using SearchSphere.API.Service.Interfaces;
using SearchSphere.Integrations.CosmosDbClient.Interfaces;

namespace SearchSphere.API.Service
{
    public class CosmosService(ICosmosDbClient cosmosClient): ICosmosService
    {
        public async Task<bool> SaveDocumentMetadata(string oid, string blobUrl, string fileName)
        {
            var data = new DocumentMetadata
            {
                Name = blobUrl,
                Url = blobUrl,
                OID = oid,
                UUID = Guid.NewGuid().ToString()
            };
            await cosmosClient.AddItemAsync(data, "DocumentMetadata");
            return true;
        }
    }
}
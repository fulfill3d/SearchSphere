using SearchSphere.Functions.BackgroundTask.Data;
using SearchSphere.Functions.BackgroundTask.Service.Interfaces;
using SearchSphere.Integrations.DocumentIntelligenceClient.Interfaces;

namespace SearchSphere.Functions.BackgroundTask.Service
{
    public class ExtractTextService(
        IDocumentIntelligenceClient documentIntelligenceClient, 
        ICosmosService cosmosService): IExtractTextService
    {
        private const string PartitionKey = "text-content";
        public async Task<bool> ExtractText(string blobName, Stream input)
        {
            var text = await documentIntelligenceClient.ExtractText(input);

            var content = new TextContent
            {
                UUID = Guid.NewGuid().ToString(),
                BlobName = blobName,
                Content = text,
                PartitionKey = PartitionKey
            };
            
            return await cosmosService.SaveTextContent(content);
        }
    }
}
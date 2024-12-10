using SearchSphere.Functions.BackgroundTask.Service.Interfaces;
using SearchSphere.Integrations.DocumentIntelligenceClient.Interfaces;

namespace SearchSphere.Functions.BackgroundTask.Service
{
    public class DocumentIntelligenceService(IDocumentIntelligenceClient documentIntelligenceClient): IDocumentIntelligenceService
    {
        public async Task<string> ExtractText(Stream documentStream)
        {
            return await documentIntelligenceClient.ExtractText(documentStream);
        }
    }
}
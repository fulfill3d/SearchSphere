using SearchSphere.Functions.BackgroundTask.Service.Interfaces;
using SearchSphere.Integrations.AzureOpenAiClient.Interfaces;

namespace SearchSphere.Functions.BackgroundTask.Service
{
    public class AzureOpenAiService(IAzureOpenAiClient openAiClient): IAzureOpenAiService
    {
        public async Task<IEnumerable<float[]>> GetEmbeddings(IEnumerable<string> texts)
        {
            return await openAiClient.GetEmbeddings(texts);
        }
    }
}
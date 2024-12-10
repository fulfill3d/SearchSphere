using Azure;
using Azure.AI.OpenAI;
using SearchSphere.Integrations.AzureOpenAiClient.Interfaces;
using SearchSphere.Integrations.AzureOpenAiClient.Options;
using Microsoft.Extensions.Options;

namespace SearchSphere.Integrations.AzureOpenAiClient
{
    public class AzureOpenAiClient : IAzureOpenAiClient
    {
        private readonly AzureOpenAiOptions _options;
        private readonly OpenAIClient _client;

        public AzureOpenAiClient(IOptions<AzureOpenAiOptions> options)
        {
            _options = options.Value;
            _client = new OpenAIClient(
                new Uri(_options.Endpoint),
                new AzureKeyCredential(_options.ApiKey)
            );
        }

        public async Task<IEnumerable<float[]>> GetEmbeddings(IEnumerable<string> texts)
        {
            var tasks = texts.Select(async text =>
            {
                var embeddingOptions = new EmbeddingsOptions(text);
                var response = await _client.GetEmbeddingsAsync(_options.DeploymentName, embeddingOptions);
                return response.Value.Data[0].Embedding.ToArray();
            });

            return await Task.WhenAll(tasks);
        }
    }
}
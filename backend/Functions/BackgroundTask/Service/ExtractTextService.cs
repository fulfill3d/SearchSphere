using Microsoft.Extensions.Options;
using SearchSphere.Functions.BackgroundTask.Data;
using SearchSphere.Functions.BackgroundTask.Service.Interfaces;
using SearchSphere.Functions.BackgroundTask.Service.Options;

namespace SearchSphere.Functions.BackgroundTask.Service
{
    public class ExtractTextService(
        IDocumentIntelligenceService documentIntelligenceService, 
        ICosmosService cosmosService,
        IAzureOpenAiService azureOpenAiService,
        IOptions<SlidingWindowOptions> opt): IExtractTextService
    {
        private const string PartitionKey = "text-content";
        private readonly int _windowSize = opt.Value.WindowSize; // Number of characters in each fragment
        private readonly int _stepSize = opt.Value.StepSize; // Overlap between fragments

        public async Task<bool> ExtractText(string blobName, Stream input)
        {
            // Extract the full text
            var text = await documentIntelligenceService.ExtractText(input);

            // Sliding window parameters
            var fragments = GenerateSlidingWindows(text, _windowSize, _stepSize);

            var embeddings = await azureOpenAiService.GetEmbeddings(fragments);

            // Save each fragment to Cosmos DB
            var tasks = embeddings.Select(embedding =>
            {
                var contentFragment = new TextContentFragment
                {
                    UUID = Guid.NewGuid().ToString(),
                    BlobName = blobName,
                    Embedding = embedding,
                    PartitionKey = PartitionKey
                };
                return cosmosService.SaveTextContent(contentFragment);
            });

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);
            return true;
        }

        private static List<string> GenerateSlidingWindows(string text, int windowSize, int stepSize)
        {
            var words = text.Split(' ');
            var fragments = new List<string>();
            for (var i = 0; i < words.Length; i += stepSize)
            {
                var end = Math.Min(i + windowSize, words.Length);
                fragments.Add(string.Join(" ", words.Skip(i).Take(end - i)));
                if (end == words.Length)
                    break;
            }
            return fragments;
        }
    }
}

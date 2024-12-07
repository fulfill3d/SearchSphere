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
        private const int WindowSize = 400; // Number of characters in each fragment
        private const int StepSize = 50; // Overlap between fragments

        public async Task<bool> ExtractText(string blobName, Stream input)
        {
            // Extract the full text
            var text = await documentIntelligenceClient.ExtractText(input);

            // Sliding window parameters
            var fragments = GenerateSlidingWindows(text, WindowSize, StepSize);

            // Save each fragment to Cosmos DB
            var tasks = fragments.Select(fragment =>
            {
                var contentFragment = new TextContentFragment
                {
                    UUID = Guid.NewGuid().ToString(),
                    BlobName = blobName,
                    ContentFragment = fragment,
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
            var fragments = new List<string>();
            for (var i = 0; i < text.Length; i += stepSize)
            {
                var end = Math.Min(i + windowSize, text.Length);
                fragments.Add(text.Substring(i, end - i));
                if (end == text.Length)
                    break;
            }
            return fragments;
        }
    }
}

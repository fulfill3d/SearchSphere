using System.Text;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.Extensions.Options;
using SearchSphere.Integrations.DocumentIntelligenceClient.Interfaces;
using SearchSphere.Integrations.DocumentIntelligenceClient.Options;

namespace SearchSphere.Integrations.DocumentIntelligenceClient
{
    public class DocumentIntelligenceClient(IOptions<DocumentIntelligenceOptions> opt) : IDocumentIntelligenceClient
    {
        private readonly DocumentAnalysisClient _documentAnalysisClient = new (new Uri(opt.Value.Endpoint), new AzureKeyCredential(opt.Value.ApiKey));

        public async Task<string> ExtractText(Stream documentStream)
        {
            if (documentStream == null || documentStream.Length == 0)
                throw new ArgumentException("Document stream cannot be null or empty.", nameof(documentStream));

            StringBuilder extractedText = new StringBuilder();

            try
            {
                var operation = await _documentAnalysisClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-read", documentStream);

                foreach (var page in operation.Value.Pages)
                {
                    foreach (var line in page.Lines)
                    {
                        extractedText.AppendLine(line.Content);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to extract text using Document Intelligence.", ex);
            }

            return extractedText.ToString();
        }
    }
}

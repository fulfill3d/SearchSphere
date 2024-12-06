using Microsoft.Azure.Functions.Worker;
using SearchSphere.Functions.BackgroundTask.Service.Interfaces;

namespace SearchSphere.Functions.BackgroundTask
{
    public class Function(IExtractTextService extractTextService)
    {
        // Process document immediately after uploading into the storage
        [Function(nameof(BlobFunction))]
        public async Task BlobFunction(
            [BlobTrigger("search-sphere/{blobName}")] Stream fileStream, string blobName)
        {
            await extractTextService.ExtractText(blobName, fileStream);
        }
    }
}
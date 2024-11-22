using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SearchSphere.Integrations.BlobClient.Interface;
using SearchSphere.Integrations.BlobClient.Model;
using SearchSphere.Integrations.BlobClient.Options;

namespace SearchSphere.Integrations.BlobClient
{
    public class BlobClient(IOptions<BlobClientConfiguration> configuration, ILogger<BlobClient> logger) : IBlobClient
    {
        public async Task<string> Upload(Blob blob)
        {
            try
            {
                var blockBlobClient = await blob.GetBlockBlobClient(configuration.Value);

                await blockBlobClient.UploadAsync(blob.Stream, blob.Options);

                return blockBlobClient.Uri.AbsoluteUri;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error while saving blob " +
                                   $"'{blob.Name}' with type '{blob.Type}' " +
                                   $"to container '{blob.Container}'");
                return string.Empty;
            }
        }

        public async Task<Stream> Download(Blob blob)
        {
            var blockBlobClient = await blob.GetBlockBlobClient(configuration.Value);
            BlobDownloadInfo downloadInfo = await blockBlobClient.DownloadAsync();
            return downloadInfo.Content;
        }

        public async Task Delete(Blob blob)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(configuration.Value.ConnectionString);
                BlobContainerClient container = blobServiceClient.GetBlobContainerClient(blob.Container);
                var item = container.GetBlobClient(blob.Name);
                await item.DeleteIfExistsAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error while deleting {blob.Name} in container {blob.Container}");
            }
        }

    }
}
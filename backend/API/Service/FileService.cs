using HttpMultipartParser;
using SearchSphere.API.Data;
using SearchSphere.API.Service.Interfaces;

namespace SearchSphere.API.Service
{
    public class FileService(IBlobService blobService, ICosmosService cosmosService): IFileService
    {
        private const string PartitionKey = "document-metadata";
        public async Task<bool> UploadFile(string oid, FilePart file)
        {
            var url = await blobService.UploadFilePartToBlobStorage(file);
            var metadata = new DocumentMetadata
            {
                Name = file.FileName,
                Url = url,
                OID = oid,
                UUID = GetBlobNameFromUrl(url),
                PartitionKey = PartitionKey
            };
            return await cosmosService.SaveDocumentMetadata(metadata);
        }

        private static string GetBlobNameFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));
            }

            try
            {
                var uri = new Uri(url);
                // Extract the path part of the URL
                var path = uri.AbsolutePath;
                // Remove the leading slash if it exists
                var segments = path.TrimStart('/').Split('/');
                // The last segment is the blob name
                return segments.Length > 1 ? segments[^1] : string.Empty;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to extract blob name from URL.", ex);
            }
        }

    }
}
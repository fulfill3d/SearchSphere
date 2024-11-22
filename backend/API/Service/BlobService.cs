using HttpMultipartParser;
using Microsoft.Extensions.Options;
using SearchSphere.API.Service.Interfaces;
using SearchSphere.API.Service.Options;
using SearchSphere.Integrations.BlobClient.Enum;
using SearchSphere.Integrations.BlobClient.Interface;
using SearchSphere.Integrations.BlobClient.Model;

namespace SearchSphere.API.Service
{
    public class BlobService(IBlobClient blobClient, IOptions<BlobServiceOptions> opt): IBlobService
    {
        private readonly BlobServiceOptions options = opt.Value;
        public async Task<string> UploadFilePartToBlobStorage(FilePart filePart)
        {
            var blob = new Blob
            {
                Name = filePart.FileName,
                Type = ContentType.PDF,
                Container = options.BlobContainerName,
                Stream = filePart.Data,
            };

            return await blobClient.Upload(blob);
        }
    }
}
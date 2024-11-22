using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using SearchSphere.Integrations.BlobClient.Enum;

namespace SearchSphere.Integrations.BlobClient.Model
{
    public class Blob
    {
        public string Name { get; set; }
        public BlobUploadOptions Options { get; set; }
        
        private ContentType _type; // Backing field for Type
        public ContentType Type
        {
            get => _type;
            
            set
            {
                _type = value;
                
                Options = new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = value.ToString()
                    }
                };
            }
        }
        
        public string Container { get; set; }
        private Stream _stream;
        public Stream Stream 
        { 
            get => _stream;
            set => _stream = value;
        }
        
        public async Task<BlockBlobClient> GetBlockBlobClient(Options.BlobClientConfiguration configuration)
        {
            var cloudBlobContainer = await CreateBlobContainerIfNotExists(configuration);
            return cloudBlobContainer.GetBlockBlobClient(Name);
        }

        private async Task<BlobContainerClient> CreateBlobContainerIfNotExists(Options.BlobClientConfiguration configuration)
        {
            var blobContainerClient = new BlobContainerClient(configuration.ConnectionString, Container);
            var isContainerExists = await blobContainerClient.ExistsAsync();
            if (isContainerExists)
            {
                return blobContainerClient;
            }

            return await CreateBlobContainer(configuration);
        }
        
        private async Task<BlobContainerClient> CreateBlobContainer(Options.BlobClientConfiguration configuration)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(configuration.ConnectionString);
            // //COMMENT OUT FOR DEV STORAGE EMULATOR
            // await blobServiceClient.SetPropertiesAsync(new BlobServiceProperties()
            // {
            //     Cors = new List<BlobCorsRule>()
            //     {
            //         new BlobCorsRule()
            //         {
            //             AllowedHeaders = "*",
            //             AllowedMethods = "GET",
            //             AllowedOrigins = "*",
            //             MaxAgeInSeconds = 1 * 60 * 60,
            //         }
            //     }
            // });

            BlobContainerClient blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(Container);
            // //COMMENT OUT FOR DEV STORAGE EMULATOR
            // await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            return blobContainerClient;
        }
    }
}

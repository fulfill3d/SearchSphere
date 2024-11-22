using HttpMultipartParser;
using SearchSphere.API.Service.Interfaces;

namespace SearchSphere.API.Service
{
    public class FileService(IBlobService blobService, ICosmosService cosmosService): IFileService
    {
        public async Task<bool> UploadFile(string oid, FilePart file)
        {
            var url = await blobService.UploadFilePartToBlobStorage(file);
            return await cosmosService.SaveDocumentMetadata(oid, url, file.FileName);
        }
    }
}
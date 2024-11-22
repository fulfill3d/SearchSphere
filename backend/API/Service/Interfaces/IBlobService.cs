using HttpMultipartParser;

namespace SearchSphere.API.Service.Interfaces
{
    public interface IBlobService
    {
        Task<string> UploadFilePartToBlobStorage(FilePart filePart);
    }
}
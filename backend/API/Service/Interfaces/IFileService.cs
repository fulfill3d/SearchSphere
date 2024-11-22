using HttpMultipartParser;

namespace SearchSphere.API.Service.Interfaces
{
    public interface IFileService
    {
        Task<bool> UploadFile(string oid, FilePart file);
    }
}
using HttpMultipartParser;
using SearchSphere.API.Data;

namespace SearchSphere.API.Service.Interfaces
{
    public interface IFileService
    {
        Task<bool> UploadFile(string oid, FilePart file);
        Task<IEnumerable<DocumentMetadata>> GetFiles(string oid);
    }
}
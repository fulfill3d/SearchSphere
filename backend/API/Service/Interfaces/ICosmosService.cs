using SearchSphere.API.Data;

namespace SearchSphere.API.Service.Interfaces
{
    public interface ICosmosService
    {
        Task<bool> SaveDocumentMetadata(DocumentMetadata metadata);
    }
}
using SearchSphere.Functions.BackgroundTask.Data;

namespace SearchSphere.Functions.BackgroundTask.Service.Interfaces
{
    public interface ICosmosService
    {
        Task<bool> SaveTextContent(TextContentFragment contentFragment);
    }
}
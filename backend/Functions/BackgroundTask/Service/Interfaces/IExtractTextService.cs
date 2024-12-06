namespace SearchSphere.Functions.BackgroundTask.Service.Interfaces
{
    public interface IExtractTextService
    {
        Task<bool> ExtractText(string blobName, Stream input);
    }
}
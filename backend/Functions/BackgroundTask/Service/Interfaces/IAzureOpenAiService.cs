namespace SearchSphere.Functions.BackgroundTask.Service.Interfaces
{
    public interface IAzureOpenAiService
    {
        Task<IEnumerable<float[]>> GetEmbeddings(IEnumerable<string> texts);
    }
}
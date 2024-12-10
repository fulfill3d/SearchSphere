namespace SearchSphere.Integrations.AzureOpenAiClient.Interfaces
{
    public interface IAzureOpenAiClient
    {
        Task<IEnumerable<float[]>> GetEmbeddings(IEnumerable<string> texts);
    }
}
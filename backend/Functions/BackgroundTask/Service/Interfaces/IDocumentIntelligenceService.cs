namespace SearchSphere.Functions.BackgroundTask.Service.Interfaces
{
    public interface IDocumentIntelligenceService
    {
        Task<string> ExtractText(Stream documentStream);
    }
}
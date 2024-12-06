namespace SearchSphere.Integrations.DocumentIntelligenceClient.Interfaces
{
    public interface IDocumentIntelligenceClient
    {
        Task<string> ExtractText(Stream documentStream);
    }
}
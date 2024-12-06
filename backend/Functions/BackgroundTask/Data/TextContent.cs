using Newtonsoft.Json;

namespace SearchSphere.Functions.BackgroundTask.Data
{
    public class TextContent
    {
        [JsonProperty("id")] public string UUID { get; set; }
        [JsonProperty("blob-name")] public string BlobName { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("partitionKey")] public string PartitionKey { get; set; }
    }
}
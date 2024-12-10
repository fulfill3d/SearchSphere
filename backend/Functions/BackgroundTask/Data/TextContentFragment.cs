using Newtonsoft.Json;

namespace SearchSphere.Functions.BackgroundTask.Data
{
    public class TextContentFragment
    {
        [JsonProperty("id")] public string UUID { get; set; }
        [JsonProperty("blob-name")] public string BlobName { get; set; }
        [JsonProperty("embedding")] public float[] Embedding { get; set; }
        [JsonProperty("partitionKey")] public string PartitionKey { get; set; }
    }
}
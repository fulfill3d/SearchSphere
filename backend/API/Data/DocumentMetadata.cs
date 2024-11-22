using Newtonsoft.Json;

namespace SearchSphere.API.Data
{
    public class DocumentMetadata
    {
        [JsonProperty("id")] public string UUID { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("oid")] public string OID { get; set; }
        [JsonProperty("url")] public string Url { get; set; }
    }
}
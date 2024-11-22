using Microsoft.Azure.Functions.Worker;

namespace SearchSphere.Functions.BackgroundTask
{
    public class Function
    {
        // Process document immediately after uploading into the storage
        [Function(nameof(BlobFunction))]
        public async Task BlobFunction(
            [BlobTrigger("template-container/{name}")] Stream myTriggerItem,
            string name,
            FunctionContext context)
        {
            // await templateService.TemplateServiceMethod();
        }
    }
}
using System.Net;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using SearchSphere.API.Service.Interfaces;

namespace SearchSphere.API
{
    public class Function(IFileService fileService)
    {
        [Function(nameof(UploadDocument))]
        public async Task<HttpResponseData> UploadDocument(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "upload-document")]
            HttpRequestData req)
        {
            var response = req.CreateResponse();

            try
            {
                // Validate the request body
                if (!req.Body.CanRead)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    await response.WriteStringAsync("Invalid request body.");
                    return response;
                }

                // Parse multipart form-data
                var formData = await MultipartFormDataParser.ParseAsync(req.Body);
                var file = formData.Files.Count > 0 ? formData.Files[0] : null;

                if (file == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    await response.WriteStringAsync("No file provided in the request.");
                    return response;
                }

                if (file.Data.Length > 1000000) // 1 MB in bytes
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    await response.WriteStringAsync("Max file size is 1MB.");
                    return response;
                }

                // Return file details for further processing
                response.StatusCode = HttpStatusCode.OK;
                await response.WriteStringAsync($"File received: {file.FileName}, Size: {file.Data.Length} bytes");

                await fileService.UploadFile("user-reference-id-from-jwt", file);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                await response.WriteStringAsync($"Error processing file: {ex.Message}");
            }

            return response;
        }
    }
}
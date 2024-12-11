using FluentValidation;
using Newtonsoft.Json;
using SearchSphere.Common.Services.Interfaces;

namespace SearchSphere.Common.Services
{
    public class HttpRequestBodyMapper<T>(IValidator<T> validator) : IHttpRequestBodyMapper<T>
    {
        public async Task<T> Map(Stream requestBody)
        {
            if (requestBody == null) throw new ArgumentNullException(nameof(requestBody));

            using var streamReader = new StreamReader(requestBody);
            var deserializedObject = JsonConvert.DeserializeObject<T>(await streamReader.ReadToEndAsync());

            if (deserializedObject == null)
            {
                throw new InvalidOperationException("Deserialization returned null.");
            }

            return deserializedObject;
        }

        public async Task<T> MapAndValidate(Stream requestBody)
        {
            T obj = await Map(requestBody);
            await validator.ValidateAndThrowAsync(obj);
            return obj;
        }
    }
}
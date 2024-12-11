namespace SearchSphere.Common.Services.Interfaces
{
    public interface IHttpRequestBodyMapper<T>
    {
        Task<T> Map(Stream requestBody);
        Task<T> MapAndValidate(Stream requestBody);
    }
}
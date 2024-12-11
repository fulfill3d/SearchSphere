using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SearchSphere.Common.Services.Interfaces;

namespace SearchSphere.Common.Services
{
    public static class DepInj
    {
        public static void ConfigureServiceOptions<TOptions>(
            this IServiceCollection services,
            Action<IServiceProvider, TOptions> configure)
            where TOptions : class
        {
            services
                .AddOptions<TOptions>()
                .Configure<IServiceProvider>((options, resolver) => configure(resolver, options));
        }
        
        public static void AddHttpRequestBodyMapper(this IServiceCollection services)
        {
            services.AddTransient(typeof(IHttpRequestBodyMapper<>), typeof(HttpRequestBodyMapper<>));
        }
        
        public static void AddFluentValidator<T>(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(T).Assembly);
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Pangea.Shared.Attributes.Authorization.Contracts;
using Pangea.Shared.Attributes.Authorization.HttpClient;

namespace Pangea.Shared.Attributes.Authorization.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddPangeaAuthorizationClient<TUserInfo>(this IServiceCollection services, string adminServiceUrl) where TUserInfo : class, IUserInfo
        {
            services.AddScoped<IUserInfo, TUserInfo>();
            services.AddHttpContextAccessor();
            services.AddAdminServiceHttpClient(adminServiceUrl);
            return services;
        }

        public static IServiceCollection AddPangeaAuthorizationServer<TUserInfo>(this IServiceCollection services) where TUserInfo : class, IUserInfo
        {
            services.AddScoped<IUserInfo, TUserInfo>();
            return services;
        }

        private static IServiceCollection AddAdminServiceHttpClient(this IServiceCollection services, string adminServiceUrl)
        {
            services.AddHttpClient<AdminHttpClientService>(htppClient =>
            {
                htppClient.BaseAddress = new Uri(adminServiceUrl.Trim());
            })
                .ConfigurePrimaryHttpMessageHandler(x => new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    }
                });

            return services;
        }
    }
}

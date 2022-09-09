using Microsoft.Extensions.DependencyInjection;
using Pangea.Shared.Attributes.Authorization.Contracts;

namespace Pangea.Shared.Attributes.Authorization.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddPangeaAuthorization<TUserInfo>(this IServiceCollection services) where TUserInfo : class, IUserInfo
        {
            services.AddScoped<IUserInfo, TUserInfo>(); 
            return services;
        }
    }
}

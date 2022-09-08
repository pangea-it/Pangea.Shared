using Microsoft.Extensions.DependencyInjection;
using Pangea.Shared.Attributes.Authorization.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Shared.Attributes.Authorization.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddClaimStore<TClaimStore>(this IServiceCollection services) where TClaimStore : class, IClaimStore
        {
            services.AddScoped<IClaimStore, TClaimStore>(); 
            return services;
        }
    }
}

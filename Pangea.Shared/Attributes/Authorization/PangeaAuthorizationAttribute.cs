using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Pangea.Shared.Attributes.Authorization.Contracts;

namespace Pangea.Shared.Attributes.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Module | AttributeTargets.Assembly, AllowMultiple = false)]
    public class PangeaAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if(context.ActionDescriptor.EndpointMetadata.Any(x=>x is IAllowAnonymous))
            {
                return;
            }

            IUserInfo userInfo = (IUserInfo)context.HttpContext.RequestServices.GetRequiredService(typeof(IUserInfo));

            if (!userInfo.IsUserAuthenticated())
            {
                context.Result = new UnauthorizedResult();
                return; 
            }

            var controller = context.RouteData.Values["controller"].ToString();
            var action = context.RouteData.Values["action"].ToString();

            string claim = string.Join('/', controller, action);


            if (!userInfo.HasClaim(claim))
            {
                context.Result = new ForbidResult();
                return;
            }

            return;
        }
    }
}

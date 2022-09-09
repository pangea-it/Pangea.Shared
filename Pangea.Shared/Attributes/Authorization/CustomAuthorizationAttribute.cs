using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Pangea.Shared.Attributes.Authorization.Contracts;
using System.Security.Claims;

namespace Pangea.Shared.Attributes.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CustomAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
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

            string? securityStamp = context.HttpContext.User.FindFirst("SecurityStamp")?.Value;
            string? username = context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            string? userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (securityStamp == null || username == null || userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (username == "SuperAdmin")
            {
                return;
            }

            var controller = context.RouteData.Values["controller"].ToString();
            var action = context.RouteData.Values["action"].ToString();

            string claim = string.Join('/', context, action);

            IClaimStore claimStore = (IClaimStore)context.HttpContext.RequestServices.GetRequiredService(typeof(IClaimStore));
            var claimList = await claimStore.GetUserClaims(int.Parse(userId!), securityStamp!);
            
            if (!claimList.Any(x => x == claim))
            {
                context.Result = new ForbidResult();
            }

            return;
        }
    }
}

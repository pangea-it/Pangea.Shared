using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Pangea.Shared.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method| AttributeTargets.Assembly | AttributeTargets.Module, AllowMultiple = false)]
    public class ModelStateValidationAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors);

                var responseContent = errors.Select(x => x.ErrorMessage);

                context.Result = new BadRequestObjectResult(responseContent);
            }

            return base.OnActionExecutionAsync(context, next);
        }
    }
}

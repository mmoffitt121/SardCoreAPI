using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Utility.Validation
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null)
            {
                return;
            }

            List<object> failures = new List<object>();
            foreach (var arg in context.ActionArguments)
            {
                if (arg.Value is IValidatable)
                {
                    List<string> validationFailures = ((IValidatable)arg.Value).Validate();
                    if (validationFailures.Count() > 0)
                    {
                        failures.Add(validationFailures);
                    }
                }
            }

            if (failures.Count() > 0)
            {
                context.Result = new BadRequestObjectResult(failures);
            }
            base.OnActionExecuting(context);
        }
    }
}

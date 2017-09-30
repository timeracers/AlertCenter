using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections;
using AlertCenter.Exceptions;
using System.Linq;

namespace AlertCenter.Middleware
{
    public class Validator : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var values = context.ActionDescriptor.Parameters.Select(
                (p) => context.ActionArguments.ContainsKey(p.Name) ? context.ActionArguments[p.Name] : null).ToArray();
            if (!Validate(values))
                context.Result = new JsonFailure(new InvalidParametersException());
        }

        public bool Validate(object obj)
        {
            if (obj == null)
                return false;
            if (obj is IEnumerable)
                foreach (var v in obj as IEnumerable)
                    if (!Validate(v))
                        return false;
            return true;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
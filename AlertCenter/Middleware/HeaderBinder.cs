using AlertCenter.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace AlertCenter.Middleware
{
    public class HeaderBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.BindingInfo.BindingSource == BindingSource.Header)
                return new HeaderBinder();
            return null;
        }
    }

    public class HeaderBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var headers = bindingContext.HttpContext.Request.Headers;
            if (headers.ContainsKey(bindingContext.BinderModelName))
                bindingContext.Result = ModelBindingResult.Success(headers[bindingContext.BinderModelName].ToString());
            else if (bindingContext.BinderModelName == "Authorization")
                throw new UnauthorizedException();
            else
                throw new InvalidParametersException(bindingContext.BinderModelName + " is required.");
            return Task.CompletedTask;
        }
    }
}

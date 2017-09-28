using AlertCenter.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AlertCenter.Middleware
{
    public class BodyBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.BindingInfo.BindingSource == BindingSource.Body)
                return new BodyBinder();
            return null;
        }
    }

    public class BodyBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.HttpContext.Request.ContentType == null || bindingContext.HttpContext.Request.ContentType == "application/json")
                try
                {
                    var stream = new MemoryStream();
                    bindingContext.HttpContext.Request.Body.CopyTo(stream);
                    var text = Encoding.UTF8.GetString(stream.ToArray());
                    bindingContext.Result = ModelBindingResult.Success(JsonConvert.DeserializeObject(text, bindingContext.ModelType));
                    return Task.CompletedTask;
                }
                catch
                {
                    throw new InvalidParametersException(ErrorMessages.InvalidJsonOrIncorrectType);
                }
            else
            {
                throw new InvalidParametersException("All requests and response bodies should be application/json", 415);
            }
            
        }
    }
}

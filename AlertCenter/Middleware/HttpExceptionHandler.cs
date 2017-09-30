using AlertCenter.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace AlertCenter.Middleware
{
    public class HttpExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is HttpException exception)
            {
                context.HttpContext.Response.StatusCode = 200;
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new JsonFailure(exception).Obj));
                context.HttpContext.Response.Body.Write(bytes, 0, bytes.Count());
                context.ExceptionHandled = true;
            }
            else
            {
                context.HttpContext.Response.StatusCode = 500;
                var bytes = Encoding.UTF8.GetBytes("Internal Server Error.");
                context.HttpContext.Response.Body.Write(bytes, 0, bytes.Count());
                context.ExceptionHandled = true;
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using AlertCenter.Exceptions;

namespace AlertCenter.Middleware
{
    public abstract class JsonContent : IActionResult
    {
        public object Obj { get; }

        public JsonContent(object obj)
        {
            Obj = obj;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = 200;
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Obj));
            context.HttpContext.Response.Body.Write(bytes, 0, bytes.Length);
            return Task.CompletedTask;
        }
    }

    public class JsonSuccess : JsonContent
    {
        public JsonSuccess()
            : base(new ResponseWrapper(204) { Response = JsonConvert.SerializeObject(null) }) { }

        public JsonSuccess(object obj)
            : base(new ResponseWrapper(200) { Response = JsonConvert.SerializeObject(obj) }) { }
    }

    public class JsonFailure : JsonContent
    {
        public JsonFailure(HttpException exception)
            : base(new ResponseWrapper(exception.StatusCode) { ExceptionType = exception.GetType().Name, ExceptionMessage = exception.Message }) { }
    }
}
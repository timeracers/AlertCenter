using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using AlertCenter.Exceptions;

namespace AlertCenter.Controllers
{
    public abstract class JsonStatusCode : IActionResult
    {
        public int HttpStatus { get; }
        public string Obj { get; }

        public JsonStatusCode(int statusCode, string obj)
        {
            HttpStatus = statusCode;
            Obj = obj;
        }

        public T DeserializeObj<T>()
        {
            return JsonConvert.DeserializeObject<T>(Obj);
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = HttpStatus;
            if(Obj != JsonConvert.SerializeObject(null))
            {
                var bytes = Encoding.UTF8.GetBytes(Obj);
                context.HttpContext.Response.Body.Write(bytes, 0, bytes.Length);
            }
            return Task.CompletedTask;
        }
    }

    public class JsonResult : JsonStatusCode
    {
        public JsonResult(int httpStatus, object obj)
            : base(httpStatus, JsonConvert.SerializeObject(obj)) { }
    }

    public class JsonContent : JsonStatusCode
    {
        public JsonContent(object obj)
            : base(200, JsonConvert.SerializeObject(obj)) { }
    }

    public class JsonNoContent : JsonStatusCode
    {

        public JsonNoContent()
            : base(204, JsonConvert.SerializeObject(null)) { }
    }

    public class JsonHttpException : JsonStatusCode
    {
        public JsonHttpException(HttpException exception)
            : base(exception.StatusCode, new JObject()
                { ["StatusCode"] = exception.StatusCode, ["Type"] = exception.GetType().Name, ["Message"] = exception.Message }.ToString()) { }
    }
}
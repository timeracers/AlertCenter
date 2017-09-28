using Microsoft.AspNetCore.Mvc;
using AlertCenter.Exceptions;

namespace AlertCenter.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        [HttpGet]
        public JsonStatusCode Get()
        {
            return new JsonContent("Insert Api Calls Here, maybe I will change to using swagger though");
        }

        [Route("{*url}", Order = 999)]
        public JsonStatusCode DefaultRoute()
        {
            return new JsonHttpException(new DoesNotExistException("Page not found. To view all available api calls visit \"/api\"."));
        }
    }
}

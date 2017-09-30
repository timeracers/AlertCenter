using Microsoft.AspNetCore.Mvc;
using AlertCenter.Exceptions;
using AlertCenter.Middleware;

namespace AlertCenter
{
    public class ApiController : Controller
    {
        //[Route("{*url}", Order = 999)]
        public JsonContent DefaultRoute()
        {
            return new JsonFailure(new DoesNotExistException("Page not found. To view all available api calls visit \"/api\"."));
        }
    }
}

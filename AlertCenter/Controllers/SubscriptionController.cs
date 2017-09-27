using AlertCenter.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AlertCenter.Controllers
{
    [Route("api/[controller]")]
    public class SubscriptionController : Controller
    {
        [HttpGet]
        public JsonStatusCode Get()
        {
            return new JsonContent(new string[] { "Welcome to alert center", "System is down" });
        }
        
        [HttpPut("{topic}")]
        public JsonStatusCode Put([FromHeader(Name = "Authentication")]string jwt, string topic)
        {
            return new JsonNoContent();
        }
        
        [HttpDelete("{topic}")]
        public JsonStatusCode Delete([FromHeader(Name = "Authentication")]string jwt, string topic)
        {
            return new JsonNoContent();
        }

        [HttpDelete]
        public JsonStatusCode Delete([FromHeader(Name = "Authentication")]string jwt)
        {
            return new JsonNoContent();
        }
    }
}

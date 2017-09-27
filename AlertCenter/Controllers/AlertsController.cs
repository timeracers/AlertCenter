using Microsoft.AspNetCore.Mvc;
using AlertCenter.Dtos;

namespace AlertCenter.Controllers
{
    [Route("api/[controller]")]
    public class AlertsController : Controller
    {
        [HttpGet]
        public JsonStatusCode Get()
        {
            return new JsonContent(new Topic[] { new Topic("Welcome to the alert center"), new Topic("System is down") });
        }
        
        [HttpGet("{topic}")]
        public JsonStatusCode Get(string topic)
        {
            return new JsonContent(new Alert(topic, "I don't know anything about this topic but I will say something anyway", 2000000000, "some noob"));
        }
        
        [HttpPost("{topic}")]
        public JsonStatusCode Post([FromHeader(Name = "Authentication")]string jwt, string topic, [FromBody]string value)
        {
            return new JsonNoContent();
        }
        
        [HttpPut("{topic}")]
        public JsonStatusCode Put([FromHeader(Name = "Authentication")]string jwt, string topic)
        {
            return new JsonNoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace AlertCenter.Controllers
{
    [Route("api/[controller]")]
    public class EmailController : Controller
    {
        [HttpGet]
        public JsonStatusCode Get([FromHeader(Name = "Authentication")]string jwt)
        {
            return new JsonContent("johnDoe@foo.bar");
        }
        
        [HttpPut]
        public JsonStatusCode Put([FromHeader(Name = "Authentication")]string jwt, [FromBody]string email)
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

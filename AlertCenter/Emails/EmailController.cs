using AlertCenter.Emails;
using AlertCenter.Middleware;
using AlertCenter.Security;
using Microsoft.AspNetCore.Mvc;

namespace AlertCenter.Controllers.Emails
{
    [Route("api/[controller]")]
    public class EmailController : Controller
    {
        private AuthenticationWrapper _auth;
        private EmailGateway _emails;

        public EmailController(AuthenticationWrapper auth, EmailGateway emails)
        {
            _auth = auth;
            _emails = emails;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWrapper<string>), 200)]
        public JsonContent Get([FromHeader(Name = "Authorization")]string jwt)
        {
            return _auth.Authenticated(jwt, (userId, _) => _emails.Get(userId));
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(ResponseWrapper<None>), 200)]
        public JsonContent Put([FromHeader(Name = "Authorization")]string jwt, [FromBody]string email)
        {
            return _auth.Authenticated(jwt, (userId, username) => _emails.Set(userId, email, username));
        }
        
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseWrapper<None>), 200)]
        public JsonContent Delete([FromHeader(Name = "Authorization")]string jwt)
        {
            return _auth.Authenticated(jwt, (userId, _) => _emails.Delete(userId));
        }
    }
}

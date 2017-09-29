﻿using Microsoft.AspNetCore.Mvc;

namespace AlertCenter.Controllers
{
    [Route("api/[controller]")]
    public class EmailController : Controller
    {
        private AuthenticationWrapper _auth;
        private UnnamedClass1 _emails;

        public EmailController(AuthenticationWrapper auth, UnnamedClass1 emails)
        {
            _auth = auth;
            _emails = emails;
        }

        [HttpGet]
        public JsonStatusCode Get([FromHeader(Name = "Authorization")]string jwt)
        {
            return _auth.Authenticated(jwt, (userId, _) => _emails.Get(userId));
        }
        
        [HttpPut]
        public JsonStatusCode Put([FromHeader(Name = "Authorization")]string jwt, [FromBody]string email)
        {
            return _auth.Authenticated(jwt, (userId, username) => _emails.Set(userId, email, username));
        }
        
        [HttpDelete]
        public JsonStatusCode Delete([FromHeader(Name = "Authorization")]string jwt)
        {
            return _auth.Authenticated(jwt, (userId, _) => _emails.Delete(userId));
        }
    }
}

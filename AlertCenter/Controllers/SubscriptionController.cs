using AlertCenter.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AlertCenter.Controllers
{
    [Route("api/[controller]")]
    public class SubscriptionController : Controller
    {
        private AuthenticationWrapper _auth;
        private UnnamedClass2 _subscriptions;

        public SubscriptionController(AuthenticationWrapper auth, UnnamedClass2 subscriptions)
        {
            _auth = auth;
            _subscriptions = subscriptions;
        }

        [HttpGet]
        public JsonStatusCode Get([FromHeader(Name = "Authorization")]string jwt)
        {
            return _auth.Authenticated(jwt, _subscriptions.GetAll);
        }
        
        [HttpPut("{topic}")]
        public JsonStatusCode Put([FromHeader(Name = "Authorization")]string jwt, string topic)
        {
            return _auth.Authenticated(jwt, userId => _subscriptions.Add(userId, topic));
        }
        
        [HttpDelete("{topic}")]
        public JsonStatusCode Delete([FromHeader(Name = "Authorization")]string jwt, string topic)
        {
            return _auth.Authenticated(jwt, userId => _subscriptions.Remove(userId, topic));
        }

        [HttpDelete]
        public JsonStatusCode Delete([FromHeader(Name = "Authorization")]string jwt)
        {
            return _auth.Authenticated(jwt, _subscriptions.RemoveAll);
        }
    }
}

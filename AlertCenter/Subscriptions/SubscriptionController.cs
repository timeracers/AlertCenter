using AlertCenter.Middleware;
using AlertCenter.Security;
using Microsoft.AspNetCore.Mvc;

namespace AlertCenter.Subscriptions
{
    [Route("api/[controller]")]
    public class SubscriptionController : Controller
    {
        private AuthenticationWrapper _auth;
        private SubscriptionGateway _subscriptions;

        public SubscriptionController(AuthenticationWrapper auth, SubscriptionGateway subscriptions)
        {
            _auth = auth;
            _subscriptions = subscriptions;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWrapper<string[]>), 200)]
        public JsonContent Get([FromHeader(Name = "Authorization")]string jwt)
        {
            return _auth.Authenticated(jwt, (userId, _) => _subscriptions.GetAll(userId));
        }
        
        [HttpPut("{topic}")]
        [ProducesResponseType(typeof(ResponseWrapper<None>), 200)]
        public JsonContent Put([FromHeader(Name = "Authorization")]string jwt, string topic)
        {
            return _auth.Authenticated(jwt, (userId, _) => _subscriptions.Add(userId, topic));
        }
        
        [HttpDelete("{topic}")]
        [ProducesResponseType(typeof(ResponseWrapper<None>), 200)]
        public JsonContent Delete([FromHeader(Name = "Authorization")]string jwt, string topic)
        {
            return _auth.Authenticated(jwt, (userId, _) => _subscriptions.Remove(userId, topic));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ResponseWrapper<None>), 200)]
        public JsonContent Delete([FromHeader(Name = "Authorization")]string jwt)
        {
            return _auth.Authenticated(jwt, (userId, _) => _subscriptions.RemoveAll(userId));
        }
    }
}

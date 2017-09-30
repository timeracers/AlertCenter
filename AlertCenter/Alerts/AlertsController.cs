using Microsoft.AspNetCore.Mvc;
using AlertCenter.Security;
using AlertCenter.Middleware;

namespace AlertCenter.Alerts
{
    [Route("api/[controller]")]
    public class AlertController : Controller
    {
        private AuthenticationWrapper _auth;
        private AlertGateway _alerts;

        public AlertController(AuthenticationWrapper auth, AlertGateway alerts)
        {
            _auth = auth;
            _alerts = alerts;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWrapper<Topic[]>), 200)]
        public JsonContent Get()
        {
            return _alerts.GetTopics();
        }
        
        [HttpGet("{topic}")]
        [ProducesResponseType(typeof(ResponseWrapper<Alert[]>), 200)]
        public JsonContent Get(string topic)
        {
            return _alerts.GetAllFrom(topic);
        }
        
        [HttpPost("{topic}")]
        [ProducesResponseType(typeof(ResponseWrapper<None>), 200)]
        public JsonContent Post([FromHeader(Name = "Authorization")]string jwt, string topic, [FromBody]string message)
        {
            return _auth.Authenticated(jwt, (userId, username) => _alerts.AddTo(topic, userId, username, message));
        }
        
        [HttpPut("{topic}")]
        [ProducesResponseType(typeof(ResponseWrapper<None>), 200)]
        public JsonContent Put([FromHeader(Name = "Authorization")]string jwt, string topic)
        {
            return _auth.Authenticated(jwt, (_, __) => _alerts.AddTopic(topic));
        }
    }
}

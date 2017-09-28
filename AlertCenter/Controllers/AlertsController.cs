using Microsoft.AspNetCore.Mvc;
using AlertCenter.Dtos;

namespace AlertCenter.Controllers
{
    [Route("api/[controller]")]
    public class AlertController : Controller
    {
        private AuthenticationWrapper _auth;
        private UnnamedClass3 _alerts;

        public AlertController(AuthenticationWrapper auth, UnnamedClass3 alerts)
        {
            _auth = auth;
            _alerts = alerts;
        }

        [HttpGet]
        public JsonStatusCode Get()
        {
            return _alerts.GetTopics();
        }
        
        [HttpGet("{topic}")]
        public JsonStatusCode Get(string topic)
        {
            return _alerts.GetAllFrom(topic);
        }
        
        [HttpPost("{topic}")]
        public JsonStatusCode Post([FromHeader(Name = "Authorization")]string jwt, string topic, [FromBody]string message)
        {
            return _auth.Authenticated(jwt, userId => _alerts.AddTo(topic, userId, message));
        }
        
        [HttpPut("{topic}")]
        public JsonStatusCode Put([FromHeader(Name = "Authorization")]string jwt, string topic)
        {
            return _auth.Authenticated(jwt, _ => _alerts.AddTopic(topic));
        }
    }
}

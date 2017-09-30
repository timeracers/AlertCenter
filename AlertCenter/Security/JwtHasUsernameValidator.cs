using JwtAuthenticator;
using Newtonsoft.Json.Linq;

namespace AlertCenter.Security
{
    public class JwtHasUsernameValidator : IJwtClaimValidator
    {
        public bool Validate(JwtPayload payload)
        {
            return payload.Validate<string>(JTokenType.String, "username", (s) => true);
        }
    }
}
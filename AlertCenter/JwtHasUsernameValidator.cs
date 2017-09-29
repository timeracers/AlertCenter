using JwtAuthenticator;
using Newtonsoft.Json.Linq;

namespace AlertCenter
{
    public class JwtHasUsernameValidator : IJwtClaimValidator
    {
        public JwtHasUsernameValidator()
        {
        }

        public bool Validate(JwtPayload payload)
        {
            return payload.Validate<string>(JTokenType.String, "username", (s) => true);
        }
    }
}
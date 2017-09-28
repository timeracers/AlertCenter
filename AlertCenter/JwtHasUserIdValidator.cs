using JwtAuthenticator;
using Newtonsoft.Json.Linq;

namespace AlertCenter
{
    public class JwtHasUserIdValidator : IJwtClaimValidator
    {
        public JwtHasUserIdValidator()
        {
        }

        public bool Validate(JwtPayload payload)
        {
            return payload.Validate<string>(JTokenType.String, "userId", (s) => true);
        }
    }
}
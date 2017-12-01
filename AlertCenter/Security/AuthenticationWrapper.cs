using System;
using JwtAuthenticator;
using AlertCenter.Exceptions;
using AlertCenter.Middleware;

namespace AlertCenter.Security
{
    public class AuthenticationWrapper
    {
        private Authenticator _authenticator;

        public AuthenticationWrapper(IEncryptor encryptor)
        {
            _authenticator = new Authenticator(encryptor, new JwtSubjectValidator(), new JwtHasUsernameValidator());
        }

        public JsonContent Authenticated(string jwt, Func<Guid, string, JsonContent> authenticatedAction)
        {
            var authenticated = _authenticator.Authenticate(jwt);
            if (authenticated.Item1 == Token.Verified)
            {
                Guid sub = Guid.Empty;
                try
                {
                    sub = authenticated.Item2["sub"].ToObject<Guid>();
                }
                catch
                {
                    return new JsonFailure(new UnauthorizedException());
                }
                return authenticatedAction(sub, authenticated.Item2["username"].ToObject<string>());
            }
            else if (authenticated.Item1 == Token.BadClaims && !new JwtExpiresValidator().Validate(authenticated.Item2))
                return new JsonFailure(new UnauthorizedException(ExceptionMessages.Expired));
            else
                return new JsonFailure(new UnauthorizedException());
        }
    }
}

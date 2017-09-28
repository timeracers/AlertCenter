using System;
using JwtAuthenticator;
using AlertCenter.Controllers;
using AlertCenter.Exceptions;

namespace AlertCenter
{
    public class AuthenticationWrapper
    {
        private Authenticator _authenticator;

        public AuthenticationWrapper(IEncryptor encryptor)
        {
            _authenticator = new Authenticator(encryptor, new JwtHasUserIdValidator());
        }

        public JsonStatusCode Authenticated(string jwt, Func<string, JsonStatusCode> authenticatedAction)
        {
            var authenticated = _authenticator.Authenticate(jwt);
            if (authenticated.Item1 == JwtAuthenticator.Token.Verified)
                return authenticatedAction(authenticated.Item2["userId"].ToObject<string>());
            else if (authenticated.Item1 == JwtAuthenticator.Token.BadClaims && !new JwtExpiresValidator().Validate(authenticated.Item2))
                return new JsonHttpException(new UnauthorizedException(ErrorMessages.Expired));
            else
                return new JsonHttpException(new UnauthorizedException());
        }
    }
}

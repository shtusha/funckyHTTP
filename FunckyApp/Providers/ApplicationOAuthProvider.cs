using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FunckyApp.DataAccess;
using FunckyApp.DataAccess.User;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace FunckyApp.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserRepository _userRepository = RepositoryFactory.GetRepository<IUserRepository>();

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var response = await _userRepository.GetAsync(context.UserName);

            var user = response.Entity;

            if (user == null || user.PasswordHash != context.Password.HashPassword(user.HashSalt))
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var claims = new[]{ new Claim(ClaimTypes.NameIdentifier, user.UserName)};
            var oAuthIdentity = new ClaimsIdentity(claims, context.Options.AuthenticationType);
            var cookiesIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);

            var properties = CreateProperties(user.UserName);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        } 
        
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key.Replace(".","_"), property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}
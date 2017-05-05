using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CloneDeploy_App;
using CloneDeploy_Services;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace CloneDeploy_App
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var auth = new AuthenticationServices();

            var validationResult = auth.GlobalLogin(context.UserName, context.Password, "Web");
            if (validationResult.Success)
            {
                ClaimsIdentity oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                context.Validated(oAuthIdentity);
                var user = new UserServices().GetUser(context.UserName);
                oAuthIdentity.AddClaim(new Claim("user_id", user.Id.ToString()));
                //set different time spans here
                if(user.Membership == "Service Account")
                context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(5);
                context.Validated(oAuthIdentity);
            }
            else
            {
                context.SetError("invalid_grant", validationResult.ErrorMessage);

            }
            return Task.FromResult<object>(null);
        }
    }
    public class Startup
    {

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


        }
    }
}
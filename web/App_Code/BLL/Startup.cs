using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using OAuth20;
using Owin;
using Security;

[assembly: OwinStartup(typeof(Startup))]

namespace OAuth20
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var auth = new Authenticate();

            var validationResult = auth.GlobalLogin(context.UserName, context.Password, "Web");
            if ((validationResult.Success))
            {
                ClaimsIdentity oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                context.Validated(oAuthIdentity);

                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                //oAuthIdentity.AddClaim(new Claim("sub", context.UserName));
                //oAuthIdentity.AddClaim(new Claim("role", "Admin"));
                oAuthIdentity.AddClaim(new Claim("user_id", BLL.User.GetUser(context.UserName).Id.ToString()));
                context.Validated(oAuthIdentity);
            }
            else
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
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
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CloneDeploy_App;
using CloneDeploy_Common;
using CloneDeploy_Services;
using CloneDeploy_Services.Workflows;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof (Startup))]

namespace CloneDeploy_App
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});
            var auth = new AuthenticationServices();

            var validationResult = auth.GlobalLogin(context.UserName, context.Password, "Web");
            if (validationResult.Success)
            {
                var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                context.Validated(oAuthIdentity);
                var user = new UserServices().GetUser(context.UserName);
                oAuthIdentity.AddClaim(new Claim("user_id", user.Id.ToString()));
                //set different time spans here
                if (user.Membership == "Service Account")
                    context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60);
                context.Validated(oAuthIdentity);
            }
            else
            {
                context.SetError("invalid_grant", validationResult.ErrorMessage);
            }
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }
    }

    public class Startup
    {
        
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            // Hangfire initialization
            
            if (SettingServices.GetSettingValue(SettingStrings.OperationMode) == "Cluster Primary")
            {
                var stringInterval = SettingServices.GetSettingValue(SettingStrings.SecondaryServerMonitorInterval);
                int interval = 0;
                var result = Int32.TryParse(stringInterval, out interval);

                if (result && interval != 0)
                {
                    GlobalConfiguration.Configuration.UseMemoryStorage();
                    app.UseHangfireDashboard();
                    app.UseHangfireServer();

                    RecurringJob.AddOrUpdate(() => new SecondaryServerMonitor().Execute(), Cron.MinuteInterval(interval));
                }
            }
        }
    }
}
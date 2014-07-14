using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Owin;

namespace Interext
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");
            var x = new FacebookAuthenticationOptions();
            x.Scope.Add("email");
            x.AppId = "650693548342259";
            x.AppSecret = "f63ea2f17a0874493962be19e5344834";
            x.Provider = new FacebookAuthenticationProvider()
            {
                OnAuthenticated = async context =>
                {
                    context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                    foreach (var claim in context.User)
                    {
                        var claimType = string.Format("urn:facebook:{0}", claim.Key);
                        string claimValue = claim.Value.ToString();
                        if (!context.Identity.HasClaim(claimType, claimValue))
                            context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, "XmlSchemaString", "Facebook"));
                    }

                }
            };

            x.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;
            app.UseFacebookAuthentication(x);
            //app.UseFacebookAuthentication(
            //   appId: "650693548342259",
            //   appSecret: "f63ea2f17a0874493962be19e5344834");

            app.UseGoogleAuthentication();
        }
    }
}
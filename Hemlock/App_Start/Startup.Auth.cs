using Hemlock.Models;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

namespace Hemlock.App_Start
{
    public partial class Startup
    {
        // NOTE: The Google API secret and client ID were sanitized for this demo.
        private readonly string _clientId = "SANITIZED_ID";

        private readonly string _clientSecret = "SANITIZED_SECRET";

        private readonly string _loginPath = "/MyActivity";

        private void ConfigureAuth(IAppBuilder app)
        {
            var cookieOptions = new CookieAuthenticationOptions
            {
                LoginPath = new PathString(_loginPath)
            };

            app.UseCookieAuthentication(cookieOptions);

            app.SetDefaultSignInAsAuthenticationType(cookieOptions.AuthenticationType);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = _clientId,
                ClientSecret = _clientSecret,
            });
        }
    }
}
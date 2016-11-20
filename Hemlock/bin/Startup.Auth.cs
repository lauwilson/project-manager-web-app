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
        private readonly string _clientId = "298831950262-ctrh8t0jgg1dkgqj52tom1l5umd8797g" +
            ".apps.googleusercontent.com";

        private readonly string _clientSecret = "e04HhcRiNbmSGSblYEHM0-oe";

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
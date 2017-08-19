using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Principal;

namespace NetUserMgtMvc.Shared
{
    public class Authentication
    {
		private const string AUTHENTICATION_SCHEME = "CookieMiddlewareInstance";
		
        private static Authentication instance;

        public static Authentication SharedInstance { 
            get {
                if (instance == null) {
                    instance = new Authentication();
                }

                return instance;
            }
        }

        private Authentication() {}

        public void Configure(IApplicationBuilder app) {
			app.UseCookieAuthentication(new CookieAuthenticationOptions()
			{
				AuthenticationScheme = AUTHENTICATION_SCHEME,
				LoginPath = new PathString("/User/Login/"),
				AccessDeniedPath = new PathString("/User/Forbidden/"),
				AutomaticAuthenticate = true,
				AutomaticChallenge = true
			});
        }

        public async void SignIn (HttpContext context, string username) {
			ClaimsIdentity objClaim = new ClaimsIdentity("Bearer");
            objClaim.AddClaim(new Claim(ClaimTypes.Name, username));

            ClaimsPrincipal principal = new ClaimsPrincipal(objClaim);

            await context.Authentication.SignInAsync(AUTHENTICATION_SCHEME, principal);
        }

        public async void SignOut (HttpContext context) {
            await context.Authentication.SignOutAsync(AUTHENTICATION_SCHEME);
        }
    }
}

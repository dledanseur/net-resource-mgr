using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace NetUserMgtMvc.Shared
{
    public class Authentication
    {
		private const string AUTHENTICATION_SCHEME = "CookieMiddlewareInstance";
		
        private static Authentication instance;

        private string AccessToken { get; set; }

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
            app.UseAuthentication();
			
        }

        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(o =>
            {
                o.Authority = configuration["auth:oidc:authority"];
                o.ClientId = configuration["auth:oidc:clientid"];
                o.ClientSecret = configuration["auth:oidc:clientsecret"];
                o.ResponseType = "code";
                o.Scope.Clear();
                o.Scope.Add("openid");
               
                    
                /*o.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents()
                {
                    OnAuthorizationCodeReceived = context => {
                        context.Token = context.Request.Query["access_token"];
                    }
                }*/

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

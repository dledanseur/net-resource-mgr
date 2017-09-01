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
using Services.Services.UserService;
using System.Threading.Tasks;

namespace NetUserMgtMvc.Shared
{
    public class Authentication
    {
		private const string AUTHENTICATION_SCHEME = "CookieMiddlewareInstance";
		
        private static Authentication instance;

        public static Authentication SharedInstance
        {
            get
            {
                return instance;
            }
        }    

        private IUserService _user_service;

        private string AccessToken { get; set; }


        

        public Authentication(IUserService userService)
        {
            if (instance != null)
            {
                throw new Exception("Authentication object already constructed");
            }

            instance = this;
            this._user_service = userService;

        }

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
                o.GetClaimsFromUserInfoEndpoint = true;

                o.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents()
                {

                    OnUserInformationReceived = async context => 
                    {
                        foreach (Claim c in context.Principal.Claims) {
                            await CreateOrUpdateProfile(c);
                        }

                        return;
                    }
                    /*,

                    OnAuthorizationCodeReceived = context =>
                    {
                        //context.Token = context.Request.Query["access_token"];
                    }*/


                };

                

            });
            
        }

        public static async Task CreateOrUpdateProfile(Claim c)
        {
            UserProfile profile = new UserProfile();
            profile.ExternalId = c.Subject.Name;
            profile.Email = c.Properties["email"];
            profile.UserName = c.Properties["nickname"];
            profile.FullName = c.Properties["name"];

            await Authentication.SharedInstance._user_service.CreateOrUpdateUserProfile(profile);

        }

    }
}

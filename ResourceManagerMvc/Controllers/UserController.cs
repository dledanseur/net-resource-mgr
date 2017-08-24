using System;
using Microsoft.AspNetCore.Mvc;
using Services.Authentication;
using Tools.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace NetUserMgtMvc.Controllers
{
    public class UserController : Controller
    {

		public const string APPLICATION_BASE_URL="BaseUrl";

        private IAuthenticationService _authenticationService;
        private IConfig _config;

        public UserController(IAuthenticationService authenticationService, IConfig config )
        {
            this._authenticationService = authenticationService;
            this._config = config;
        }

        public IActionResult Login() {

            string baseurl = _config.GetStringParam(APPLICATION_BASE_URL);

            string finalRedirectUrl = baseurl + "/user/verify";

            AuthenticationSession res = this._authenticationService.InitiateSession(finalRedirectUrl);

            CookieOptions options = new CookieOptions();
            options.HttpOnly = true;
            options.Secure = true;

            HttpContext.Response.Cookies.Append("x-state",res.State, options);

            return new RedirectResult(res.LoginURI);

        }

        [Authorize]
        public IActionResult Authenticate() {
            /*string userName = this._authenticationService.VerifyLogin();*/


            return new RedirectToActionResult("Index", "Home",null);
           

        }

    }
}

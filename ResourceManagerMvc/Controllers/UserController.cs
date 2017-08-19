using System;
using Microsoft.AspNetCore.Mvc;
using UserServices.Services;
using Tools.Configuration;
using Microsoft.AspNetCore.Http;

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

        public IActionResult Login(HttpResponse response) {

            string baseurl = _config.GetStringParam(APPLICATION_BASE_URL);

            string finalRedirectUrl = baseurl + "/user/verify";

            AuthenticationSession res = this._authenticationService.InitiateSession(finalRedirectUrl);

            return new RedirectResult(res.LoginURI);

        }

        public IActionResult Authenticate() {
            string userName = this._authenticationService.VerifyLogin();


            return RedirectToAction("Index", "Home");

        }

    }
}

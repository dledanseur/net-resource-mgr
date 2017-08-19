using System;
namespace UserServices.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Initiate an authentication session. This method must perform
        /// any initialization required by the authentication provider
        /// and return an authentication sesssion, that will allow the
        /// UI to redirect to the login service
        /// </summary>
        /// <param name="redirectURI">The redirection url that must be setup when preparing authentication</param>
        /// <returns>The authentication session.</returns>
        AuthenticationSession InitiateSession(string redirectURI);

        string VerifyLogin();
    }
}

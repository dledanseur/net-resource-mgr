using System;
using Tools;
namespace Services.Authentication
{
    /// <summary>
    /// This class contains information used to perform an authentication.
    /// 
    ///nThe RedirectURI property must be used by the frontend as a redirection
    /// so that the user can log into the system.
    /// 
    /// </summary>
    public class AuthenticationSession
    {
        public AuthenticationSession() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Services.Authentication.AuthenticationSession"/> class.
        ///
        /// </summary>
        /// <param name="login_uri">URI to which redirect the user so that he can authenticate</param>
        /// <param name="state">Random string used as a CSRF mechanism, to be stored in a secured cookie for instance</param>
        public AuthenticationSession(string login_uri, string state) {
            this.LoginURI = login_uri;
            this.State = state;
        }

        /// <summary>
        /// Gets or sets the redirect URI to which the user is redirected to 
        /// perform the authentication.
        /// </summary>
        /// <value>The redirect URI.</value>
        public string LoginURI { get; set; }

        /// <summary>
        /// A random token, that must be provided back when calling the verify
        /// authentication method of the authentication provider. The web application
        /// has to store that token, for instance via a secure cookie
        /// </summary>
        /// <value>The state.</value>
        public string State { get; set; }

        public override bool Equals(object obj)
        {
            return new EqualsBuilder<AuthenticationSession>(this, obj)
                .With(o => o.LoginURI)
                .With(o => o.State)
                .Equals();
        }

        public override int GetHashCode()
        {
			return (LoginURI == null ? 0 : LoginURI.GetHashCode()) ^
                    (State == null ? 0 : State.GetHashCode());
        }
    }
}

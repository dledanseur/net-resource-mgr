using System;
using Tools.Configuration;
using Tools.HttpHandler;
using Tools.Random;
using Tools.Random.Impl;
using System.Web;
                
namespace Services.Authentication.Gitlab
{
    public class GitlabAuthenticationService: IAuthenticationService
    {

        public const string GITLAB_URL_CONFIG_KEY = "Gitlab:Url";
        public const string GITLAB_CLIENT_ID_CONFIG_KEY = "Gitlab:ClientId";

        private IConfig _config;
        private IRestClient _restClient;
        private IRandomObjects _randomOjects;

        public GitlabAuthenticationService(IConfig c, IRestClient client) : this(c, client, new RandomObjects()) {}

        public GitlabAuthenticationService(IConfig c, IRestClient client, IRandomObjects randomObjects)
        {
            this._config = c;
            this._restClient = client;
            this._randomOjects = randomObjects;
        }

        public AuthenticationSession InitiateSession(string redirectURI)
        {

            string url = _config.GetStringParam(GITLAB_URL_CONFIG_KEY);
            string clientId = _config.GetStringParam(GITLAB_CLIENT_ID_CONFIG_KEY);
            string state = _randomOjects.RandomHexString(8);

            string complete_url = url + "/oauth/authorize" +
                "?client_id=" + clientId +
                "&redirect_uri=" + HttpUtility.UrlEncode(redirectURI) +
                "&response_type=code" +
                "&state=" + state;

            return new AuthenticationSession(complete_url, state);
        }

        public string VerifyLogin()
        {
            throw new NotImplementedException();
        }
    }
}

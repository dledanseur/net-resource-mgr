using Xunit;
using Moq;
using Tools.Configuration;
using Tools.HttpHandler;
using Tools.Random;

namespace UserServices.Services.Gitlab
{
    public class GitlabAuthenticationServiceTest
    {
        private GitlabAuthenticationService gitlab_client;
        private Mock<IConfig> config_mock;
        private Mock<IRestClient> rest_client_mock;
        private Mock<IRandomObjects> _random_objects_mock;


        public GitlabAuthenticationServiceTest() 
        {
            this.config_mock = new Mock<IConfig>();

            this._random_objects_mock = new Mock<IRandomObjects>();

            this.rest_client_mock = new Mock<IRestClient>();

            this.gitlab_client = new GitlabAuthenticationService(
                this.config_mock.Object, 
                this.rest_client_mock.Object,
                this._random_objects_mock.Object
            );

            config_mock.Setup(c => c.GetStringParam(GitlabAuthenticationService.GITLAB_URL_CONFIG_KEY))
                       .Returns("http://gitlab.example.com");

		}

        [Fact]
        public void TestInitiateSession()
        {
			//given
			config_mock.Setup(c => c.GetStringParam(GitlabAuthenticationService.GITLAB_CLIENT_ID_CONFIG_KEY))
					   .Returns("1234");
            
            _random_objects_mock.Setup(r => r.RandomHexString(It.IsAny<int>())).Returns("5678");

            // when
            AuthenticationSession authSession = gitlab_client.InitiateSession("http://redirect.to");

            // then
            AuthenticationSession expectedSession = new AuthenticationSession(
                "http://gitlab.example.com/oauth/authorize" +
	                "?client_id=1234" +
	                "&redirect_uri=http%3A%2F%2Fredirect.to" +
	                "&response_type=code" +
	                "&state=5678",
                "5678"
            );

            Assert.Equal(expectedSession, authSession);

            _random_objects_mock.Verify(r => r.RandomHexString(8));
        }
    }
}

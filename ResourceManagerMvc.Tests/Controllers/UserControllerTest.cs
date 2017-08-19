using System;
using Xunit;
using NetUserMgtMvc.Controllers;
using Moq;
using Services.Authentication;
using Tools.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace NetUserMgtMvc.Tests
{
    public class UserControllerTest
    {
        private UserController _userController;
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private Mock<IConfig> _configMock;

        public UserControllerTest() {

            this._authenticationServiceMock = new Mock<IAuthenticationService>();
            this._configMock = new Mock<IConfig>();
            this._userController = new UserController(_authenticationServiceMock.Object, _configMock.Object);

        }

        [Fact]
        public void TestLogin()
        {
            // given

            AuthenticationSession expectedAuthenticationSession = new AuthenticationSession("http://gitlab.example.com/oauth/authorize", "1324");

            _configMock.Setup(c => c.GetStringParam(UserController.APPLICATION_BASE_URL)).Returns("http://my.app.com");
            _authenticationServiceMock.Setup(a => a.InitiateSession(It.IsAny<String>())).Returns(expectedAuthenticationSession);
            Mock<HttpResponse> responseReceiver = new Mock<HttpResponse>();
            Mock<IResponseCookies> responseCookies = new Mock<IResponseCookies>();
            responseReceiver.Setup(r => r.Cookies).Returns(responseCookies.Object);

            // when
            IActionResult returnAction = _userController.Login(responseReceiver.Object);

            // then
            Assert.IsType<RedirectResult>(returnAction);

            RedirectResult redirect = (RedirectResult)returnAction;

            Assert.Equal("http://gitlab.example.com/oauth/authorize", redirect.Url);

            _authenticationServiceMock.Verify(a => a.InitiateSession("http://my.app.com/user/verify"));

            CookieOptions expectedCookieOptions = new CookieOptions();
            expectedCookieOptions.HttpOnly = true;
            expectedCookieOptions.Secure = true;

            responseCookies.Verify(c => c.Append("x-state","1234",expectedCookieOptions));
        }
    }
}

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
        private Mock<IResponseCookies> responseCookies;

        public UserControllerTest() {

            this._authenticationServiceMock = new Mock<IAuthenticationService>();
            this._configMock = new Mock<IConfig>();
            this._userController = new UserController(_authenticationServiceMock.Object, _configMock.Object);
			_userController.ControllerContext = new ControllerContext();


            responseCookies = new Mock<IResponseCookies>();
            Mock<HttpResponse> httpResponseMock = new Mock<HttpResponse>();
			Mock<HttpContext> httpContextMock = new Mock<HttpContext>();

            httpResponseMock.Setup(r => r.Cookies).Returns(responseCookies.Object);
            httpContextMock.Setup(c => c.Response).Returns(httpResponseMock.Object);

            _userController.ControllerContext.HttpContext = httpContextMock.Object;

        }

        [Fact]
        public void TestLogin()
        {

            AuthenticationSession expectedAuthenticationSession = new AuthenticationSession("http://gitlab.example.com/oauth/authorize", "1234");

            _configMock.Setup(c => c.GetStringParam(UserController.APPLICATION_BASE_URL)).Returns("http://my.app.com");
            _authenticationServiceMock.Setup(a => a.InitiateSession(It.IsAny<String>())).Returns(expectedAuthenticationSession);

            // when
            IActionResult returnAction = _userController.Login();

            // then
            Assert.IsType<RedirectResult>(returnAction);

            RedirectResult redirect = (RedirectResult)returnAction;

            Assert.Equal("http://gitlab.example.com/oauth/authorize", redirect.Url);

            _authenticationServiceMock.Verify(a => a.InitiateSession("http://my.app.com/user/verify"));

            responseCookies.Verify(c => c.Append("x-state","1234",
                    It.Is<CookieOptions>(opt => opt.HttpOnly == true && opt.Secure == true)
            ));
        }
    }
}
// given

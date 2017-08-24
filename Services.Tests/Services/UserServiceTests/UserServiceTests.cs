using System;
using Services.Services.User;
using Moq;
using Services.Data;
using Xunit;
using System.Threading.Tasks;
using Services.Data.Entities;

namespace Services.Tests.Services.UserServiceTests
{
    public class UserServiceTests
    {
        private UserService _user_service;
        private Mock<IUserRepository> _user_repository_mock;

        public UserServiceTests() {
            this._user_repository_mock = new Mock<IUserRepository>();

            this._user_service = new UserService(_user_repository_mock.Object);
        
            [Fact]
            public void TestCreateProfile() {
            _user_repository_mock.Setup(r => r.FindUserByExternalId(It.IsAny<string>()))
                                 .Returns(Task.FromResult(null));


            }



    }


}

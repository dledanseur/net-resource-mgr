﻿using System;
using Services.Services.UserService;
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

        public UserServiceTests()
        {
            this._user_repository_mock = new Mock<IUserRepository>();

            this._user_service = new UserService(_user_repository_mock.Object);
        }

        [Fact]
        public async Task TestCreateProfile() {
            // given
            _user_repository_mock.Setup(r => r.FindUserByExternalId(It.IsAny<string>()))
                                .Returns(Task.FromResult<User>(null));


            UserProfile prof = JohnDoeUserProfile();

            // when
            await _user_service.CreateOrUpdateUserProfile(prof);

            // then
            User expectedEntity = JohnDoeEntity();

            _user_repository_mock.Verify(r => r.FindUserByExternalId("123"));
            _user_repository_mock.Verify(r => r.SaverUser(expectedEntity));

        }

        [Fact]
        public async Task TestUpdateProfile()
        {
            // given
            User janeDoeEntity = JaneDoeEntity();
            janeDoeEntity.ExternalId = "123";

            _user_repository_mock.Setup(r => r.FindUserByExternalId(It.IsAny<string>()))
                                .Returns(Task.FromResult(JaneDoeEntity()));


            UserProfile prof = JohnDoeUserProfile();

            // when
            await _user_service.CreateOrUpdateUserProfile(prof);

            // then
            User expectedEntity = JohnDoeEntity();

            _user_repository_mock.Verify(r => r.FindUserByExternalId("123"));
            _user_repository_mock.Verify(r => r.SaverUser(expectedEntity));

        }


        private UserProfile JohnDoeUserProfile()
        {
            UserProfile prof = new UserProfile();
            prof.ExternalId = "123";
            prof.Email = "a@a.com";
            prof.FullName = "John Doe";
            prof.UserName = "john.doe";

            return prof;
        }

        private User JaneDoeEntity()
        {
            User expectedEntity = new User();
            expectedEntity.FullName = "Jane Doe";
            expectedEntity.ExternalId = "456";
            expectedEntity.Email = "b@b.com";
            expectedEntity.UserName = "jane.doe";

            return expectedEntity;
        }

        private User JohnDoeEntity()
        {
            User expectedEntity = new User();
            expectedEntity.FullName = "John Doe";
            expectedEntity.ExternalId = "123";
            expectedEntity.Email = "a@a.com";
            expectedEntity.UserName = "john.doe";

            return expectedEntity;
        }


    }


}

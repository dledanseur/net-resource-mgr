using System;
namespace Services.Services.User
{
    public interface IUserService
    {
        void CreateOrUpdateUserProfile(UserProfile userProfile);
    }
}

using System;
using Services.Data;
using System.Threading.Tasks;
using Services.Data.Entities;

namespace Services.Services.UserService
{
    public class UserService: IUserService
    {
        private IUserRepository _user_repository;

        public UserService(IUserRepository userRepository)
        {
            this._user_repository = userRepository;
        }

        public async Task CreateOrUpdateUserProfile(UserProfile userProfile)
        {
            User u = await _user_repository.FindUserByExternalId(userProfile.ExternalId);

            if (u == null)
            {
                u = new User();
            }

            u.ExternalId = userProfile.ExternalId;
            u.Email = userProfile.Email;
            u.UserName = userProfile.UserName;
            u.FullName = userProfile.FullName;

            _user_repository.SaverUser(u);

        }
    }
}

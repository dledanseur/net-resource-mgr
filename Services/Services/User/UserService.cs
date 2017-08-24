using System;
using Services.Data;
using System.Threading.Tasks;
namespace Services.Services.User
{
    public class UserService: IUserService
    {
        private IUserRepository _user_repository;

        public UserService(IUserRepository userRepository)
        {
            this._user_repository = userRepository;
        }

        public Task CreateOrUpdateUserProfile(UserProfile userProfile)
        {
            throw new NotImplementedException();
        }
    }
}

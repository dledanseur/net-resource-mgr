using System;
using System.Threading.Tasks;

namespace Services.Services.UserService
{
    public interface IUserService
    {
        Task CreateOrUpdateUserProfile(UserProfile userProfile);
    }
}

using System;
using Services.Data.Entities;
using System.Threading.Tasks;

namespace Services.Data
{
    public class UserRepository: IUserRepository
    {
        public UserRepository()
        {
        }

        public Task<User> FindUserByExternalId(string externalId)
        {
            throw new NotImplementedException();
        }

        public void SaverUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}

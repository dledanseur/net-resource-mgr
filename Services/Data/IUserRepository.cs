using System;
using UserServices.Data.Entities;
using System.Threading.Tasks;
namespace Services.Data
{
    public interface IUserRepository
    {
        Task²²²²²<User> FindUserByExternalId(string externalId);

		void SaverUser(User user);

	}
}

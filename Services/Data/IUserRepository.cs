using System;
using Services.Data.Entities;
using System.Threading.Tasks;
namespace Services.Data
{
    public interface IUserRepository
    {
        Task<User> FindUserByExternalId(string externalId);

		Task SaverUser(User user);

	}
}

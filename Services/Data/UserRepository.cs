using System;
using Services.Data.Entities;
using System.Threading.Tasks;
using System.Linq;


namespace Services.Data
{
    public class UserRepository: IUserRepository
    {
        private EFDBContext _context;

        public UserRepository(EFDBContext context)
        {
            this._context = context;
        }

        public async Task<User> FindUserByExternalId(string externalId)
        {
            
            var query = from product in _context.Users
                        where product.ExternalId == externalId
                        select product;

            return await query.ToAsyncEnumerable().FirstOrDefault(null);
        }

        public async Task SaverUser(User user)
        {
            await _context.SaveChangesAsync();
        }
    }
}

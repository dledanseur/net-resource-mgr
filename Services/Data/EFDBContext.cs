using System;
using Microsoft.EntityFrameworkCore;
using Services.Data.Entities;
namespace Services.Data
{
    public class EFDBContext: DbContext
    {

        public EFDBContext(DbContextOptions<EFDBContext> ctx): base(ctx)
        {
            
        }

        public DbSet<MysqlResource> MysqlResources { get; set; }

        public DbSet<User> Users { get; set; }

    }
}

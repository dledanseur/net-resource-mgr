using System;
namespace Services.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int SchoolYear { get; set; }

        public User()
        {
        }
    }
}

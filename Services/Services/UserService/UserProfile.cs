using System;
namespace Services.Services.UserService
{
    public class UserProfile
    {

        public string ExternalId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }


        public UserProfile()
        {
        }
    }
}

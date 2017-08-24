using System;
namespace Services.Data.Entities
{
    public class MysqlResource : Resource
    {
        public string DatabaseName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public virtual User Owner { get; set; }
        public MysqlResource()
        {
        }
    }
}

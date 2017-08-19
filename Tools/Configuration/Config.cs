using System;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace Tools.Configuration
{
    public class Config : IConfig
	{
		private IConfiguration config;

		public Config(string env_prefix)
		{
			config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables(env_prefix)
				.AddJsonFile("appsettings.json")
				.Build();
		}

		public string ConnectionString
		{
			get
			{
				return this.GetConnectionString("DefaultConnection");

			}
		}

        public string GetConnectionString(string id)
        {
            return config.GetConnectionString(id);
        }

        public string GetStringParam(string id)
        {
            return config["Application:" + id];
        }
    }

}

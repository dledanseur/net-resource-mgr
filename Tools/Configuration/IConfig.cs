using System;
namespace Tools.Configuration
{
	public interface IConfig
	{
		string ConnectionString { get; }

        string GetConnectionString(String id);

        string GetStringParam(String id);
	}
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Tools.HttpHandler
{
    public interface RestClient
    {
        Task<T> Get<T>(string path, IDictionary<String, String> queryParams);

    }
}

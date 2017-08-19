using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Tools.HttpHandler
{
    public enum ArgumentEncoding
    {
        JSON,
        FORM
    }

    public interface IRestClient
    {
        Task<RestResponse<T>> Get<T>(string path, IDictionary<String, String> queryParams = null);

        Task<RestResponse<T>> Post<T>(string path, object content, ArgumentEncoding argEncoding=ArgumentEncoding.JSON);

    }
}

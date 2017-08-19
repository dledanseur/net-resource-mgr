using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
namespace Tools.HttpHandler
{
    public interface IHttpHandler
    {
        Task<HttpResponseMessage> Get(string uri);
        Task<HttpResponseMessage> Post(String uri, HttpContent content);
    }
}

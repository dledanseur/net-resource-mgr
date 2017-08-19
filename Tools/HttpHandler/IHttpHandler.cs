using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
namespace Tools.HttpHandler
{
    public interface IHttpHandler
    {
        Task Get(string uri);
        Task Post(String uri, HttpContent content);
    }
}

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tools.HttpClient.Impl
{
    public class HttpClient: IHttpClient
    {
        public HttpClient()
        {
        }

        public Task Get(string uri)
        {
            throw new NotImplementedException();
        }

        public Task Post(string uri, HttpContent content)
        {
            throw new NotImplementedException();
        }
    }
}

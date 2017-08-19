using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tools.HttpHandler.Impl
{
    public class HttpHandler : IHttpHandler
    {
        private HttpClient client = new HttpClient();

        public Task<HttpResponseMessage> Get(string uri)
        {
            return client.GetAsync(uri);
        }

        public Task<HttpResponseMessage> Post(string uri, HttpContent content)
        {
            return client.PostAsync(uri, content);
        }
    }
}

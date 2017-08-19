using System;
using Tools.HttpHandler.Impl;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Linq;

namespace Tools.HttpHandler.Impl
{
    public class RestClient : IRestClient
    {
        private IHttpHandler _httpHandler;

        public RestClient() : this(new Impl.HttpHandler()) {}

        public RestClient(IHttpHandler handler) {
            this._httpHandler = handler;
        }

        public async Task<RestResponse<T>> Get<T>(string path, IDictionary<string, string> queryParams = null)
        {

            String url = BuildUrl(path, queryParams);

            HttpResponseMessage response = await _httpHandler.Get(url);

			String stringContent = await response.Content.ReadAsStringAsync();

            return BuildResponse<T>(JsonConvert.DeserializeObject<T>(stringContent), response.Headers);
        }



        public async Task<RestResponse<T>> Post<T>(string path, object content, 
                ArgumentEncoding argEncoding = ArgumentEncoding.JSON)
        {
            string jsonContent = JsonConvert.SerializeObject(content);

            HttpResponseMessage response = await _httpHandler.Post(path, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

			String stringContent = await response.Content.ReadAsStringAsync();

            return BuildResponse<T>(JsonConvert.DeserializeObject<T>(stringContent), response.Headers);

        }

        private RestResponse<T> BuildResponse<T>(T content, HttpHeaders headers) {
            Dictionary<string, string[]> headersDic = new Dictionary<string, string[]>();

            foreach (var kv in headers) {
                
                string header = kv.Key;

                string[] values = kv.Value.ToArray();

                headersDic.Add(header,values);

            }
            return new RestResponse<T>(content, headersDic);    
        }

		private string BuildUrl(string path, IDictionary<string, string> queryParams)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(path);

			AppendQueryParams(builder, queryParams);

            return builder.ToString();
		}

		private void AppendQueryParams(StringBuilder builder, IDictionary<string, string> queryParams)
		{
            bool first = true;

			if (queryParams != null)
			{
                
				foreach (string k in queryParams.Keys)
				{
					if (first)
					{
						builder.Append("?");
						first = false;
					}
					else
					{
						builder.Append("&");
					}

					builder.Append(k);
					builder.Append("=");
					builder.Append(queryParams[k]);
				}
			}

		}
    }
}

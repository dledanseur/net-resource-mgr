using System;
using Tools.HttpHandler.Impl;
using System.Collections.Generic;

namespace Tools.HttpHandler
{
    public class RestClient
    {
        private IHttpHandler _httpHandler;

        public RestClient() : this(new Impl.HttpHandler()) {}

        public RestClient(IHttpHandler handler) {
            this._httpHandler = handler;
        }

        public void Get(string URI, IDictionary<String,String> queryParams) {
            
        }

        public void Post<T>(string URI, IDictionary<String,String> queryParams, T content) {
             
        }
    }
}

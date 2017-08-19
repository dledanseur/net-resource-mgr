using System;
using System.Collections.Generic;

namespace Tools.HttpHandler
{
    public class RestResponse<T>
    {
        public T Body { get; }

        public Dictionary<string, string[]> Headers { get; }

        public RestResponse(T body, Dictionary<string,string[]> headers) {
            this.Headers = headers;
            this.Body = body;
        } 


    }
}

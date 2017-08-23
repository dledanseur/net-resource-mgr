using System;
using System.Collections.Generic;

namespace Tools.HttpHandler
{
    /// <summary>
    /// Represents a REST call result
    /// </summary>
    public class RestResponse<T>
    {
        /// <summary>
        /// Response content, bound to the requested object type
        /// </summary>
        /// <value>The body.</value>
        public T Body { get; }

        /// <summary>
        /// All headers returned in the request, in the form of a key -> array_of_value dictionnary
        /// </summary>
        /// <value>The headers.</value>
        public Dictionary<string, string[]> Headers { get; }

        /// <summary>
        /// This flag is false if a failure status code has been returned, i.e.
        /// is between 400 and 599
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }

        public RestResponse(bool success, T body, Dictionary<string,string[]> headers) {
            this.Success = success;
            this.Headers = headers;
            this.Body = body;
        } 


    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;

namespace T.Utility.Http
{
    /// <summary>
    /// Represents a HTTP request message including the inital data.
    /// </summary>
    public class HttpRequestContext
    {
        /// <summary>
        /// Gets or sets the base address of the Internet resource used when sending requests.
        /// </summary>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets the segments of the Internet resource used when sending requests.
        /// </summary>
        public List<string> Segments { get; set; }

        /// <summary>
        /// Gets or sets the query params of the Internet resource used when sending requests.
        /// </summary>
        public Dictionary<string, object> QueryParams { get; set; }

        /// <summary>
        /// Gets the collection of HTTP request headers.
        /// </summary>
        public Dictionary<string, object> Headers { get; set; }

        /// <summary>
        /// Gets or sets the System.Uri used for the HTTP request.
        /// </summary>
        public Uri RequestUri { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method used by the HTTP request message.
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Gets or sets the contents of the HTTP message.
        /// </summary>
        public HttpContent Content { get; set; }

        /// <summary>
        /// Gets or sets the HttpClient for sending requests and receiving responses.
        /// </summary>
        public HttpClient Client { get; set; }
    }
}

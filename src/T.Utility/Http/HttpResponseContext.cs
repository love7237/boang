using System;
using System.Net;
using System.Net.Http.Headers;

namespace T.Utility.Http
{
    /// <summary>
    /// Represents a HTTP response message including the status code and data.
    /// </summary>
    public class HttpResponseContext<T>
    {
        /// <summary>
        /// Gets or sets a value that indicates if the HTTP response was successful.
        /// </summary>
        public bool IsSuccessStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the reason phrase which typically is sent by servers together with the status code.
        /// </summary>
        public string ReasonPhrase { get; set; }

        /// <summary>
        /// Gets or sets the status code of the HTTP response.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets the collection of trailing headers included in an HTTP response.
        /// </summary>
        public HttpResponseHeaders Headers { get; set; }

        /// <summary>
        /// Gets the HTTP content headers as defined in RFC 2616.
        /// </summary>
        public HttpContentHeaders ContentHeaders { get; set; }

        /// <summary>
        /// Gets or sets the content of a HTTP response message.
        /// </summary>
        public T Content { get; set; }

        /// <summary>
        /// Gets or sets the HTTP message version.
        /// </summary>
        public Version Version { get; set; }
    }
}

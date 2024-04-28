using System;

namespace T.Utility.Http
{
    /// <summary>
    /// HTTP请求结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpResult<T>
    {
        /// <summary>
        /// Gets or sets the request message which led to this response message.
        /// </summary>
        public HttpRequestContext Request { get; set; }

        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        public HttpResponseContext<T> Response { get; set; }

        /// <summary>
        /// Gets a value that indicates if the HTTP response was successful.
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return Response != null && Response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Gets the content of a HTTP response message.
        /// </summary>
        public T Content
        {
            get
            {
                return Response == null ? default : Response.Content;
            }
        }

        /// <summary>
        /// Gets or sets the exception during sending request or receiving response.
        /// </summary>
        public Exception Exception { get; set; }
    }
}

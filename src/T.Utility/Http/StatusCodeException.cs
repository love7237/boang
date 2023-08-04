using System;
using System.Runtime.Serialization;

namespace T.Utility.Http
{
    /// <summary>
    /// HTTP响应<see cref="System.Net.HttpStatusCode" />不在[200,299]范围内时触发此异常
    /// </summary>
    public class StatusCodeException : SystemException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCodeException" /> class.
        /// </summary>
        public StatusCodeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCodeException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public StatusCodeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCodeException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception.</param>
        public StatusCodeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCodeException" /> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected StatusCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

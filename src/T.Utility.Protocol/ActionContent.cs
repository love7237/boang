using System.Net;
using System.Runtime.Serialization;

namespace T.Utility.Protocol
{
    /// <summary>
    /// Defines a basic contract that represents the result of an action method.
    /// </summary>
    [DataContract]
    public class ActionContent
    {
        /// <summary>
        /// Gets or sets the status code of the action result.
        /// </summary>
        [DataMember(Name = "state", Order = 1)]
        public int State { get; set; }

        /// <summary>
        /// Gets or sets the description of the action result.
        /// </summary>
        [DataMember(Name = "desc", Order = 2)]
        public string Desc { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContent"/> class.
        /// </summary>
        public ActionContent() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContent"/> class with the specified parameters.
        /// </summary>
        /// <param name="state"></param>
        public ActionContent(int state)
        {
            this.State = state;
            this.Desc = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContent"/> class with the specified parameters.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="desc"></param>
        public ActionContent(int state, string desc)
        {
            this.State = state;
            this.Desc = desc ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContent"/> class with the specified parameters.
        /// </summary>
        /// <param name="statusCode"></param>
        public ActionContent(HttpStatusCode statusCode)
        {
            this.State = (int)statusCode;
            this.Desc = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContent"/> class with the specified parameters.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="desc"></param>
        public ActionContent(HttpStatusCode statusCode, string desc)
        {
            this.State = (int)statusCode;
            this.Desc = desc ?? string.Empty;
        }
    }

    /// <summary>
    /// Defines a generic implementation of basic <see cref="ActionContent" />.
    /// </summary>
    [DataContract]
    public class ActionContent<T> : ActionContent
    {
        /// <summary>
        /// Gets or sets the value of the action result.
        /// </summary>
        [DataMember(Name = "value", Order = 3)]
        public T Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContent"/> class.
        /// </summary>
        public ActionContent() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContent"/> class with the specified parameters.
        /// </summary>
        /// <param name="state"></param>
        public ActionContent(int state) : base(state) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContent"/> class with the specified parameters.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="desc"></param>
        public ActionContent(int state, string desc) : base(state, desc) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContent"/> class with the specified parameters.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="desc"></param>
        /// <param name="value"></param>
        public ActionContent(int state, string desc, T value) : base(state, desc)
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContent"/> class with the specified parameters.
        /// </summary>
        /// <param name="statusCode"></param>
        public ActionContent(HttpStatusCode statusCode) : base(statusCode) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContent"/> class with the specified parameters.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="desc"></param>
        public ActionContent(HttpStatusCode statusCode, string desc) : base(statusCode, desc) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContent"/> class with the specified parameters.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="desc"></param>
        /// <param name="value"></param>
        public ActionContent(HttpStatusCode statusCode, string desc, T value) : base(statusCode, desc)
        {
            this.Value = value;
        }
    }
}

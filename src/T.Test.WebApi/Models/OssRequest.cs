using System.Runtime.Serialization;
using T.Utility.Protocol;

namespace T.Test.WebApi.Models
{
    /// <summary>
    /// 对象存储请求
    /// </summary>
    [DataContract]
    public class OssRequest
    {
        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "key")]
        public string Key { get; set; }

        /// <summary>
        /// 图像数据类型
        /// </summary>
        [DataMember(Name = "imgType")]
        public ImageType ImageType { get; set; }

        /// <summary>
        /// 图像数据
        /// </summary>
        [DataMember(Name = "imgSource")]
        public string ImageSource { get; set; }
    }

}

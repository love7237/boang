using System.Runtime.Serialization;
using T.Utility.Protocol;

namespace T.Test.WebApi.Models
{
    /// <summary>
    /// 算法请求基类
    /// </summary>
    [DataContract]
    public class AlgorithmRequest
    {
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

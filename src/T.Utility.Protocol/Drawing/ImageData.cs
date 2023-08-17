using System.Runtime.Serialization;

namespace T.Utility.Protocol
{
    /// <summary>
    /// 图像数据封装
    /// </summary>
    [DataContract]
    public class ImageData
    {
        /// <summary>
        /// 数据类型
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

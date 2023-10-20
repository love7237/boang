using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using T.Utility.Protocol;

namespace T.Test.WebApi.Models
{
    /// <summary>
    /// 车脸车型识别请求
    /// </summary>
    [DataContract]
    public class FaceModelRequest
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        [DataMember(Name = "imgType")]
        public ImageType ImageType { get; set; }

        /// <summary>
        /// 图像数据
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [DataMember(Name = "imgSource")]
        public string ImageSource { get; set; }

        /// <summary>
        /// 车脸顶点(x1,y1,x2,y2)
        /// </summary>
        [DataMember(Name = "facePoints")]
        public List<int> FacePoints { get; set; }
    }
}

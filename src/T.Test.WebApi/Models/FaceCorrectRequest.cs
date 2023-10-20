using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using T.Utility.Protocol;

namespace T.Test.WebApi.Models
{
    /// <summary>
    /// 车脸矫正请求
    /// </summary>
    [DataContract]
    public class FaceCorrectRequest
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
        /// 车牌顶点(x1,y1,x2,y2,x3,y3,x4,y4)
        /// </summary>
        [DataMember(Name = "platePoints")]
        public List<int> PlatePoints { get; set; }
    }
}

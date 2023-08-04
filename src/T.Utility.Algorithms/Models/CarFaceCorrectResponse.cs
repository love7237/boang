using System.Collections.Generic;
using System.Runtime.Serialization;
using T.Utility.Protocol;

namespace T.Utility.Algorithms
{
    /// <summary>
    /// 车头车尾矫正请求
    /// </summary>
    [DataContract]
    public class CarFaceCorrectRequest
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [DataMember(Name = "oprNum")]
        public string OprNum { get; set; }

        /// <summary>
        /// 图像数据类型
        /// </summary>
        [DataMember(Name = "imgType")]
        public ImageType ImgType { get; set; }

        /// <summary>
        /// 图像数据
        /// </summary>
        [DataMember(Name = "imgSource")]
        public string ImgSource { get; set; }

        /// <summary>
        /// 车牌顶点坐标列表(x1,y1,x2,y2,x3,y3,x4,y4)
        /// </summary>
        [DataMember(Name = "platePoints")]
        public List<int> Points { get; set; }
    }

    /// <summary>
    /// 车头车尾矫正响应
    /// </summary>
    [DataContract]
    public class CarFaceCorrectResponse
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [DataMember(Name = "oprNum")]
        public string OprNum { get; set; }

        /// <summary>
        /// 接口状态码(0 正常;1 调试;2 失败)
        /// </summary>
        [DataMember(Name = "state")]
        public int State { get; set; }

        /// <summary>
        /// 接口状态码描述(success 成功;debug 调试;failed 失败)
        /// </summary>
        [DataMember(Name = "desc")]
        public string Desc { get; set; }

        /// <summary>
        /// 车头车尾类型
        /// </summary>
        [DataMember(Name = "label")]
        public MulticlassType Label { get; set; }

        /// <summary>
        /// 车头车尾图像数据(base64)
        /// </summary>
        [DataMember(Name = "base64ImgData")]
        public string FaceImgSource { get; set; }

        /// <summary>
        /// 车头车尾顶点坐标(x1,y1,x2,y2,x3,y3,x4,y4)
        /// </summary>
        [DataMember(Name = "carFacePoints")]
        public List<int> FacePoints { get; set; }
    }

    /// <summary>
    /// 车头车尾矫正结果封装
    /// </summary>
    [DataContract]
    public class CarFaceCorrectContent
    {
        /// <summary>
        /// 车头车尾类型
        /// </summary>
        [DataMember(Name = "faceType")]
        public FaceType FaceType { get; set; }

        /// <summary>
        /// 车头车尾图像数据(base64)
        /// </summary>
        [DataMember(Name = "faceImgSource")]
        public string FaceImgSource { get; set; }

        /// <summary>
        /// 车头车尾顶点坐标(x1,y1,x2,y2,x3,y3,x4,y4)
        /// </summary>
        [DataMember(Name = "facePoints")]
        public List<int> FacePoints { get; set; }
    }

}

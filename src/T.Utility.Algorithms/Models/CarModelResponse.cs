using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using T.Utility.Extensions;
using T.Utility.Protocol;

namespace T.Utility.Algorithms
{
    /// <summary>
    /// 车型识别请求
    /// </summary>
    [DataContract]
    public class CarModelRequest
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
        /// 
        /// </summary>
        [DataMember(Name = "area_left")]
        public int? Left { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "area_top")]
        public int? Top { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "area_right")]
        public int? Right { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "area_bottom")]
        public int? Bottom { get; set; }
    }

    /// <summary>
    /// 车型识别响应
    /// </summary>
    [DataContract]
    public class CarModelResponse
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
        /// 车型索引
        /// </summary>
        [DataMember(Name = "nLabelIndex")]
        public int Label { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        [DataMember(Name = "vehicleName")]
        public string Name { get; set; }

        /// <summary>
        /// 置信度
        /// </summary>
        [DataMember(Name = "fConfidence")]
        public double Confidence { get; set; }
    }

    /// <summary>
    /// 车型识别结果简单封装
    /// </summary>
    [DataContract]
    public class CarModelSimpleContent
    {
        /// <summary>
        /// 车型索引
        /// </summary>
        [DataMember(Name = "label")]
        public int Label { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 置信度
        /// </summary>
        [DataMember(Name = "confidence")]
        public double Confidence { get; set; }
    }

    /// <summary>
    /// 车型识别结果扩展封装
    /// </summary>
    [DataContract]
    public class CarModelExtendContent
    {
        /// <summary>
        /// 车头车尾类型
        /// </summary>
        [DataMember(Name = "faceType")]
        public FaceType FaceType { get; set; }

        /// <summary>
        /// 车型索引
        /// </summary>
        [DataMember(Name = "label")]
        public int Label { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 置信度
        /// </summary>
        [DataMember(Name = "confidence")]
        public double Confidence { get; set; }

        /// <summary>
        /// 顶点坐标列表(x1,y1,x2,y2)
        /// </summary>
        [DataMember(Name = "points")]
        public List<int> Points { get; set; }

        /// <summary>
        /// 矩形区域
        /// </summary>
        [IgnoreDataMember]
        public Rectangle Rect
        {
            get
            {
                if (this.Points.IsNullOrEmpty() || this.Points.Count != 4)
                    return Rectangle.Empty;

                return new Rectangle(Points[0], Points[1], Points[2] - Points[0], Points[3] - Points[1]);
            }
        }
    }

}

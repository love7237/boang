using System.Collections.Generic;
using System.Runtime.Serialization;

namespace T.Utility.Algorithms
{
    /// <summary>
    /// 泊位高点映射请求
    /// </summary>
    [DataContract]
    public class BerthRegionReshapeRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "oprNum")]
        public string OprNum { get; set; }

        /// <summary>
        /// 像机分辨率，默认值1920
        /// </summary>
        [DataMember(Name = "cameraWidth")]
        public int CameraWidth { get; set; }

        /// <summary>
        /// 像机分辨率，默认值1080
        /// </summary>
        [DataMember(Name = "cameraHeight")]
        public int CameraHeight { get; set; }

        /// <summary>
        /// 泊位宽度(米)，默认值2.2
        /// </summary>
        [DataMember(Name = "berthWidth")]
        public double BerthWidth { get; set; }

        /// <summary>
        /// 泊位长度(米)，默认值5.5
        /// </summary>
        [DataMember(Name = "berthLength")]
        public double BerthLength { get; set; }

        /// <summary>
        /// 泊位高度(米)，默认值1.5
        /// </summary>
        [DataMember(Name = "berthHeight")]
        public double BerthHeight { get; set; }

        /// <summary>
        /// 低点归一化值(x1,y1,x2,y2,x3,y3,x4,y4)
        /// </summary>
        [DataMember(Name = "berthPoints")]
        public List<double> BerthPoints { get; set; }
    }

    /// <summary>
    /// 泊位高点映射响应
    /// </summary>
    [DataContract]
    public class BerthRegionReshapeResponse
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
        /// 像机分辨率，默认值1920
        /// </summary>
        [DataMember(Name = "cameraWidth")]
        public int CameraWidth { get; set; }

        /// <summary>
        /// 像机分辨率，默认值1080
        /// </summary>
        [DataMember(Name = "cameraHeight")]
        public int CameraHeight { get; set; }

        /// <summary>
        /// 泊位宽度(米)，默认值2.2
        /// </summary>
        [DataMember(Name = "berthWidth")]
        public double BerthWidth { get; set; }

        /// <summary>
        /// 泊位长度(米)，默认值5.5
        /// </summary>
        [DataMember(Name = "berthLength")]
        public double BerthLength { get; set; }

        /// <summary>
        /// 泊位高度(米)，默认值1.5
        /// </summary>
        [DataMember(Name = "berthHeight")]
        public double BerthHeight { get; set; }

        /// <summary>
        /// 低点和高点归一化值(x1,y1,x2,y2,x3,y3,x4,y4,x5,y5,x6,y6,x7,y7,x8,y8)
        /// </summary>
        [DataMember(Name = "berthPoints")]
        public List<double> BerthPoints { get; set; }
    }
}

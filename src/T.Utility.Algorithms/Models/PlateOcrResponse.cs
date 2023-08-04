using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using T.Utility.Extensions;
using T.Utility.Protocol;

namespace T.Utility.Algorithms
{
    /// <summary>
    /// 车牌识别请求
    /// </summary>
    [DataContract]
    public class PlateOcrRequest
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
    }

    /// <summary>
    /// 车牌识别响应
    /// </summary>
    [DataContract]
    public class PlateOcrResponse
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
        /// 图片类型
        /// </summary>
        [DataMember(Name = "imgType")]
        public ImageType ImgType { get; set; }

        /// <summary>
        /// 图像源数据
        /// </summary>
        [DataMember(Name = "imgSource")]
        public string ImgSource { get; set; }

        /// <summary>
        /// 车牌对象列表
        /// </summary>
        [DataMember(Name = "plates")]
        public List<PlateOcrObject> Plates { get; set; }
    }

    /// <summary>
    /// 车牌区域对象
    /// </summary>
    [DataContract]
    public class PlateOcrObject
    {
        /// <summary>
        /// 车牌号码
        /// </summary>
        [DataMember(Name = "result")]
        public string PlateNumber { get; set; }

        /// <summary>
        /// 车牌颜色
        /// </summary>
        [DataMember(Name = "color")]
        public PlateColor PlateColor { get; set; }

        /// <summary>
        /// 置信度([0,1])
        /// </summary>
        [DataMember(Name = "score")]
        public double Score { get; set; }

        /// <summary>
        /// 遮挡阈值([0,1])
        /// </summary>
        [DataMember(Name = "cover")]
        public double Cover { get; set; }

        /// <summary>
        /// 参与投票的算法结果列表
        /// </summary>
        [DataMember(Name = "ocr")]
        public List<PlateOcrOption> OcrOptions { get; set; }

        /// <summary>
        /// 顶点坐标列表(x1,y1,x2,y2,x3,y3,x4,y4)
        /// </summary>
        [DataMember(Name = "points")]
        public List<int> Points { get; set; }
    }

    /// <summary>
    /// 参与投票的算法结果
    /// </summary>
    [DataContract]
    public class PlateOcrOption
    {
        /// <summary>
        /// 算法类型(1 yolo;2 caffe;3 shuffle)
        /// </summary>
        [DataMember(Name = "type")]
        public int Type { get; set; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        [DataMember(Name = "result")]
        public string PlateNumber { get; set; }
    }

    /// <summary>
    /// 车牌识别结果封装
    /// </summary>
    [DataContract]
    public class PlateOcrContent
    {
        /// <summary>
        /// 车牌号码
        /// </summary>
        [DataMember(Name = "plateNumber")]
        public string PlateNumber { get; set; }

        /// <summary>
        /// 车牌颜色
        /// </summary>
        [DataMember(Name = "plateColor")]
        public PlateColor PlateColor { get; set; }

        /// <summary>
        /// 置信度([0,1])
        /// </summary>
        [DataMember(Name = "score")]
        public double Score { get; set; }

        /// <summary>
        /// 遮挡阈值([0,1])
        /// </summary>
        [DataMember(Name = "cover")]
        public double Cover { get; set; }

        /// <summary>
        /// 顶点坐标列表(x1,y1,x2,y2,x3,y3,x4,y4)
        /// </summary>
        [DataMember(Name = "points")]
        public List<int> Points { get; set; }

        /// <summary>
        /// 倾斜角度
        /// </summary>
        [IgnoreDataMember]
        public double Angle
        {
            get
            {
                if (this.Points.IsNullOrEmpty() || this.Points.Count != 8)
                    return 0;

                int y = Points[5] - Points[7];
                int x = Points[4] - Points[6];

                return Math.Atan2(y * -1.0, x) * 180 / Math.PI;
            }
        }

        /// <summary>
        /// 矩形区域
        /// </summary>
        [IgnoreDataMember]
        public Rectangle Rect
        {
            get
            {
                if (this.Points.IsNullOrEmpty() || this.Points.Count != 8)
                    return Rectangle.Empty;

                var xs = new List<int>() { Points[0], Points[2], Points[4], Points[6] };
                var ys = new List<int>() { Points[1], Points[3], Points[5], Points[7] };

                return new Rectangle(xs.Min(), ys.Min(), xs.Max() - xs.Min(), ys.Max() - ys.Min());
            }
        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using T.Utility.Extensions;
using T.Utility.Protocol;

namespace T.Utility.Algorithms
{
    /// <summary>
    /// 九类检测请求参数
    /// </summary>
    [DataContract]
    public class MulticlassRequest
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
        /// 保留测试文件
        /// </summary>
        [DataMember(Name = "draw_flag")]
        public bool Flag { get; set; } = false;

        /// <summary>
        /// 置信度过滤值(默认0.3)
        /// </summary>
        [DataMember(Name = "fThresh")]
        public double Thresh { get; set; } = 0.3;
    }

    /// <summary>
    /// 九类检测响应
    /// </summary>
    [DataContract]
    public class MulticlassResponse
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
        /// 原始图像的列
        /// </summary>
        [DataMember(Name = "img_cols")]
        public int Cols { get; set; }

        /// <summary>
        /// 原始图像的行
        /// </summary>
        [DataMember(Name = "img_rows")]
        public int Rows { get; set; }

        /// <summary>
        /// 目标对象列表
        /// </summary>
        [DataMember(Name = "arr")]
        public List<MulticlassObject> Objects { get; set; }
    }

    /// <summary>
    /// 九类检测目标对象
    /// </summary>
    [DataContract]
    public class MulticlassObject
    {
        /// <summary>
        /// 目标类型
        /// </summary>
        [DataMember(Name = "label")]
        public MulticlassType Label { get; set; }

        /// <summary>
        /// 置信度
        /// </summary>
        [DataMember(Name = "fScore")]
        public double Score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "left")]
        public int Left { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "top")]
        public int Top { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "right")]
        public int Right { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "bottom")]
        public int Bottom { get; set; }
    }

    /// <summary>
    /// 九类检测结果封装
    /// </summary>
    [DataContract]
    public class MulticlassContent
    {
        /// <summary>
        /// 目标类型
        /// </summary>
        [DataMember(Name = "label")]
        public MulticlassType Label { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get { return Label.ToString(); } }

        /// <summary>
        /// 置信度
        /// </summary>
        [DataMember(Name = "score")]
        public double Score { get; set; }

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

    /// <summary>
    /// 九类检测类型枚举
    /// </summary>
    public enum MulticlassType
    {
        /// <summary>
        /// 车辆
        /// </summary>
        car = 0,

        /// <summary>
        /// 行人
        /// </summary>
        person = 1,

        /// <summary>
        /// 摩托车或电动车
        /// </summary>
        motorbike = 2,

        /// <summary>
        /// 自行车
        /// </summary>
        bike = 3,

        /// <summary>
        /// 三轮车
        /// </summary>
        tricycle = 4,

        /// <summary>
        /// 人脸
        /// </summary>
        face = 5,

        /// <summary>
        /// 车牌
        /// </summary>
        plate = 6,

        /// <summary>
        /// 车头
        /// </summary>
        head = 7,

        /// <summary>
        /// 车尾
        /// </summary>
        tail = 8,

        /// <summary>
        /// 头肩
        /// </summary>
        shoulder = 9
    }

}

using System.Runtime.Serialization;

namespace T.Utility.Algorithms
{
    /// <summary>
    /// 按需取图(视频)请求
    /// </summary>
    [DataContract]
    public class DeviceFirstRequest
    {
        /// <summary>
        /// 车场编码
        /// </summary>
        [DataMember(Name = "parkCode")]
        public string ParkCode { get; set; }

        /// <summary>
        /// 泊位编码
        /// </summary>
        [DataMember(Name = "berthNumber")]
        public string BerthCode { get; set; }

        /// <summary>
        /// 数据类型(1 图片;2 视频)
        /// </summary>
        [DataMember(Name = "type")]
        public int Type { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember(Name = "startTime")]
        public long StartTime { get; set; }

        /// <summary>
        /// 结束时间(仅取视频时需要)
        /// </summary>
        [DataMember(Name = "endTime")]
        public long EndTime { get; set; }

        /// <summary>
        /// 系统标识
        /// </summary>
        [DataMember(Name = "tag")]
        public string Tag { get; set; } = "alg";
    }

    /// <summary>
    /// 查询响应结果
    /// </summary>
    [DataContract]
    public class DeviceFirstRequestResult
    {
        /// <summary>
        /// 状态码(1 成功;0 设备不在线;-1 请求体非法;-2 请求的矩阵号与查证的矩阵号不一致;-3 未查到板卡号与泊位号对应的矩阵号)
        /// </summary>
        [DataMember(Name = "result")]
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember(Name = "msg")]
        public string Message { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        [DataMember(Name = "type")]
        public int Type { get; set; }

        /// <summary>
        /// 请求唯一标识
        /// </summary>
        [DataMember(Name = "taskId")]
        public string TaskId { get; set; }
    }

    /// <summary>
    /// 查询取图(视频)结果
    /// </summary>
    [DataContract]
    public class DeviceSecondRequestResult
    {
        /// <summary>
        /// 状态码(1 结果返回,取图成功;0 taskId不能为空;2 结果返回,取图为空;-1 结果未返回;-2 查询的任务不存在)
        /// </summary>
        [DataMember(Name = "result")]
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember(Name = "msg")]
        public string Message { get; set; }

        /// <summary>
        /// 请求唯一标识
        /// </summary>
        [DataMember(Name = "taskId")]
        public string TaskId { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        [DataMember(Name = "value")]
        public DeviceResponseImage Value { get; set; }
    }

    /// <summary>
    /// 设备响应的按需取图(视频)信息
    /// </summary>
    [DataContract]
    public class DeviceResponseImage
    {
        /// <summary>
        /// 图片(视频)Url(注意包含-internal字段)
        /// </summary>
        [DataMember(Name = "imgUrls")]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 请求唯一标识
        /// </summary>
        [DataMember(Name = "taskId")]
        public string TaskId { get; set; }
    }
}

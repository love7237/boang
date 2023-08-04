using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace T.Utility.Algorithms
{
    /// <summary>
    /// 泊位历史状态查询参数
    /// </summary>
    [DataContract]
    public class BerthHistoryStateRequest
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
        /// 查询起始时间
        /// </summary>
        [DataMember(Name = "beginTime")]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 查询结束时间
        /// </summary>
        [DataMember(Name = "endTime")]
        public DateTime EndTime { get; set; }
    }

    /// <summary>
    /// 泊位历史状态查询响应
    /// </summary>
    [DataContract]
    public class BerthHistoryStateResponse
    {
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
        /// 查询起始时间
        /// </summary>
        [DataMember(Name = "beginTime")]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 查询结束时间
        /// </summary>
        [DataMember(Name = "endTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 历史状态列表(默认按时间倒序)
        /// </summary>
        [DataMember(Name = "berthState")]
        public List<BerthHistoryState> HistoryStates { get; set; }
    }

    /// <summary>
    /// 泊位历史状态
    /// </summary>
    [DataContract]
    public class BerthHistoryState
    {
        /// <summary>
        /// 状态(0 无车;1 有车)
        /// </summary>
        [DataMember(Name = "parkState")]
        public int ParkState { get; set; }

        /// <summary>
        /// 状态时间戳
        /// </summary>
        [DataMember(Name = "time")]
        public long Timestamp { get; set; }
    }

    /// <summary>
    /// 泊位历史状态结果封装
    /// </summary>
    [DataContract]
    public class BerthHistoryStateContent
    {
        /// <summary>
        /// 车场编码
        /// </summary>
        [DataMember(Name = "parkCode")]
        public string ParkCode { get; set; }

        /// <summary>
        /// 泊位编码
        /// </summary>
        [DataMember(Name = "berthCode")]
        public string BerthCode { get; set; }

        /// <summary>
        /// 查询起始时间
        /// </summary>
        [DataMember(Name = "beginTime")]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 查询结束时间
        /// </summary>
        [DataMember(Name = "endTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 历史状态列表
        /// </summary>
        [DataMember(Name = "states")]
        public List<BerthHistoryStateItem> HistoryStates { get; set; }
    }

    /// <summary>
    /// 泊位历史状态结果封装
    /// </summary>
    [DataContract]
    public class BerthHistoryStateItem
    {
        /// <summary>
        /// 状态(0 无车;1 有车)
        /// </summary>
        [DataMember(Name = "state")]
        public int ParkState { get; set; }

        /// <summary>
        /// 状态时刻
        /// </summary>
        [DataMember(Name = "time")]
        public DateTime Timestamp { get; set; }
    }
}

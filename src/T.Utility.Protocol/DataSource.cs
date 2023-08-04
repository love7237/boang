namespace T.Utility.Protocol
{
    /// <summary>
    /// 平台数据源(设备)类型
    /// </summary>
    public enum DataSource
    {
        /// <summary>
        /// 其他
        /// </summary>
        其他 = 0,

        /// <summary>
        /// 枪球联动设备
        /// </summary>
        视频设备A = 1,

        /// <summary>
        /// 手持终端设备
        /// </summary>
        手持设备 = 2,

        /// <summary>
        /// 封闭停车场设备
        /// </summary>
        封闭停车场 = 3,

        /// <summary>
        /// 自主研发设备
        /// </summary>
        视频设备B = 4,

        /// <summary>
        /// 异常列表校正
        /// </summary>
        异常列表 = 5,

        /// <summary>
        /// 车牌校验校正
        /// </summary>
        人工校验 = 7,

        /// <summary>
        /// 手动录入
        /// </summary>
        巡检录入 = 9,

        /// <summary>
        /// 海康设备
        /// </summary>
        视频设备C = 10,

        /// <summary>
        /// 平行多枪
        /// </summary>
        视频设备D = 11,

        /// <summary>
        /// 中位
        /// </summary>
        视频设备E = 12,

        /// <summary>
        /// 无人停管设备
        /// </summary>
        无人停管设备 = 13,

        /// <summary>
        /// 自研半封闭设备
        /// </summary>
        视频设备G = 14,

        /// <summary>
        /// 双球设备
        /// </summary>
        视频设备H = 15,

        /// <summary>
        /// 视频桩设备
        /// </summary>
        视频设备I = 16,

        /// <summary>
        /// 视频桩第二设备
        /// </summary>
        视频设备K = 17,

        /// <summary>
        /// 巡检车
        /// </summary>
        巡检车 = 18,

        /// <summary>
        /// 自研视频桩设备
        /// </summary>
        视频设备M = 20,

        /// <summary>
        /// 三方平台
        /// </summary>
        三方平台 = 99,

        /// <summary>
        /// 海康违停球设备
        /// </summary>
        违停球 = 100,

        /// <summary>
        /// 荆州设备
        /// </summary>
        荆州设备 = 102,

        /// <summary>
        /// 用户自主离场
        /// </summary>
        用户自主离场 = 110,

        /// <summary>
        /// 系统僵尸车离场
        /// </summary>
        系统僵尸车离场 = 111,

        /// <summary>
        /// APP自主入场
        /// </summary>
        APP自主入场 = 113
    }

    /// <summary>
    /// 平台数据源(设备)子类型
    /// </summary>
    public enum DataSourceDetail
    {
        /// <summary>
        /// 其他
        /// </summary>
        其他 = 0,

        /// <summary>
        /// 垂停设备：标准垂停
        /// </summary>
        标准垂停 = 1,

        /// <summary>
        /// 垂停设备：高配垂停
        /// </summary>
        高配垂停 = 2,

        /// <summary>
        /// 垂停设备：斜停垂停
        /// </summary>
        斜停 = 3,

        /// <summary>
        /// 平行多枪：平行平行
        /// </summary>
        标准平行 = 11,

        /// <summary>
        /// 平行多枪：异侧平行
        /// </summary>
        异侧平行 = 12,

        /// <summary>
        /// 平行多枪：内嵌平行
        /// </summary>
        内嵌平行 = 13,

        /// <summary>
        /// 平行多枪：模拟平行
        /// </summary>
        模拟平行 = 14,

        /// <summary>
        /// 巡检车
        /// </summary>
        视频巡检车 = 18,

        /// <summary>
        /// 中位设备：车头向中位
        /// </summary>
        车头向中位 = 21,

        /// <summary>
        /// 中位设备：灯下黑中位
        /// </summary>
        灯下黑中位 = 22,

        /// <summary>
        /// 中位设备：车尾向中位
        /// </summary>
        车尾向中位 = 23,

        /// <summary>
        /// 半封闭设备
        /// </summary>
        标准半封闭 = 31,

        /// <summary>
        /// 海康小型球机：标准球机
        /// </summary>
        标准球机 = 41,

        /// <summary>
        /// 海康小型球机：异侧球机
        /// </summary>
        异侧球机 = 42,

        /// <summary>
        /// 海康小型球机：内嵌球机
        /// </summary>
        内嵌球机 = 43,

        /// <summary>
        /// 海康小型球机：模拟球机
        /// </summary>
        模拟球机 = 44,

        /// <summary>
        /// 海康双小球：标准双小球
        /// </summary>
        标准双小球 = 51,

        /// <summary>
        /// 海康双小球：异侧双小球
        /// </summary>
        异侧双小球 = 52,

        /// <summary>
        /// 视频桩：视频桩单目
        /// </summary>
        视频桩单目 = 61,

        /// <summary>
        /// 视频桩：视频桩双目
        /// </summary>
        视频桩双目 = 62,

        /// <summary>
        /// 系统推车：泊位离线推车
        /// </summary>
        泊位离线推车 = 71,

        /// <summary>
        /// 系统推车：白虚线推车
        /// </summary>
        白虚线推车 = 71,

        /// <summary>
        /// 系统推车：定时推车
        /// </summary>
        定时推车 = 73,

        /// <summary>
        /// 系统推车：一键推车
        /// </summary>
        一键推车 = 74,

        /// <summary>
        /// 第三方
        /// </summary>
        第三方 = 90,

        /// <summary>
        /// 第三方PDA
        /// </summary>
        第三方PDA = 91,

        /// <summary>
        /// 第三方用户自主出入场
        /// </summary>
        第三方用户自主出入场 = 92
    }
}

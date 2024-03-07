namespace T.Utility.Protocol
{
    /// <summary>
    /// 设备类型
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// 其他
        /// </summary>
        其他 = 0,

        /// <summary>
        /// 自主研发设备
        /// </summary>
        视频设备B = 4,

        /// <summary>
        /// 视频设备D
        /// </summary>
        平行多枪 = 11,

        /// <summary>
        /// 视频设备E
        /// </summary>
        中位 = 12,

        /// <summary>
        /// 视频设备M
        /// </summary>
        视频桩 = 20
    }

    /// <summary>
    /// 设备子类型
    /// </summary>
    public enum DeviceSubType
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
        /// 平行多枪：吊装灯下黑
        /// </summary>
        吊装灯下黑 = 15,

        /// <summary>
        /// 平行多枪：吊装车头向
        /// </summary>
        吊装车头向 = 16,

        /// <summary>
        /// 平行多枪：吊装车尾向
        /// </summary>
        吊装车尾向 = 17,

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
        /// 视频桩：视频桩单目
        /// </summary>
        视频桩单目 = 61,

        /// <summary>
        /// 视频桩：视频桩双目
        /// </summary>
        视频桩双目 = 62,
    }
}

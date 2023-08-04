namespace T.Utility.Protocol
{
    /// <summary>
    /// 泊位质量枚举
    /// </summary>
    public enum BerthQuality
    {
        /// <summary>
        /// 坏泊位
        /// </summary>
        故障泊位 = 0,

        /// <summary>
        /// 好泊位
        /// </summary>
        正常泊位 = 1,

        /// <summary>
        /// 画线异常
        /// </summary>
        画线异常 = 2,

        /// <summary>
        /// 泊位重复
        /// </summary>
        泊位重复 = 3,

        /// <summary>
        /// 有遮挡物
        /// </summary>
        有遮挡物 = 4,

        /// <summary>
        /// 相机偏移
        /// </summary>
        相机偏移 = 5,

        /// <summary>
        /// 图片模糊
        /// </summary>
        图片模糊 = 6,

        /// <summary>
        /// 夜间模糊
        /// </summary>
        夜间模糊 = 7,

        /// <summary>
        /// 树枝遮挡
        /// </summary>
        树枝遮挡 = 8,

        /// <summary>
        /// 离线
        /// </summary>
        相机离线 = 9,

        /// <summary>
        /// 水印异常
        /// </summary>
        水印异常 = 10,

        /// <summary>
        /// 临时关闭
        /// </summary>
        临时关闭 = 11,

        /// <summary>
        /// 低质泊位
        /// </summary>
        低质泊位 = 12
    }
}

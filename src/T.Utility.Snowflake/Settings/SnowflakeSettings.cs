namespace T.Utility.Snowflake
{
    /// <summary>
    /// 雪花Id生成参数
    /// </summary>
    public class SnowflakeSettings
    {
        /// <summary>
        /// 数据中心Id，取值范围 [0,31]
        /// </summary>
        public int CenterId { get; set; }

        /// <summary>
        /// 工作节点Id，取值范围 [0,31]
        /// </summary>
        public int WorkerId { get; set; }
    }
}

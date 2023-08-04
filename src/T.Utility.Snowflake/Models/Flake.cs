using System;

namespace T.Utility.Snowflake
{
    /// <summary>
    /// 雪花Id
    /// </summary>
    public class Flake
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 数据中心Id
        public long CenterId { get; set; }

        /// <summary>
        /// 工作节点Id
        /// </summary>
        public long WorkerId { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 毫秒序列
        /// </summary>
        public long Sequence { get; set; }
    }
}

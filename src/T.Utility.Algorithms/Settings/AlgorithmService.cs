using System.Collections.Generic;

namespace T.Utility.Algorithms
{
    /// <summary>
    /// 算法服务配置信息
    /// </summary>
    public class AlgorithmService
    {
        /// <summary>
        /// 支持的算法列表
        /// </summary>
        public readonly List<string> Options = new List<string> { "车牌识别", "九类检测", "车型识别", "泊位高点映射", "泊位历史状态" };

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string Url { get; set; }
    }
}

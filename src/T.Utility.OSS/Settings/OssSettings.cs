using System.Collections.Generic;

namespace T.Utility.OSS
{
    /// <summary>
    /// 对象存储配置
    /// </summary>
    public class OssSettings
    {
        /// <summary>
        /// 适配方案
        /// </summary>
        public string Adapter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool SSL { get; set; }

        /// <summary>
        /// 网络地址转换列表
        /// </summary>
        public List<IntranetTransform> IntranetTransforms { get; set; }
    }

    /// <summary>
    /// 网络地址转换
    /// </summary>
    public class IntranetTransform
    {
        /// <summary>
        /// 转换前的地址
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 转换后的地址
        /// </summary>
        public string Target { get; set; }
    }


    /// <summary>
    /// 对象存储适配方案类型
    /// </summary>
    public class Adapter
    {
        #region 支持的方案

        /// <summary>
        /// 阿里云
        /// </summary>
        public const string Aliyun = "Aliyun";

        /// <summary>
        /// 腾讯云
        /// </summary>
        public const string Tencent = "Tencent";

        /// <summary>
        /// 华为云
        /// </summary>
        public const string Huawei = "Huawei";

        /// <summary>
        /// MinIO
        /// </summary>
        public const string Minio = "Minio";

        /// <summary>
        /// 本地文件
        /// </summary>
        public const string File = "File";

        #endregion


    }
}

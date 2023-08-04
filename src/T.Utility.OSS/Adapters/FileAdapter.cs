using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using T.Utility.Protocol;

/* 基于文件的对象存储封装：
 * 1、基础路径： Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files/oss");
 * 2、bucket和key的第一个片段没有严格的区分，所以bucket可以为空
 */

namespace T.Utility.OSS
{
    /// <summary>
    /// 本地文件对象存储封装
    /// </summary>
    public class FileAdapter : IAdapter
    {
        private readonly ILogger<FileAdapter> _logger;
        private readonly OssSettings _settings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="option"></param>
        public FileAdapter(ILogger<FileAdapter> logger, IOptions<OssSettings> option)
        {
            _logger = logger;
            _settings = option.Value;
        }

        /// <summary>
        /// Checks if the bucket exists
        /// </summary>
        /// <param name="bucket"></param>
        /// <returns>True when the bucket exists under the current user;Otherwise returns false.</returns>
        public async Task<ActionContent<bool>> DoesBucketExist(string bucket)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "oss", bucket);

                bool exist = Directory.Exists(path);
                return await Task.FromResult(new ActionContent<bool>() { State = 200, Value = exist });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "bucket检查失败");
                return new ActionContent<bool>() { State = 400, Desc = "bucket检查失败", Value = false };
            }
        }

        /// <summary>
        /// Creates a new bucket
        /// </summary>
        /// <param name="bucket"></param>
        /// <returns>True when success;Otherwise returns false.</returns>
        public async Task<ActionContent<bool>> CreateBucket(string bucket)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "oss", bucket);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return await Task.FromResult(new ActionContent<bool>() { State = 200, Value = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "bucket创建失败");
                return new ActionContent<bool>() { State = 400, Desc = "bucket创建失败", Value = false };
            }
        }

        /// <summary>
        /// Checks if the object exists
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns>True when the object exists;Otherwise returns false.</returns>
        public async Task<ActionContent<bool>> DoesObjectExist(string bucket, string key)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "oss", bucket, key);

                bool exist = File.Exists(path);
                return await Task.FromResult(new ActionContent<bool>() { State = 200, Value = exist });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "object检查失败");
                return new ActionContent<bool>() { State = 400, Desc = "object检查失败", Value = false };
            }
        }

        /// <summary>
        /// Gets object
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns>Returns the byte array when success;Otherwise returns null.</returns>
        public async Task<ActionContent<byte[]>> GetObject(string bucket, string key)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "oss", bucket, key);

                if (File.Exists(path))
                {
                    var bytes = await File.ReadAllBytesAsync(path);
                    return new ActionContent<byte[]>() { State = 200, Value = bytes };
                }
                else
                {
                    return new ActionContent<byte[]>() { State = 400, Desc = "The specified key does not exist." };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "object下载失败");
                return new ActionContent<byte[]>() { State = 400, Desc = "object下载失败" };
            }
        }

        /// <summary>
        /// Puts object to the specified bucket with specified object key
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <returns>Returns the url string when success;Otherwise returns null.</returns>
        public async Task<ActionContent<string>> PutObject(string bucket, string key, Stream content)
        {
            try
            {
                string bkt_key = Path.Combine(bucket, key);

                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "oss", bkt_key);

                var fileInfo = new FileInfo(path);

                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await content.CopyToAsync(fileStream);
                }

                return new ActionContent<string>() { State = 200, Value = $"file://{bkt_key.Replace('\\', '/')}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "object创建失败");
                return new ActionContent<string>() { State = 400, Desc = "object创建失败" };
            }
        }

        /// <summary>
        /// Deletes object
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns>True when success;Otherwise returns false.</returns>
        public async Task<ActionContent<bool>> DeleteObject(string bucket, string key)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "oss", bucket, key);

                File.Delete(path);

                return await Task.FromResult(new ActionContent<bool>() { State = 200, Value = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "object删除失败");
                return new ActionContent<bool>() { State = 400, Desc = "object删除失败", Value = false };
            }
        }

    }
}

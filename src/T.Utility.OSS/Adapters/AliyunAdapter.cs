using Aliyun.OSS;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using T.Utility.Protocol;
using static COSXML.Model.Tag.ListBucket;

namespace T.Utility.OSS
{
    /// <summary>
    /// 阿里云对象存储封装
    /// </summary>
    public class AliyunAdapter : IAdapter
    {
        private readonly ILogger<AliyunAdapter> _logger;
        private readonly OssSettings _settings;

        /// <summary>
        /// 锁对象
        /// </summary>
        private readonly object _o = new object();

        /// <summary>
        /// 客户端对象
        /// </summary>
        private OssClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="option"></param>
        public AliyunAdapter(ILogger<AliyunAdapter> logger, IOptions<OssSettings> option)
        {
            _logger = logger;
            _settings = option.Value;
        }

        /// <summary>
        /// Ensures that the client for the adapter exists. If it exists, no action is taken. If it does not exist then the client will be created.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool TryGetClient(out OssClient client)
        {
            try
            {
                lock (_o)
                {
                    if (_client == null)
                    {
                        _client = new OssClient(_settings.Endpoint, _settings.AccessKey, _settings.SecretKey);
                    }
                }

                client = _client;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "client创建失败");
                client = null;
                return false;
            }
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
                if (!TryGetClient(out var client))
                    return await Task.FromResult(new ActionContent<bool>() { State = 400, Desc = "client创建失败", Value = false });

                bool exist = client.DoesBucketExist(bucket);
                return new ActionContent<bool>() { State = 200, Value = exist };
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
                if (!TryGetClient(out var client))
                    return await Task.FromResult(new ActionContent<bool>() { State = 400, Desc = "client创建失败", Value = false });

                var instance = client.CreateBucket(bucket);
                return new ActionContent<bool>() { State = 200, Value = instance != null };
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
                if (!TryGetClient(out var client))
                    return await Task.FromResult(new ActionContent<bool>() { State = 400, Desc = "client创建失败", Value = false });

                bool exist = client.DoesObjectExist(bucket, key);
                return new ActionContent<bool>() { State = 200, Value = exist };
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
                if (!TryGetClient(out var client))
                    return await Task.FromResult(new ActionContent<byte[]>() { State = 400, Desc = "client创建失败" });

                var result = client.GetObject(bucket, key);

                using (MemoryStream stream = new MemoryStream())
                {
                    result.Content.CopyTo(stream);

                    return new ActionContent<byte[]>() { State = 200, Value = stream.ToArray() };
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
                if (!TryGetClient(out var client))
                    return await Task.FromResult(new ActionContent<string>() { State = 400, Desc = "client创建失败" });

                var result = client.PutObject(bucket, key, content);
                if (result.HttpStatusCode == HttpStatusCode.OK)
                {
                    string scheme = _settings.SSL ? "https" : "http";

                    return new ActionContent<string>() { State = 200, Value = $"{scheme}://{bucket}.{_settings.Endpoint}/{key}" };
                }
                else
                {
                    return new ActionContent<string>() { State = 400, Value = "object创建失败" };
                }
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
                if (!TryGetClient(out var client))
                    return await Task.FromResult(new ActionContent<bool>() { State = 400, Desc = "client创建失败", Value = false });

                var result = client.DeleteObject(bucket, key);
                if (result.HttpStatusCode == HttpStatusCode.NoContent)
                {
                    return new ActionContent<bool>() { State = 200, Value = true };
                }
                else
                {
                    return new ActionContent<bool>() { State = 400, Desc = "object删除失败" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "object删除失败");
                return new ActionContent<bool>() { State = 400, Desc = "object删除失败", Value = false };
            }
        }

        /// <summary>
        /// Get the object key from url which start with file:// or http:// or https://
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetObjectKey(string bucket, string url)
        {
            try
            {
                string scheme = _settings.SSL ? "https" : "http";

                string prefix = $"{scheme}://{bucket}.{_settings.Endpoint}/";

                return url.StartsWith(prefix) ? url.Replace(prefix, "") : string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "object key解析失败");
                return string.Empty;
            }
        }
    }
}

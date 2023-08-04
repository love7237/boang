using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OBS;
using OBS.Model;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using T.Utility.Protocol;

namespace T.Utility.OSS
{
    /// <summary>
    /// 华为云对象存储封装
    /// </summary>
    public class HuaweiAdapter : IAdapter
    {
        private readonly ILogger<HuaweiAdapter> _logger;
        private readonly OssSettings _settings;

        /// <summary>
        /// 锁对象
        /// </summary>
        private readonly object _o = new object();

        /// <summary>
        /// 客户端对象
        /// </summary>
        private ObsClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="option"></param>
        public HuaweiAdapter(ILogger<HuaweiAdapter> logger, IOptions<OssSettings> option)
        {
            _logger = logger;
            _settings = option.Value;
        }

        /// <summary>
        /// Ensures that the client for the adapter exists. If it exists, no action is taken. If it does not exist then the client will be created.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool TryGetClient(out ObsClient client)
        {
            try
            {
                lock (_o)
                {
                    if (_client == null)
                    {
                        _client = new ObsClient(_settings.AccessKey, _settings.SecretKey, _settings.Endpoint);
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

                HeadBucketRequest request = new HeadBucketRequest() { BucketName = bucket };

                bool exist = client.HeadBucket(request);
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

                CreateBucketRequest request = new CreateBucketRequest() { BucketName = bucket };

                var response = client.CreateBucket(request);
                return new ActionContent<bool>() { State = 200, Value = response.StatusCode == HttpStatusCode.OK };
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

                HeadObjectRequest request = new HeadObjectRequest() { BucketName = bucket, ObjectKey = key };

                bool exist = client.HeadObject(request);
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

                GetObjectRequest request = new GetObjectRequest() { BucketName = bucket, ObjectKey = key };

                var result = client.GetObject(request);

                using (MemoryStream stream = new MemoryStream())
                {
                    result.OutputStream.CopyTo(stream);

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

                PutObjectRequest request = new PutObjectRequest() { BucketName = bucket, ObjectKey = key, InputStream = content };

                var result = client.PutObject(request);
                if (result.StatusCode == HttpStatusCode.OK)
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

                DeleteObjectRequest request = new DeleteObjectRequest() { BucketName = bucket, ObjectKey = key };

                var result = client.DeleteObject(request);
                if (result.StatusCode == HttpStatusCode.NoContent)
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
    }
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using T.Utility.Protocol;

namespace T.Utility.OSS
{
    /// <summary>
    /// MinIO对象存储封装
    /// </summary>
    public class MinioAdapter : IAdapter
    {
        private readonly ILogger<MinioAdapter> _logger;
        private readonly OssSettings _settings;

        /// <summary>
        /// 锁对象
        /// </summary>
        private readonly object _o = new object();

        /// <summary>
        /// 客户端对象
        /// </summary>
        private MinioClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="option"></param>
        public MinioAdapter(ILogger<MinioAdapter> logger, IOptions<OssSettings> option)
        {
            _logger = logger;
            _settings = option.Value;
        }

        /// <summary>
        /// Ensures that the client for the adapter exists. If it exists, no action is taken. If it does not exist then the client will be created.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool TryGetClient(out MinioClient client)
        {
            try
            {
                lock (_o)
                {
                    if (_client == null)
                    {
                        _client = new MinioClient()
                                    .WithEndpoint(_settings.Endpoint)
                                    .WithCredentials(_settings.AccessKey, _settings.SecretKey)
                                    .WithSSL(_settings.SSL)
                                    .Build();
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

                var args = new BucketExistsArgs().WithBucket(bucket);

                bool exist = await client.BucketExistsAsync(args);
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

                var args = new MakeBucketArgs().WithBucket(bucket);

                await client.MakeBucketAsync(args);
                return new ActionContent<bool>() { State = 200, Value = true };
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

                var args = new StatObjectArgs().WithBucket(bucket).WithObject(key);

                var stat = await client.StatObjectAsync(args);
                return new ActionContent<bool>() { State = 200, Value = true };
            }
            catch (Exception ex)
            {
                if (ex is MinioException me && me.Response.Code == "NoSuchKey")
                {
                    return new ActionContent<bool>() { State = 200, Value = false };
                }

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

                using (MemoryStream stream = new MemoryStream())
                {
                    var args = new GetObjectArgs().WithBucket(bucket).WithObject(key).WithCallbackStream((outputStream) =>
                    {
                        outputStream.CopyTo(stream);
                    });

                    var result = await client.GetObjectAsync(args);

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

                var args = new PutObjectArgs().WithBucket(bucket).WithObject(key).WithStreamData(content).WithObjectSize(content.Length);

                await client.PutObjectAsync(args);

                string scheme = _settings.SSL ? "https" : "http";

                return new ActionContent<string>() { State = 200, Value = $"{scheme}://{_settings.Endpoint}/{bucket}/{key}" };
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

                var args = new RemoveObjectArgs().WithBucket(bucket).WithObject(key);

                var result = client.RemoveObjectAsync(args);

                return new ActionContent<bool>() { State = 200, Value = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "object删除失败");
                return new ActionContent<bool>() { State = 400, Desc = "object删除失败", Value = false };
            }
        }
    }
}

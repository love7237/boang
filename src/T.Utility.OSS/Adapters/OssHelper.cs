﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using T.Utility.Extensions;
using T.Utility.Http;
using T.Utility.Protocol;

namespace T.Utility.OSS
{
    /// <summary>
    /// 简单的对象存储封装
    /// </summary>
    public class OssHelper
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<OssHelper> _logger;
        private readonly OssSettings _settings;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="logger"></param>
        /// <param name="option"></param>
        /// <param name="httpClient"></param>
        public OssHelper(IServiceProvider provider, ILogger<OssHelper> logger, IOptions<OssSettings> option, HttpClient httpClient)
        {
            _provider = provider;
            _logger = logger;
            _settings = option.Value;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Ensures that the client for the adapter exists. If it exists, no action is taken. If it does not exist then the client will be created.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool TryGetClient(out IAdapter client)
        {
            try
            {
                switch (_settings.Adapter)
                {
                    case Adapter.Aliyun:
                        client = _provider.CreateScope().ServiceProvider.GetRequiredService<AliyunAdapter>();
                        return true;

                    case Adapter.Tencent:
                        client = _provider.CreateScope().ServiceProvider.GetRequiredService<TencentAdapter>();
                        return true;

                    case Adapter.Huawei:
                        client = _provider.CreateScope().ServiceProvider.GetRequiredService<HuaweiAdapter>();
                        return true;

                    case Adapter.Minio:
                        client = _provider.CreateScope().ServiceProvider.GetRequiredService<MinioAdapter>();
                        return true;

                    case Adapter.File:
                        client = _provider.CreateScope().ServiceProvider.GetRequiredService<FileAdapter>();
                        return true;

                    default:
                        client = null;
                        return false;
                }
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
        /// <returns>True when the bucket exists under the current user;Otherwise returns false.</returns>
        public async Task<ActionContent<bool>> DoesBucketExist()
        {
            try
            {
                if (!TryGetClient(out var client))
                    return await Task.FromResult(new ActionContent<bool>() { State = 400, Desc = "client创建失败", Value = false });

                return await client.DoesBucketExist(_settings.Bucket);
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
        /// <returns>True when success;Otherwise returns false.</returns>
        public async Task<ActionContent<bool>> CreateBucket()
        {
            try
            {
                if (!TryGetClient(out var client))
                    return await Task.FromResult(new ActionContent<bool>() { State = 400, Desc = "client创建失败", Value = false });

                return await client.CreateBucket(_settings.Bucket);
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
        /// <param name="key"></param>
        /// <returns>True when the object exists;Otherwise returns false.</returns>
        public async Task<ActionContent<bool>> DoesObjectExist(string key)
        {
            try
            {
                if (!TryGetClient(out var client))
                    return await Task.FromResult(new ActionContent<bool>() { State = 400, Desc = "client创建失败", Value = false });

                return await client.DoesObjectExist(_settings.Bucket, key);
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
        /// <param name="key"></param>
        /// <returns>Returns the byte array when success;Otherwise returns null.</returns>
        public async Task<ActionContent<byte[]>> GetObject(string key)
        {
            try
            {
                if (!TryGetClient(out var client))
                    return await Task.FromResult(new ActionContent<byte[]>() { State = 400, Desc = "client创建失败" });

                return await client.GetObject(_settings.Bucket, key);
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
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <returns>Returns the url string when success;Otherwise returns null.</returns>
        public async Task<ActionContent<string>> PutObject(string key, Stream content)
        {
            try
            {
                if (!TryGetClient(out var client))
                    return await Task.FromResult(new ActionContent<string>() { State = 400, Desc = "client创建失败" });

                return await client.PutObject(_settings.Bucket, key, content);
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
        /// <param name="key"></param>
        /// <returns>True when success;Otherwise returns false.</returns>
        public async Task<ActionContent<bool>> DeleteObject(string key)
        {
            try
            {
                if (!TryGetClient(out var client))
                    return await Task.FromResult(new ActionContent<bool>() { State = 400, Desc = "client创建失败", Value = false });

                return await client.DeleteObject(_settings.Bucket, key);
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
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetObjectKey(string url)
        {
            try
            {
                if (!TryGetClient(out var client))
                    return string.Empty;

                return client.GetObjectKey(_settings.Bucket, url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "object key解析失败");
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the network address from a file based oss url
        /// </summary>
        /// <param name="fileUrl">a file based oss url, such as: file://bucket/.../xxx.jpg</param>
        /// <returns>Returns the wrapped network address, such as: http://.../file/bucket/.../xxx.jpg</returns>
        public string GetNetworkUrl(string fileUrl)
        {
            try
            {
                if (_settings.Adapter == Adapter.File && fileUrl.StartsWith("file://"))
                {
                    return $"{_settings.Endpoint.TrimEnd('/')}/{fileUrl.Replace(":/", "")}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "地址转换失败");
            }

            return fileUrl;
        }

        /// <summary>
        /// Gets object with network address translation
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ActionContent<byte[]>> ProxyDownload(string url)
        {
            try
            {
                if (url.StartsWith("https://") || url.StartsWith("http://"))
                {
                    if (_settings.IntranetTransforms.IsNotNullOrEmpty())
                    {
                        foreach (var item in _settings.IntranetTransforms)
                        {
                            if (url.Contains(item.Source))
                            {
                                url = url.Replace(item.Source, item.Target);
                                break;
                            }
                        }
                    }

                    var httpResult = await _httpClient.SetBaseUri(url).GetByteArrayAsync();
                    if (httpResult.IsSuccess)
                    {
                        return new ActionContent<byte[]>() { State = 200, Value = httpResult.Content };
                    }
                    else
                    {
                        return new ActionContent<byte[]>() { State = 400, Desc = httpResult.Exception.Message };
                    }
                }
                else if (url.StartsWith("file://"))
                {
                    string objectKey = GetObjectKey(url);

                    if (!string.IsNullOrWhiteSpace(objectKey))
                    {
                        return await GetObject(objectKey);
                    }
                }

                return new ActionContent<byte[]>(400, "代理下载失败");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "代理下载失败");
                return new ActionContent<byte[]>() { State = 400, Desc = "代理下载失败" };
            }
        }
    }
}

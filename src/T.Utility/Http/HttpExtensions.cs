using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using T.Utility.Extensions;

namespace T.Utility.Http
{
    /// <summary>
    /// HTTP扩展类
    /// </summary>
    public static class HttpExtensions
    {
        /// <summary>
        /// 设置请求基址
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="uriString"></param>
        /// <returns></returns>
        public static HttpRequestContext SetBaseUri(this HttpClient httpClient, string uriString)
        {
            var context = new HttpRequestContext
            {
                BaseUri = new Uri(uriString),
                Client = httpClient
            };
            return context;
        }

        /// <summary>
        /// 追加请求路径
        /// </summary>
        /// <param name="context"></param>
        /// <param name="segments"></param>
        /// <returns></returns>
        public static HttpRequestContext AppendSegments(this HttpRequestContext context, params string[] segments)
        {
            if (segments != null)
            {
                context.Segments = context.Segments ?? new List<string>();

                foreach (var segment in segments)
                {
                    context.Segments.Add(segment.Trim('/').Trim('\\'));
                }
            }

            return context;
        }

        /// <summary>
        /// 设置请求头部信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static HttpRequestContext WithHeaders(this HttpRequestContext context, Dictionary<string, object> headers)
        {
            context.Headers = context.Headers ?? new Dictionary<string, object>();

            if (!context.Headers.ContainsKey("Accept"))
            {
                context.Headers.Add("Accept", "*/*");
            }

            if (!context.Headers.ContainsKey("User-Agent"))
            {
                context.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.124 Safari/537.36 Edg/102.0.1245.44");
            }

            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    if (context.Headers.ContainsKey(key))
                    {
                        context.Headers[key] = headers[key];
                    }
                    else
                    {
                        context.Headers.Add(key, headers[key]);
                    }
                }
            }

            return context;
        }

        /// <summary>
        /// 设置查询参数信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static HttpRequestContext WithQueryParams(this HttpRequestContext context, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                context.QueryParams = context.QueryParams ?? new Dictionary<string, object>();

                foreach (var key in parameters.Keys)
                {
                    if (context.QueryParams.ContainsKey(key))
                    {
                        context.QueryParams[key] = parameters[key];
                    }
                    else
                    {
                        context.QueryParams.Add(key, parameters[key]);
                    }
                }
            }

            return context;
        }

        /// <summary>
        /// 设置<see cref="StringContent" />类型的请求体(默认 application/json; charset=utf-8)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static HttpRequestContext WithContent(this HttpRequestContext context, string content)
        {
            if (content != null)
            {
                context.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }
            return context;
        }

        /// <summary>
        /// 设置<see cref="StringContent" />类型的请求体(默认 application/json; charset=utf-8)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static HttpRequestContext WithContent(this HttpRequestContext context, string content, Encoding encoding, string mediaType = "application/json")
        {
            if (content != null)
            {
                context.Content = new StringContent(content, encoding, mediaType);
            }
            return context;
        }

        /// <summary>
        /// 设置请求体
        /// </summary>
        /// <param name="context"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static HttpRequestContext WithContent(this HttpRequestContext context, HttpContent content)
        {
            if (content != null)
            {
                context.Content = content;
            }
            return context;
        }

        /// <summary>
        /// 创建<see cref="HttpRequestMessage"/>对象
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static HttpRequestMessage CreateHttpRequestMessage(this HttpRequestContext context)
        {
            //请求地址
            string relativeUri = string.Empty;

            if (context.Segments.IsNotNullOrEmpty())
            {
                relativeUri = string.Join("/", context.Segments);
            }

            if (context.QueryParams.IsNotNullOrEmpty())
            {
                string queryString = string.Join("&", context.QueryParams.Select(x => $"{x.Key}={x.Value}"));
                relativeUri = $"{relativeUri}?{queryString}";
            }

            context.RequestUri = new Uri(context.BaseUri, relativeUri);

            //请求消息
            var request = new HttpRequestMessage { Method = context.Method, RequestUri = context.RequestUri, Content = context.Content };

            //请求头
            if (context.Headers.IsNotNullOrEmpty())
            {
                foreach (var key in context.Headers.Keys)
                {
                    if (request.Headers.Contains(key))
                    {
                        request.Headers.Remove(key);
                    }
                    request.Headers.Add(key, context.Headers[key].ToString());
                }
            }

            return request;
        }

        /// <summary>
        /// 返回HTTP响应消息
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="request"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        private static async Task<HttpResponseMessage> GetResponseAsync(this HttpClient httpClient, HttpRequestMessage request, CancellationTokenSource cts = null)
        {
            if (cts != null)
            {
                return await httpClient.SendAsync(request, cts.Token);
            }
            else
            {
                return await httpClient.SendAsync(request);
            }
        }

        /// <summary>
        /// 获取指定格式的响应信息
        /// </summary>
        /// <typeparam name="T">泛型(string、byte[]、Stream)</typeparam>
        /// <param name="context"></param>
        /// <param name="cts"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        private static async Task<HttpResult<T>> GetResponseAsync<T>(this HttpRequestContext context, CancellationTokenSource cts = null, int attempts = 1)
        {
            int times = 0;
            attempts = Math.Max(attempts, 1);

            var httpResult = new HttpResult<T>() { Request = context, Response = new HttpResponseContext<T>() };

            do
            {
                times++;

                try
                {
                    using (HttpRequestMessage request = context.CreateHttpRequestMessage())
                    {
                        using (HttpResponseMessage response = await context.Client.GetResponseAsync(request, cts))
                        {
                            httpResult.Response.IsSuccessStatusCode = response.IsSuccessStatusCode;
                            httpResult.Response.StatusCode = response.StatusCode;
                            httpResult.Response.ReasonPhrase = response.ReasonPhrase;
                            httpResult.Response.Version = response.Version;
                            httpResult.Response.Headers = response.Headers;

                            if (response.IsSuccessStatusCode)
                            {
                                httpResult.Response.ContentHeaders = response.Content.Headers;

                                if (httpResult is HttpResult<string> stringHttpResult)
                                {
                                    stringHttpResult.Response.Content = await response.Content.ReadAsStringAsync();
                                }
                                else if (httpResult is HttpResult<byte[]> bytesHttpResult)
                                {
                                    bytesHttpResult.Response.Content = await response.Content.ReadAsByteArrayAsync();
                                }
                                else if (httpResult is HttpResult<Stream> streamHttpResult)
                                {
                                    streamHttpResult.Response.Content = await response.Content.ReadAsStreamAsync();
                                }
                            }
                            else
                            {
                                string message = await response.Content.ReadAsStringAsync();
                                if (string.IsNullOrWhiteSpace(message))
                                {
                                    message = ((int)response.StatusCode).ToString();
                                }
                                httpResult.Exception = new StatusCodeException(message);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    httpResult.Exception = ex;

                    if (ex is TaskCanceledException && cts != null && cts.IsCancellationRequested)
                        break;
                }

                if (httpResult.IsSuccess == false && times < attempts)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500 * Math.Pow(2, times)));
                }

            } while (httpResult.IsSuccess == false && times < attempts);

            return httpResult;
        }

        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a string in an asynchronous operation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cts"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public static async Task<HttpResult<string>> GetStringAsync(this HttpRequestContext context, CancellationTokenSource cts = null, int attempts = 1)
        {
            context.Method = HttpMethod.Get;

            return await context.GetResponseAsync<string>(cts, attempts);
        }

        /// <summary>
        /// Sends a GET request to the specified Uri and return the response body as a byte array in an asynchronous operation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cts"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public static async Task<HttpResult<byte[]>> GetByteArrayAsync(this HttpRequestContext context, CancellationTokenSource cts = null, int attempts = 1)
        {
            context.Method = HttpMethod.Get;

            return await context.GetResponseAsync<byte[]>(cts, attempts);
        }

        /// <summary>
        /// Sends a GET request to the specified Uri and return the response body as a stream in an asynchronous operation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cts"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public static async Task<HttpResult<Stream>> GetStreamAsync(this HttpRequestContext context, CancellationTokenSource cts = null, int attempts = 1)
        {
            context.Method = HttpMethod.Get;

            return await context.GetResponseAsync<Stream>(cts, attempts);
        }

        /// <summary>
        /// Send a POST request to the specified Uri and return the response body as a string in an asynchronous operation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cts"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public static async Task<HttpResult<string>> PostAsync(this HttpRequestContext context, CancellationTokenSource cts = null, int attempts = 1)
        {
            context.Method = HttpMethod.Post;

            return await context.GetResponseAsync<string>(cts, attempts);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri and return the response body as a string in an asynchronous operation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cts"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public static async Task<HttpResult<string>> PutAsync(this HttpRequestContext context, CancellationTokenSource cts = null, int attempts = 1)
        {
            context.Method = HttpMethod.Put;

            return await context.GetResponseAsync<string>(cts, attempts);
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri and return the response body as a string in an asynchronous operation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cts"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public static async Task<HttpResult<string>> DeleteAsync(this HttpRequestContext context, CancellationTokenSource cts = null, int attempts = 1)
        {
            context.Method = HttpMethod.Delete;

            return await context.GetResponseAsync<string>(cts, attempts);
        }

    }
}

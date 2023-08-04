using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;
using T.Utility.Http;

namespace T.Utility.Algorithms
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static class HttpResultExtensions
    {
        /// <summary>
        /// 触发异常和非期望结果的请求日志
        /// </summary>
        /// <param name="httpResult"></param>
        /// <param name="logger"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public static void Log(this HttpResult<string> httpResult, ILogger logger, string title, string body, LogLevel logLevel = LogLevel.Error)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"地址：{httpResult.Request.RequestUri.AbsoluteUri}");

                if (!string.IsNullOrWhiteSpace(title))
                {
                    sb.AppendLine($"描述：{title}");
                }

                if (!string.IsNullOrWhiteSpace(body))
                {
                    sb.AppendLine($"请求体：{body}");
                }

                if (httpResult.Response != null)
                {
                    sb.AppendLine($"状态码：{httpResult.Response.StatusCode} ({(int)httpResult.Response.StatusCode})");
                }

                if (httpResult.Response?.Content != null)
                {
                    sb.AppendLine($"响应体：{httpResult.Content}");
                }

                if (httpResult.Exception != null)
                {
                    switch (httpResult.Exception)
                    {
                        case TaskCanceledException _:
                            sb.AppendLine($"System.Threading.Tasks.TaskCanceledException: {httpResult.Exception.Message}");
                            logger.LogWarning(sb.ToString());
                            break;

                        case StatusCodeException _:
                            logger.Log(logLevel, httpResult.Exception, sb.ToString());
                            break;

                        default:
                            logger.Log(logLevel, httpResult.Exception, sb.ToString());
                            break;
                    }
                }
                else
                {
                    logger.Log(logLevel, sb.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"{nameof(HttpResultExtensions)}.{nameof(Log)}");
            }
        }
    }
}

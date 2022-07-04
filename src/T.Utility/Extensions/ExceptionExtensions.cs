using System;
using System.Collections.Generic;

namespace T.Utility.Extensions
{
    /// <summary>
    /// 异常处理扩展类
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 获取自上至下的异常传递列表
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static IEnumerable<Exception> RouteExceptions(this Exception exception)
        {
            List<Exception> routeExceptions = new List<Exception>();

            Exception innerException = exception;
            do
            {
                routeExceptions.Add(innerException);
                yield return innerException;
            } while ((innerException = innerException.InnerException) != null);
        }

        /// <summary>
        /// 获取最底层异常提示信息
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string InnerMessage(this Exception exception)
        {
            Exception innerException = exception;

            while (innerException.InnerException != null)
            {
                innerException = innerException.InnerException;
            }

            return innerException.Message;
        }
    }
}

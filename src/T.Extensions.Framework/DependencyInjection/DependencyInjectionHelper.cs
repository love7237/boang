using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace T.Extensions.Framework.DependencyInjection
{
    /// <summary>
    /// 依赖注入辅助类
    /// </summary>
    public static class DependencyInjectionHelper
    {
        /// <summary>
        /// The object that provides custom support to other objects.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; }

        static DependencyInjectionHelper()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpClient();

            serviceCollection.AddMemoryCache();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Get service of type T from the System.IServiceProvider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : class
        {
            return ServiceProvider.GetService<T>();
        }

        /// <summary>
        /// 基于依赖注入的，由<see cref="IHttpClientFactory" />管理的<see cref="System.Net.Http.HttpClient" />对象
        /// </summary>
        public static HttpClient HttpClient => GetService<IHttpClientFactory>().CreateClient();

        /// <summary>
        /// 基于依赖注入的<see cref="Microsoft.Extensions.Caching.Memory.MemoryCache" />对象
        /// </summary>
        public static IMemoryCache MemoryCache => GetService<IMemoryCache>();
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace T.Utility.Algorithms
{
    /// <summary>
    /// 
    /// </summary>
    public static class AlgorithmExtensions
    {
        /// <summary>
        /// 注册算法模块
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAlgorithms(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<List<AlgorithmService>>(configuration.GetSection(nameof(AlgorithmService)));
            services.AddHttpClient<AlgorithmHttpClient>();
            return services;
        }
    }
}

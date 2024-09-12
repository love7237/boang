using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace T.Utility.OSS
{
    /// <summary>
    /// 
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 注册对象存储模块
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddOSS(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OssSettings>(configuration.GetSection(nameof(OssSettings)));

            services.AddSingleton<AliyunAdapter>();
            services.AddSingleton<TencentAdapter>();
            services.AddSingleton<HuaweiAdapter>();
            services.AddSingleton<MinioAdapter>();
            services.AddSingleton<FileAdapter>();

            services.AddSingleton<OssHelper>();

            return services;
        }
    }
}

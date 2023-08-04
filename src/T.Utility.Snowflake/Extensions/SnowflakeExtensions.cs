using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace T.Utility.Snowflake
{
    /// <summary>
    /// 
    /// </summary>
    public static class SnowflakeExtensions
    {
        /// <summary>
        /// 注册雪花Id模块
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSnowflake(this IServiceCollection services, IConfiguration configuration)
        {
            var snowflakeSettings = configuration.GetSection(nameof(SnowflakeSettings)).Get<SnowflakeSettings>();

            if (snowflakeSettings == null)
            {
                snowflakeSettings = new SnowflakeSettings();
            }

            services.AddSingleton(snowflakeSettings);
            services.AddSingleton<SnowflakeHelper>();
            return services;
        }

    }
}

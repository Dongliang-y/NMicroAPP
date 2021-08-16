using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ZJJWEPlatform.Config;
using ZJJWEPlatform.Filters;
using ZJJWEPlatform.Infrastructure;

namespace ZJJWEPlatform.DI
{
    /// <summary>
    /// 缓存依赖注入
    /// </summary>
    public static class CacheDependencyInjectionExtensions
    {
        internal static IServiceProvider ServiceProvider;

        /// <summary>
        /// 添加redis缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config">redis配置信息</param>
        /// <param name="resetRedisSettingDelegate">当缓存实例没有找到配置时，返回的处理逻辑</param>
        public static void AddRedisCache(this IServiceCollection services, RedisConfig config, ResetRedisSettingDelegate resetRedisSettingDelegate = null)
        {

            services.AddSingleton<RedisCacheInterceptor>((svc) =>
            {
                var instance = new RedisCacheInterceptor(new RedisHelper(config.Pipe), config.EnableCacheInterceptor);
                if (resetRedisSettingDelegate != null)
                {
                    instance.GetRedisSettingDelegate = resetRedisSettingDelegate;
                }
                return instance;
            });
        }
    }


}

/********************************************************************************

** auth： DongliangYi

** date： 2016/8/31 15:07:14

** desc： Memcached 配置文件实例

** Ver.:  V1.0.0

*********************************************************************************/

using Com.Ctrip.Framework.Apollo;
using Newtonsoft.Json;
using NPlatform.Config;

namespace NPlatform.Config
{
    public sealed class RedisConfig : IConfig
    {
        private string redisType;

        /// <summary>
        /// Gets or sets redis 安装模式，Normal 普通，Twemproxy 代理，Sentinel 哨兵
        /// </summary>
        public string RedisType
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(RedisConfig),nameof(RedisType), this.redisType);
            }
            set
            {
                this.redisType = value;
            }
        }

        private string[] connections;
        /// <summary>
        /// Gets or sets 连接字符串  ,  [IP:port]
        /// </summary>

        public string[] Connections
        {
            get
            {
                var conns= ApolloConfiguration.GetConfig(nameof(RedisConfig), nameof(Connections),"");
                if(string.IsNullOrEmpty(conns))
                {
                    return new string[0];
                }
                else
                {
                    return conns.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
                }
            }
            set { connections = value; }
        }

        private string password;
        /// <summary>
        /// redis 密码
        /// </summary>
        public string Password
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(RedisConfig), nameof(Password), this.password);
            }
            set { password = value; }
        }

        private bool allowAdmin;
        /// <summary>
        /// AllowAdmin
        /// </summary>
        public bool AllowAdmin
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(RedisConfig), nameof(AllowAdmin), this.allowAdmin);
            }
            set { allowAdmin = value; }
        }

        private int pipe=15;
        /// <summary>
        /// redis 管道
        /// </summary>
        public int Pipe
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(RedisConfig), nameof(Pipe), this.pipe);
            }
            set { pipe = value; }
        }

        private bool enableCacheInterceptor = false;

        /// <summary>
        /// 是否开启缓存方法拦截
        /// </summary>
        public bool EnableCacheInterceptor
        {
            get
            {
                return ApolloConfiguration.GetConfig(nameof(RedisConfig), nameof(EnableCacheInterceptor), this.enableCacheInterceptor);
            }
            set { enableCacheInterceptor = value; }
        }
    }
}
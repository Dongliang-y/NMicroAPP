#region << 版 本 注 释 >>

/*----------------------------------------------------------------
* 项目名称 ：NPlatform.Infrastructure.Redis
* 类 名 称 ：ComRedisEngine
* 类 描 述 ：通用Redis公共辅助类库
* 命名空间 ：NPlatform.Infrastructure.Redis
* CLR 版本 ：4.0.30319.42000
* 作    者 ：YanZhou
* 创建时间 ：2018-12-21 16:36:58
* 更新时间 ：2018-12-21 16:36:58
* 版 本 号 ：v1.0.0.0
//----------------------------------------------------------------*/

#endregion

namespace NPlatform.Infrastructure.Redis { 
    using NPlatform.Config;

    /// <summary>
    /// 通用Redis缓存
    /// </summary>
    public class ComRedis
    {
        /// <summary>
        /// 通用Redis单例
        /// </summary>
        private static ComRedis engine;

        private static RedisConfig config = new ConfigFactory<RedisConfig>().Build();

        private static object engineLock = new object();

        private static RedisHelper redis;

        private ComRedis()
        {
            redis = new RedisHelper(10);
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>对象</returns>
        public static ComRedis Instance
        {
            get
            {
                // 单例缓存会错乱 
                // lock (engineLock)
                // {
                // if (engine == null)
                // {
                engine = new ComRedis();

                // }
                // else
                // {
                // return engine;
                // }
                return engine;

                // }
            }
        }

        /// <summary>
        /// 将缓存对象以json形式存储
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="t">值对象</param>
        /// <param name="group">add添加到指定组，获取就从指定组获取</param>
        public void Add<T>(string key, T t, string group = "")
            where T : class
        {
            if (group != string.Empty)
            {
                key = $"{group}_{key}";
            }

            redis.StringSet<T>(key, t);
        }

        /// <summary>
        /// 将缓存对象以json形式存储
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="t">值对象</param>
        /// <param name="group">add添加到指定组，获取就从指定组获取</param>
        public void Add<T>(string key, T t,int timeout, string group = "")
        {
            if (group != string.Empty)
            {
                key = $"{group}_{key}";
            }

            redis.StringSet<T>(key, t, new System.TimeSpan(0,0,timeout));
        }

        /// <summary>
        /// 获取泛型对象信息
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="group">获取指定组的缓存，add添加到指定组，获取就从指定组获取</param>
        /// <returns>T</returns>
        public T Get<T>(string key, string group = "")
        {
            if (group != string.Empty)
            {
                key = $"{group}_{key}";
            }

            return redis.StringGet<T>(key);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="group">从指定组里删除指定键的缓存</param>
        public void Remove(string key, string group = "")
        {
            if (group != string.Empty)
            {
                key = $"{group}_{key}";
            }

            redis.KeyDelete(key);
        }

        /// <summary>
        /// 根据组名清空缓存
        /// </summary>
        /// <param name="group">组名</param>
        public void RemoveGroup(string group)
        {
            redis.KeyDeletePattern($"{group}_");
        }
    }
}